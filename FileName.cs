using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data; 

namespace Calculator_for_nick
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter numbers or action you want please (+, -, *, /,=)");

            List<string> expressionParts = new List<string>();
            bool expectingNumber = true;

            while (true)
            {
                if (expectingNumber)
                {
                    Console.Write("Current expression: " + string.Join(" ", expressionParts) + ". Enter a number: ");
                }
                else
                {
                    Console.Write("Current expression: " + string.Join(" ", expressionParts) + ". Enter an action (+, -, *, /,=):");
                }

                string input = Console.ReadLine().ToLower().Trim();

                if (input == "=")
                {
                    if (expressionParts.Count == 0)
                    {
                        Console.WriteLine("Please enter number before you enter =");
                        continue;
                    }



                    string fullExpression = string.Join("", expressionParts);

                    try
                    {
                        DataTable dt = new DataTable();
                        var result = dt.Compute(fullExpression, string.Empty);

                        Console.WriteLine(fullExpression+"="+result);

                        expressionParts.Clear();
                        expectingNumber = true;
                        continue;
                    }
                    catch (SyntaxErrorException ex)
                    {
                        Console.WriteLine("Error in expression: "+ex.Message+". Please check your input.");
                    }
                    catch (DivideByZeroException)
                    {
                        Console.WriteLine("Error!!! you can't divide by zero!");
                    }
                    expressionParts.Clear();
                    expectingNumber = true;
                    Console.WriteLine("Starting a new calculation.");
                    continue;
                }

                float numberInput;
                bool isNumber = float.TryParse(input, out numberInput);

                if (expectingNumber)
                {
                    if (isNumber)
                    {
                        expressionParts.Add(input);
                        expectingNumber = false;
                    }
                    else
                    {
                        Console.WriteLine("It is not relevant");
                    }
                }
                else
                {
                    if (input == "+" || input == "-" || input == "*" || input == "/")
                    {
                        expressionParts.Add(input);
                        expectingNumber = true;
                    }
                    else
                    {
                        Console.WriteLine("Error!!! Please enter action (+, -, *, /,=) ");
                    }
                }
            }
        }
    }
}