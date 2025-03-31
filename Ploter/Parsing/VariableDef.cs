using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploter.Parsing
{
    public class VariableDef : Definition
    {
        public string Name { get; set; }
        public Expression Expression { get; set; }

        public VariableDef(string name, Expression expression)
        {
            Name = name;
            Expression = expression;
        }
    }
}
