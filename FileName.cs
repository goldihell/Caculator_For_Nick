using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace caculator_for_nick
{
    internal class Program
    {
        enum CalculatorActions { Add, Subtract, Multiply, Divide }

        static void Main(string[] args)
        {
            Console.WriteLine("Enter the first number please:");
            float num1 = GetNumber();

            Console.WriteLine("Choose the action you want please (+, -, *, /):");
            CalculatorActions action = GetAction();

            Console.WriteLine("Enter the second number please:");
            float num2 = GetNumber();

            float result = CalculateResult(action, num1, num2);

            Console.WriteLine(result);
        }

        static float GetNumber()
        {
            while (true)
            {
                string input = Console.ReadLine();

                bool success = float.TryParse(input, out float number);

                if (success && !float.IsNaN(number))
                {
                    return number;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number:");
                }
            }
        }

        static CalculatorActions GetAction()
        {
            while (true)
            {
                string input = Console.ReadLine();

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
                        Console.WriteLine("Invalid action. Please enter one of (+, -, *, /):");
                        break;
                }
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
                        return 80085;
                    }
                    return num1 / num2;
                default:
                    return 80085;
            }
        }
    }
}