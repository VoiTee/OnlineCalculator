﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ServerFunctionality
{
    public class Parser
    {
        private string userInput;
        private List<double> numbers = new List<double>();
        private List<char> operations = new List<char>();
        private double result;

        public Parser(string userInput)
        {
            this.userInput = userInput;
            parseInput();
            CalculationEngine.FixOrder(ref numbers, ref operations);
            calculateResult();
        }
        public Parser() { }

        private void parseInput()
        {
            int inputSize = userInput.Length;
            while (userInput.IndexOf("sin", 0, inputSize - 1) != -1 ||
                userInput.IndexOf("cos", 0, inputSize - 1) != -1 ||
                userInput.IndexOf("tan", 0, inputSize - 1) != -1 ||
                userInput.IndexOf("ctg", 0, inputSize - 1) != -1)
            {
                solveTrigonometric();
                inputSize = userInput.Length;
            }

            for (int i = 0; i < inputSize; i++)
            {
                double tmp = 0.0, num = 0.0;
                bool isNum = false, isDouble = false;
                string tmp_str = "";
                // if checked element is a number, count how many elements long it is
                while (i + (int)tmp < inputSize && (Char.IsNumber(userInput, i + (int)tmp) ||
                    (i + (int)tmp > 0 && Char.IsNumber(userInput, i + (int)tmp - 1) && userInput.ElementAt(i + (int)tmp) == ',' && Char.IsNumber(userInput, i + (int)tmp + 1))))
                {
                    if (userInput.ElementAt(i + (int)tmp) == '.')
                        isDouble = true;
                    tmp++;
                    isNum = true;
                }
                // if number was found above, convert it to double and add it to vector
                if (isNum == true)
                {
                    for (int k = i; k <= (int)tmp + i - 1; k++)
                    {
                        tmp_str += userInput.ElementAt(k);
                    }
                    numbers.Add(Convert.ToDouble(tmp_str));
                    i--;
                }
                // else it means that our element is an operation character
                else
                {
                    switch (userInput.ElementAt(i))
                    {
                        case '+': operations.Add('+'); break;
                        case '-': operations.Add('-'); break;
                        case '*': operations.Add('*'); break;
                        case '/': operations.Add('/'); break;
                        case '^': operations.Add('^'); break;
                        default: throw new FormatException("Wrong operation."); break;
                    }
                }
                i += (int)tmp;
            }
        }

        private void solveTrigonometric()
        {
            int inputSize = userInput.Length;
            int startIndex = 0;
            string trig = "";
            if (userInput.IndexOf("sin", 0, inputSize - 1) != -1)
            {
                startIndex = userInput.IndexOf("sin", 0, inputSize - 1) + 4;
                trig = "sin";
            }
            else if (userInput.IndexOf("cos", 0, inputSize - 1) != -1)
            {
                startIndex = userInput.IndexOf("cos", 0, inputSize - 1) + 4;
                trig = "cos";
            }
            else if (userInput.IndexOf("tan", 0, inputSize - 1) != -1)
            {
                startIndex = userInput.IndexOf("tan", 0, inputSize - 1) + 4;
                trig = "tan";
            }
            else if (userInput.IndexOf("ctg", 0, inputSize - 1) != -1)
            {
                startIndex = userInput.IndexOf("ctg", 0, inputSize - 1) + 4;
                trig = "ctg";
            }

            int length = userInput.IndexOf(")", 0, inputSize) - startIndex;
            string equation = userInput.Substring(startIndex, length);
            Parser ptrig = new Parser(equation);

            userInput = userInput.Remove(startIndex - 4, length + 5);
            double result = 0;
            switch (trig)
            {
                case "sin": result = Math.Sin(ptrig.getResult() * Math.PI / 180); break;
                case "cos": result = Math.Cos(ptrig.getResult() * Math.PI / 180); break;
                case "tan": result = Math.Tan(ptrig.getResult() * Math.PI / 180); break;
                case "ctg": result = Math.Atan2(1, ptrig.getResult() * Math.PI / 180); break;
                default: throw new FormatException("Wrong operation.");
            }
            if (result < 0.00000000001) result = 0;
            userInput = userInput.Insert(startIndex - 4, result.ToString());
        }

        private void calculateResult()
        {
            for (int i = 0; i < operations.Count(); i++)
            {
                switch (operations.ElementAt(i))
                {
                    case '+': numbers[i + 1] = CalculationEngine.AddNumbers(numbers.ElementAt(i), numbers.ElementAt(i + 1)); break;
                    case '-': numbers[i + 1] = CalculationEngine.SubNumbers(numbers.ElementAt(i), numbers.ElementAt(i + 1)); break;
                    case '*': numbers[i + 1] = CalculationEngine.MulNumbers(numbers.ElementAt(i), numbers.ElementAt(i + 1)); break;
                    case '/': numbers[i + 1] = CalculationEngine.DivNumbers(numbers.ElementAt(i), numbers.ElementAt(i + 1)); break;
                    case '^': numbers[i + 1] = CalculationEngine.PowNumbers(numbers.ElementAt(i), numbers.ElementAt(i + 1)); break;
                    default: throw new FormatException("Wrong operation.");
                }
            }
            this.result = numbers.ElementAt(numbers.Count() - 1);
            numbers.Clear();
            operations.Clear();
        }

        public void setUserInput(string userInput)
        {
            this.userInput = userInput;
        }

        public double getResult()
        {
            return this.result;
        }

        public double execute()
        {
            parseInput();
            CalculationEngine.FixOrder(ref numbers, ref operations);
            calculateResult();
            return result;
        }
    }
}
