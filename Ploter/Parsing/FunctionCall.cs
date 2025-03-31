using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Ploter.Parsing
{
    internal class FunctionCall : Expression
    {
        public string Name { get; set; }
        public List<Expression> Arguments { get; set; }

        public FunctionCall(string name, List<Expression> arguments)
        {
            Name = name;
            Arguments = arguments;
        }

        public override Complex Resolve(List<Variable> variables, List<FunctionDef> functions)
        {
            foreach (FunctionDef function in functions)
            {
                if (function.Name == Name)
                {
                    return function.Invoke(Arguments.Select(expr => expr.Resolve(variables, functions)).ToArray());
                }
            }
            throw new Exception($"Function {Name} not found in the current context");
        }

        public override bool DependsOn(string name)
        {
            return Arguments.Any(exp => exp.DependsOn(name));
        }
    }
}
