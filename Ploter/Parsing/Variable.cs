using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Ploter.Parsing
{
    public class Variable : Expression
    {
        public string Name { get; set; }
        public Complex? Value { get; set; }

        public Variable(string name, Complex? value = null)
        {
            Name = name;
            Value = value;
        }

        public override Complex Resolve(List<Variable> variables, List<FunctionDef> functions)
        {
            foreach (var variable in variables)
            {
                if (variable.Name == Name)
                {
                    return variable.Value.Value;
                }
            }
            throw new Exception($"Variable {Name} not found in current context");
        }

        public override bool DependsOn(string name)
        {
            return name.Equals(Name);
        }
    }
}
