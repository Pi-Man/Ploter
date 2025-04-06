using Plotter.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Plotter.Parsing
{
    public class Parser
    {

        public enum EquationType
        {
            GTE,
            LTE,
            GT,
            LT,
            EQ,
            ERR,
        }

        private readonly string _text;
        private readonly List<Definition> _context;

        public Parser(string text, List<Definition> context)
        {
            _text = text;
            _context = context;
        }

        public Definition Parse()
        {
            int index = 0;
            int index2 = 0;
            EquationType type = EquationType.ERR;
            string left;
            string right;
            string name;
            string[] parameters;
            IExpression expr;
            if (_text.TrySplit(">=", out left, out right))
            {
                type = EquationType.GTE;
            }
            else if (_text.TrySplit("<=", out left, out right))
            {
                type = EquationType.LTE;
            }
            else if (_text.TrySplit(">", out left, out right))
            {
                type = EquationType.GT;
            }
            else if (_text.TrySplit("<", out left, out right))
            {
                type = EquationType.LT;
            }
            if (_text.TrySplit("=", out left, out right))
            {
                type = EquationType.EQ;
                if (!ParseFunctionDef(left, out name, out parameters))
                {
                    throw new Exception("Could not parse function definition");
                }
                if (right.TrySplit(";", out string exprStr, out string initialsString))
                {
                    expr = ParseExpression(exprStr);
                    if (expr.DependsOn(name))
                    {
                        if (parameters.Length != 1)
                        {
                            throw new Exception("Recursive function must be a single variable function");
                        }
                        else
                        {
                            List<IExpression> initials = initialsString.Split(',', StringSplitOptions.TrimEntries).Select(ParseExpression).ToList();
                            return new RecursiveDefinition(name, parameters[0], expr, initials, _context);
                        }
                    }
                }
            }
            if (!ParseFunctionDef(left, out name, out parameters))
            {
                throw new Exception("Could not parse function definition");
            }
            expr = ParseExpression(right);
            if (expr.DependsOn(name))
            {
                throw new Exception("Inequalities can not be Recursive");
            }
            return new Definition(name, parameters, expr, _context);
        }

        private static bool ParseFunctionDef(string text, out string name, out string[] parameters)
        {
            text = text.Trim();
            string[] parts = text.Split('(', 2);
            name = parts[0].Trim();
            if (!name.All(char.IsLetter))
            {
                name = "";
                parameters = [];
                return false;
            }
            if (parts.Length == 2)
            {
                if (parts[1][^1] != ')')
                {
                    parameters = [];
                    return false;
                }
                parameters = parts[1][..^1].Split(',').Select(s => s.Trim()).ToArray();
                if (!parameters.All(s => s.All(char.IsLetter)))
                {
                    return false;
                }
            }
            else
            {
                parameters = [];
            }
            return true;
        }

        private IExpression ParseExpression(string text)
        {
            text = text.Trim();

            if (string.IsNullOrEmpty(text))
            {
                throw new Exception("Expected Expression");
            }

            if (text[0] == '-')
            {
                return new Operator(new Literal(-1), ParseExpression(text[1..]), Operator.MUL);
            }
            for (int i = text.Length - 1; i >= 0; i--)
            {
                char c = text[i];
                if (c == '+' || c == '-')
                {
                    return new Operator(ParseExpression(text[..i]), ParseExpression(text[(i + 1)..]), Operator.Map[c]);
                }
                else if (c == ')')
                {
                    int j = 1;
                    while (j != 0)
                    {
                        i--;
                        c = text[i];
                        if (c == ')') j++;
                        if (c == '(') j--;
                    }
                }
            }
            for (int i = text.Length - 1; i >= 0; i--)
            {
                char c = text[i];
                if (c == '*' || c == '/' || c == '%')
                {
                    return new Operator(ParseExpression(text[..i]), ParseExpression(text[(i + 1)..]), Operator.Map[c]);
                }
                else if (c == ')')
                {
                    int j = 1;
                    while (j != 0)
                    {
                        i--;
                        c = text[i];
                        if (c == ')') j++;
                        if (c == '(') j--;
                    }
                }
            }
            for (int i = text.Length - 1; i >= 0; i--)
            {
                char c = text[i];
                if (c == '^')
                {
                    return new Operator(ParseExpression(text[..i]), ParseExpression(text[(i + 1)..]), Operator.Map[c]);
                }
                else if (c == ')')
                {
                    int j = 1;
                    while (j != 0)
                    {
                        i--;
                        c = text[i];
                        if (c == ')') j++;
                        if (c == '(') j--;
                    }
                }
            }

            if (text[0] == '(')
            {
                if (text[^1] == ')')
                {
                    return ParseExpression(text[1..^1]);
                }
                int index = text.LastIndexOf(')');
                return new Operator(ParseExpression(text[1..index]), ParseExpression(text[(index + 1)..]), Operator.MUL);
            }
            else if (char.IsLetter(text[0]))
            {
                text = ParseIdentifier(text, out string name).Trim();
                if (string.IsNullOrEmpty(text))
                {
                    return new Call(name, []);
                }
                if (text[0] == '(')
                {
                    if (text[^1] == ')') {
                        return new Call(name, text[1..^1].Split(',').Select(ParseExpression).ToList());
                    }
                    else
                    {
                        throw new Exception($"Missing closing `)` in Function Call: {text}");
                    }
                }
                else
                {
                    return new Operator(new Call(name, []), ParseExpression(text), Operator.MUL);
                }
            }
            else if (char.IsDigit(text[0]))
            {
                text = ParseLiteral(text, out Literal value);
                if (string.IsNullOrEmpty(text.Trim()))
                {
                    return value;
                }
                else
                {
                    return new Operator(value, ParseExpression(text), Operator.MUL);
                }
            }
            else
            {
                throw new Exception($"Could not parse Text: {text}");
            }
        }

        private static string ParseIdentifier(string text, out string name)
        {
            StringBuilder nameBuilder = new StringBuilder();
            int i;
            for (i = 0; i < text.Length && char.IsLetter(text[i]); i++)
            {
                nameBuilder.Append(text[i]);
            }
            name = nameBuilder.ToString();
            return text[i..];
        }

        private static string ParseLiteral(string text, out Literal value)
        {
            StringBuilder valueBuilder = new StringBuilder();
            int i;
            bool flag = true;
            for (i = 0; i < text.Length && (char.IsDigit(text[i]) || (flag && text[i] == '.')); i++)
            {
                if (text[i] == '.') flag = false;
                valueBuilder.Append(text[i]);
            }
            value = new Literal(double.Parse(valueBuilder.ToString()));
            return text[i..];
        }
    }
}
