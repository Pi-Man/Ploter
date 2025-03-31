using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Ploter.Parsing
{
    public class Parser
    {

        private readonly Dictionary<char, Func<Complex, Complex, Complex>> _func_map;

        private readonly string _text;
        private readonly int _k;
        public Parser(string text, int k)
        {
            _text = text;
            _k = k;
            _func_map = new Dictionary<char, Func<Complex, Complex, Complex>>()
            {
                ['+'] = (c1, c2) => c1 + c2,
                ['-'] = (c1, c2) => c1 - c2,
                ['*'] = (c1, c2) => c1 * c2,
                ['/'] = (c1, c2) => c1 / c2,
                ['%'] = (c1, c2) => new Complex(c1.Real % c2.Real, c1.Imaginary % c2.Imaginary),
                ['^'] = (c1, c2) => Patches.Complex.Pow(c1, c2, _k),
            };
        }

        public Definition Parse()
        {
            int index = 0;
            int index2 = 0;
            int type = -1;
            index = _text.LastIndexOf(">=");
            index2 = index + 2;
            type = 0;
            if (index == -1)
            {
                index = _text.LastIndexOf("<=");
                index2 = index + 2;
                type = 1;
            }
            if (index == -1)
            {
                index = _text.LastIndexOf(">");
                index2 = index + 1;
                type = 2;
            }
            if (index == -1)
            {
                index = _text.LastIndexOf("<");
                index2 = index + 1;
                type = 3;
            }
            if (index == -1)
            {
                index = _text.LastIndexOf("=");
                index2 = index + 1;
                type = 4;
            }
            bool ok = ParseFunctionDef(_text[..index], out string name, out string[] parameters);
            if (!ok)
            {
                throw new Exception("Could not parse function definition");
            }
            if (parameters.Length != 0)
            {
                return new FunctionDef(name, parameters, ParseExpression(_text[index2..]));
            }
            else
            {
                return new VariableDef(name, ParseExpression(_text[index2..]));
            }
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

        private Expression ParseExpression(string text)
        {
            text = text.Trim();

            if (string.IsNullOrEmpty(text))
            {
                throw new Exception("Expected Expression");
            }

            if (text[0] == '-')
            {
                return new Operator(new Value(-1), ParseExpression(text[1..]), _func_map['*']);
            }
            for (int i = text.Length - 1; i >= 0; i--)
            {
                char c = text[i];
                if (c == '+' || c == '-')
                {
                    return new Operator(ParseExpression(text[..i]), ParseExpression(text[(i + 1)..]), _func_map[c]);
                }
                else if (c == ')')
                {
                    while (c != '(')
                    {
                        i--;
                        c = text[i];
                    }
                }
            }
            for (int i = text.Length - 1; i >= 0; i--)
            {
                char c = text[i];
                if (c == '*' || c == '/' || c == '%')
                {
                    return new Operator(ParseExpression(text[..i]), ParseExpression(text[(i + 1)..]), _func_map[c]);
                }
                else if (c == ')')
                {
                    while (c != '(')
                    {
                        i--;
                        c = text[i];
                    }
                }
            }
            for (int i = text.Length - 1; i >= 0; i--)
            {
                char c = text[i];
                if (c == '^')
                {
                    return new Operator(ParseExpression(text[..i]), ParseExpression(text[(i + 1)..]), _func_map[c]);
                }
                else if (c == ')')
                {
                    while (c != '(')
                    {
                        i--;
                        c = text[i];
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
                return new Operator(ParseExpression(text[1..index]), ParseExpression(text[(index + 1)..]), _func_map['*']);
            }
            else if (char.IsLetter(text[0]))
            {
                text = ParseIdentifier(text, out string name).Trim();
                if (string.IsNullOrEmpty(text))
                {
                    return new Variable(name);
                }
                if (text[0] == '(')
                {
                    if (text[^1] == ')') {
                        return new FunctionCall(name, text[1..^1].Split(',').Select(ParseExpression).ToList());
                    }
                    else
                    {
                        throw new Exception($"Missing closing `)` in Function Call: {text}");
                    }
                }
                else
                {
                    throw new Exception("");
                }
            }
            else if (char.IsDigit(text[0]))
            {
                text = ParseLiteral(text, out Value value);
                if (string.IsNullOrEmpty(text.Trim()))
                {
                    return value;
                }
                else
                {
                    if (text[0] == '(')
                    {
                        return new Operator(value, ParseExpression(text), _func_map['*']);
                    }
                    throw new Exception($"Could not parse literal: {text}");
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

        private static string ParseLiteral(string text, out Value value)
        {
            StringBuilder valueBuilder = new StringBuilder();
            int i;
            bool flag = true;
            for (i = 0; i < text.Length && (char.IsDigit(text[i]) || (flag && text[i] == '.')); i++)
            {
                if (text[i] == '.') flag = false;
                valueBuilder.Append(text[i]);
            }
            value = new Value(double.Parse(valueBuilder.ToString()));
            return text[i..];
        }
    }
}
