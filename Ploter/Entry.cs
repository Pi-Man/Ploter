using Plotter.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Plotter
{
    public class Entry
    {
        private readonly string _text;
        private string _error;
        private Definition? _definition = null;
        private readonly List<Definition> _context;

        public Entry(string text, List<Definition> context)
        {
            _text = text;
            _error = "";
            _context = context;
        }

        public bool Parse()
        {
            Parser parser = new Parser(_text, _context);
            try
            {
                _definition = parser.Parse();
                return true;
            }
            catch (Exception e)
            {
                _error = e.Message;
                return false;
            }
        }
        
        public bool CanRender()
        {
            return _definition is Definition func && func.GetParamCount() == 1;
        }

        public bool HasError()
        {
            return !string.IsNullOrEmpty(_error);
        }

        public string GetError()
        {
            return _error;
        }

        public bool Parsed()
        {
            return !string.IsNullOrEmpty(_error) || _definition is not null;
        }

        public Complex Invoke(params Complex[] args)
        {
            if (_definition is not null)
            {
                return _definition.Invoke(args);
            }
            return Complex.NaN;
        }
    }
}
