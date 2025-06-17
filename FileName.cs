using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator_for_nick
{
    internal class Program
    {
        enum CalculatorActions { None, Add, Subtract, Multiply, Divide }

        static void Main(string[] args)
        {
            Console.WriteLine("Enter numbers or action you want please (+, -, *, /,=)");

            float currentResult = 0;
            CalculatorActions lastAction = CalculatorActions.None;
            bool expectingNumber = true;

            List<string> expressionParts = new List<string>();

            while (true)
            {
                if (expectingNumber)
                {
                    Console.Write("Current result:" + currentResult+". Enter a number: ");
                }
                else
                {
                    Console.Write("Current result:"+ currentResult+ ". Enter an action (+, -, *, /,=):");
                }

                string input = Console.ReadLine().ToLower().Trim();


                if (input == "=")
                {
                    if (expectingNumber && lastAction != CalculatorActions.None)
                    {
                        Console.WriteLine("Error you can't end with action. enter number");
                        continue;
                    }
                    else if (!expressionParts.Any())
                    {
                        Console.WriteLine("Please enter number before you enter =");
                        continue;
                    }

                    Console.WriteLine(string.Join("", expressionParts)+"="+currentResult);

                    currentResult = 0;
                    lastAction = CalculatorActions.None;
                    expectingNumber = true;
                    expressionParts.Clear(); ;
                    continue;
                }

                if (expectingNumber)
                {
                    bool isNumber = float.TryParse(input, out float numberInput);
                    if (isNumber)
                    {
                        if (lastAction == CalculatorActions.None)
                        {
                            currentResult = numberInput;
                        }
                        else
                        {
                            currentResult = CalculateResult(lastAction, currentResult, numberInput);
                        }
                        expressionParts.Add(input); 
                        expectingNumber = false; 
                        lastAction = CalculatorActions.None; 
                    }
                    else
                    {
                        Console.WriteLine("It is not relevant");
                    }
                }
                else 
                {
                    CalculatorActions newAction = GetActionFromString(input);
                    if (newAction != CalculatorActions.None)
                    {
                        lastAction = newAction; 
                        expressionParts.Add(GetActionSymbol(newAction)); 
                        expectingNumber = true; 
                    }
                    else
                    {
                        Console.WriteLine("Error!!! Please enter action (+, -, *, /,=) ");
                    }
                }
            }
        }

        static CalculatorActions GetActionFromString(string input)
        {
            switch (input)
            {
                case "+":
                    return CalculatorActions.Add;
                case "-":
                    return CalculatorActions.Subtract;
                case "*":
                    return CalculatorActions.Multiply;
                case "/":
                    return CalculatorActions.Divide;
                default:
                    return CalculatorActions.None; 
            }
        }

        static string GetActionSymbol(CalculatorActions action)
        {
            switch (action)
            {
                case CalculatorActions.Add:
                    return "+";
                case CalculatorActions.Subtract:
                    return "-";
                case CalculatorActions.Multiply:
                    return "*";
                case CalculatorActions.Divide:
                    return "/";
                default:
                    return ""; 
            }
        }

        static float CalculateResult(CalculatorActions action, float num1, float num2)
        {
            switch (action)
            {
                case CalculatorActions.Add:
                    return num1 + num2;
                case CalculatorActions.Subtract:
                    return num1 - num2;
                case CalculatorActions.Multiply:
                    return num1 * num2;
                case CalculatorActions.Divide:
                    if (num2 == 0)
                    {
                        Console.WriteLine("Error!!! you can't divide by zero!");
                        return 0;
                    }
                    return num1 / num2;
                default:
                    Console.WriteLine("Error!!! Unknown action for calculation.");
                    return num1;
            }
        }
    }
}