using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Plotter.Parsing
{
    internal class Call : IExpression
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
            return Arguments.Any(exp => exp.DependsOn(name));
        }
    }
}
