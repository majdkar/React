using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SchoolV01.Application.Extensions
{
    public static partial class EvalExtensions
    {
        [GeneratedRegex(@"(?<={)([^}]+)(?=})")]
        private static partial Regex MyRegex();


        private static readonly string[] _operators = ["-", "+", "/", "*", "^"];
        private static readonly Func<double, double, double>[] _operations = [
            (a1, a2) => a1 - a2,
            (a1, a2) => a1 + a2,
            (a1, a2) => a1 / a2,
            (a1, a2) => a1 * a2,
            Math.Pow
            ];

        public static double Eval(string expression)
        {
            List<string> tokens = GetTokens(expression);
            Stack<double> operandStack = new();
            Stack<string> operatorStack = new();
            int tokenIndex = 0;

            while (tokenIndex < tokens.Count)
            {
                string token = tokens[tokenIndex];
                if (token == "(")
                {
                    string subExpr = GetSubExpression(tokens, ref tokenIndex);
                    operandStack.Push(Eval(subExpr));
                    continue;
                }
                if (token == ")")
                {
                    throw new ArgumentException("Mis-matched parentheses in expression");
                }
                //If this is an operator  
                if (Array.IndexOf(_operators, token) >= 0)
                {
                    while (operatorStack.Count > 0 && Array.IndexOf(_operators, token) < Array.IndexOf(_operators, operatorStack.Peek()))
                    {
                        string op = operatorStack.Pop();
                        double arg2 = operandStack.Pop();
                        double arg1 = operandStack.Pop();
                        operandStack.Push(_operations[Array.IndexOf(_operators, op)](arg1, arg2));
                    }
                    operatorStack.Push(token);
                }
                else
                {
                    operandStack.Push(double.Parse(token));
                }
                tokenIndex += 1;
            }

            while (operatorStack.Count > 0)
            {
                string op = operatorStack.Pop();
                double arg2 = operandStack.Pop();
                double arg1 = operandStack.Pop();
                operandStack.Push(_operations[Array.IndexOf(_operators, op)](arg1, arg2));
            }
            return operandStack.Pop();
        }

        private static string GetSubExpression(List<string> tokens, ref int index)
        {
            StringBuilder subExpr = new();
            int parenLevels = 1;
            index += 1;
            while (index < tokens.Count && parenLevels > 0)
            {
                string token = tokens[index];
                if (tokens[index] == "(")
                {
                    parenLevels += 1;
                }

                if (tokens[index] == ")")
                {
                    parenLevels -= 1;
                }

                if (parenLevels > 0)
                {
                    subExpr.Append(token);
                }

                index += 1;
            }

            if ((parenLevels > 0))
            {
                throw new ArgumentException("Mis-matched parentheses in expression");
            }
            return subExpr.ToString();
        }

        private static List<string> GetTokens(string expression)
        {
            string operators = "()^*/+-";
            List<string> tokens = [];
            StringBuilder sb = new();

            foreach (char c in expression.Replace(" ", string.Empty))
            {
                if (operators.Contains(c))
                {
                    if ((sb.Length > 0))
                    {
                        tokens.Add(sb.ToString());
                        sb.Length = 0;
                    }
                    tokens.Add(c.ToString());
                }
                else
                {
                    sb.Append(c);
                }
            }

            if ((sb.Length > 0))
            {
                tokens.Add(sb.ToString());
            }
            return tokens;
        }

      
    }
}