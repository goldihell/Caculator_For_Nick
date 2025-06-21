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
                
                string correctedExpression = CorrectDecimalFormat(expression);
                string cleanedInput = CleanExpression(correctedExpression); 

                float result = EvaluateMathematicalExpression(cleanedInput); 
               
                DisplayResult(expression, result);
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Input Error:"+ ex.Message);
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine("Error!! Cannot divide by zero.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred:"+ ex.Message);
            }
        }

        
        static string CorrectDecimalFormat(string expression)
        {

            return Regex.Replace(expression, @"(?<!\d)\.(\d+)", "0.$1");
        }


        static string CleanExpression(string expression)
        {
            return expression.Replace(" ", "");
        }


        static void DisplayResult(string originalExpression, float result)
        {
            Console.WriteLine("Result:"+ originalExpression+"="+ result);
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
            var tokens = Regex.Matches(expression, @"(?<=\A-|\D-)?(\d+\.?\d*)|([+\-*/])")
                                 .Cast<Match>()
                                 .Select(m => m.Value)
                                 .ToList();

            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i] == "-" && i + 1 < tokens.Count && float.TryParse(tokens[i + 1], out _))
                {
                    if (i == 0 || (i > 0 && IsOperator(tokens[i - 1][0])))
                    {
                        tokens[i] = "-" + tokens[i + 1];
                        tokens.RemoveAt(i + 1);
                    }
                }
            }

            return tokens;
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
                    throw new FormatException("Invalid token "+ token +"found in expression.");
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
                    throw new InvalidOperationException("Unexpected operator "+op+ "after precedence handling.");
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