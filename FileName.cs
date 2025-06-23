using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Calculator_for_nick
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WelcomeMessage();
            CalculatorLoop();
        }

        static void WelcomeMessage()
        {
            Console.WriteLine("Enter mathematical your expression please.");
            Console.WriteLine("Supported operations: +, -, *, /");
        }

        static void CalculatorLoop()
        {
            while (true)
            {
                string input = GetInput("Enter the expression: ");
                ProcessExpression(input);
            }
        }

        static string GetInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }

        static void ProcessExpression(string expression)
        {
            try
            {
                string cleanedInput = CleanExpression(expression);

                float result = EvaluateMathematicalExpression(cleanedInput);

                DisplayResult(expression, result);
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Input Error: " + ex.Message);
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine("Error!! Cannot divide by zero.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }

        static string CleanExpression(string expression)
        {
            return expression.Replace(" ", "");
        }

        static void DisplayResult(string originalExpression, float result)
        {
            Console.WriteLine("Result: " + originalExpression + " = " + result);
        }

        static float EvaluateMathematicalExpression(string expression)
        {
            List<string> tokens = TokenizeExpression(expression);

            List<float> numbers = new List<float>();
            List<char> operators = new List<char>();

            PopulateNumbersAndOperators(tokens, numbers, operators);

            ProcessHighPrecedenceOperations(numbers, operators);

            return ProcessLowPrecedenceOperations(numbers, operators);
        }

        static List<string> TokenizeExpression(string expression)
        {
            List<string> rawTokens = new List<string>();

            string pattern = @"(\d+(?:\.\d+)?)|([+\-*/])|(.)";
            MatchCollection matches = Regex.Matches(expression, pattern);

            int lastIndex = 0;

            foreach (Match m in matches)
            {
                if (m.Index > lastIndex)
                {
                    throw new FormatException("Invalid character sequence detected: '" + expression.Substring(lastIndex, m.Index - lastIndex) + "'.");
                }

                if (m.Groups[3].Success)
                {
                    throw new FormatException("Invalid character: '" + m.Value + "'.");
                }

                rawTokens.Add(m.Value);
                lastIndex = m.Index + m.Length;
            }

            if (lastIndex < expression.Length)
            {
                throw new FormatException("Invalid trailing characters: '" + expression.Substring(lastIndex) + "'.");
            }

            List<string> finalTokens = new List<string>();
            for (int i = 0; i < rawTokens.Count; i++)
            {
                string currentToken = rawTokens[i];

                if (currentToken == "-")
                {
                    if (i + 1 < rawTokens.Count && float.TryParse(rawTokens[i + 1], out _))
                    {
                        bool isUnary = false;
                        if (i == 0)
                        {
                            isUnary = true;
                        }
                        else
                        {
                            string prevToken = rawTokens[i - 1];
                            if (IsOperator(prevToken[0]) && prevToken != "-")
                            {
                                isUnary = true;
                            }
                            else if (prevToken == "-")
                            {
                                finalTokens[finalTokens.Count - 1] = "+";
                                finalTokens.Add(rawTokens[i + 1]);
                                i++;
                                continue;
                            }
                        }

                        if (isUnary)
                        {
                            finalTokens.Add("-" + rawTokens[i + 1]);
                            i++;
                            continue;
                        }
                    }
                }

                finalTokens.Add(currentToken);
            }

            return finalTokens;
        }


        static void PopulateNumbersAndOperators(List<string> tokens, List<float> numbers, List<char> operators)
        {
            foreach (string token in tokens)
            {
                if (float.TryParse(token, out float number))
                {
                    numbers.Add(number);
                }
                else if (token.Length == 1 && IsOperator(token[0]))
                {
                    operators.Add(token[0]);
                }
                else
                {
                    throw new FormatException("Invalid number or operator format: '" + token + "'. Please check your expression for typos or unexpected characters.");
                }
            }
        }

        static void ProcessHighPrecedenceOperations(List<float> numbers, List<char> operators)
        {
            for (int i = 0; i < operators.Count; i++)
            {
                char op = operators[i];
                if (op == '*' || op == '/')
                {
                    if (numbers.Count <= i + 1)
                    {
                        throw new FormatException("Invalid expression: Missing operand for multiplication/division.");
                    }

                    float num1 = numbers[i];
                    float num2 = numbers[i + 1];
                    float result;

                    if (op == '*')
                    {
                        result = num1 * num2;
                    }
                    else
                    {
                        if (num2 == 0)
                        {
                            throw new DivideByZeroException();
                        }
                        result = num1 / num2;
                    }

                    numbers[i] = result;
                    numbers.RemoveAt(i + 1);
                    operators.RemoveAt(i);
                    i--;
                }
            }
        }

        static float ProcessLowPrecedenceOperations(List<float> numbers, List<char> operators)
        {
            if (!numbers.Any())
            {
                throw new FormatException("Invalid expression: No numbers to calculate.");
            }

            float finalResult = numbers[0];

            for (int i = 0; i < operators.Count; i++)
            {
                if (numbers.Count <= i + 1)
                {
                    throw new FormatException("Invalid expression: Missing operand for addition/subtraction.");
                }

                char op = operators[i];
                float nextNumber = numbers[i + 1];

                if (op == '+')
                {
                    finalResult += nextNumber;
                }
                else if (op == '-')
                {
                    finalResult -= nextNumber;
                }
                else
                {
                    throw new InvalidOperationException("Unexpected operator " + op + " after precedence handling.");
                }
            }

            return finalResult;
        }

        static bool IsOperator(char c)
        {
            return c == '+' || c == '-' || c == '*' || c == '/';
        }
    }
}