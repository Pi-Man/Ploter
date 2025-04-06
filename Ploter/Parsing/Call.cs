using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Plotter.Parsing
{
    public class Call : IExpression
    {
        public string Name { get; set; }
        public List<IExpression> Arguments { get; set; }

        public Call(string name, List<IExpression> arguments)
        {
            Name = name;
            Arguments = arguments;
        }

        public Complex Resolve(List<Definition> definitions)
        {
            foreach (Definition definition in definitions)
            {
                if (definition.Name == Name)
                {
                    return definition.Invoke(Arguments.Select(expr => expr.Resolve(definitions)).ToArray());
                }
            }
            throw new Exception($"{(Arguments.Count == 0 ? "Variable" : "Function")} {Name} not found in the current context");
        }

        public bool DependsOn(string name)
        {
            return name.Equals(Name) || Arguments.Any(exp => exp.DependsOn(name));
        }

        public Call[] GetCalls(string name)
        {
            return name.Equals(Name) ? [this] : Arguments.SelectMany(expr => expr.GetCalls(name)).ToArray();
        }

        public int GetRecursionOffset(string argName)
        {
            if (Arguments is
            [
                Operator
            {
                Left: Call
                {
                    Name: var callName,
                    Arguments: [],
                },
                Right: Literal
                {
                    Val: Complex
                    {
                        Real: var val,
                        Imaginary: 0
                    }
                },
                Op: var op
            }
            ]
            && callName.Equals(argName)
            && val == Math.Floor(val)
            && op == Operator.SUB)
                return (int)val;
            else throw new Exception("Recursive calls do not match the form 'name(param - integerLiteral)'");
        }
    }
}
