using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Plotter.Parsing
{
    internal class RecursiveDefinition : Definition
    {
        private readonly List<IExpression> _initials;
        private readonly Dictionary<int, Complex> _cache = [];
        public RecursiveDefinition(string name, string parameter, IExpression expression, List<IExpression> initials, List<Definition> definitions) : base(name, [parameter], expression, definitions)
        {
            _initials = initials;
            Call[] calls = expression.GetCalls(name);
            int initials_count = calls.Select(call => call.GetRecursionOffset(parameter)).Max();
            if (initials.Count != initials_count) throw new ArgumentException($"{initials.Count} initial values were given, expected {initials_count}");
        }

        public override Complex Invoke(params Complex[] args)
        {
            if (args.Length == 1)
            {
                int recursion = (int) Math.Round(args[0].Real);
                if (recursion < 0) recursion = 0;
                if (_cache.TryGetValue(recursion, out Complex value))
                {
                    return value;
                }
                else
                {
                    Complex result = base.Invoke(args);
                    _cache.Add(recursion, result);
                    return result;
                }
            }
            return base.Invoke(args); // will throw because args is not length 1
        }

        public void ClearCache()
        {
            _cache.Clear();
            for (int i = 0; i < _initials.Count; i++)
            {
                _cache[i] = _initials[i].Resolve(_defintions);
            }
        }
    }
}
