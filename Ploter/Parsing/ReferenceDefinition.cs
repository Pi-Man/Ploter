using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Plotter.Parsing
{
    public class ReferenceDefinition<T> : Definition where T : INumberBase<T>
    {
        
        public T Value { get; set; }

        public ReferenceDefinition(string name, T initialVal) : base(name, [], new Literal(0), [])
        {
            Value = initialVal;
        }

        public override int GetParamCount()
        {
            return 0;
        }

        public override Complex Invoke(params Complex[] args)
        {
            return Complex.CreateSaturating(Value);
        }
    }
}
