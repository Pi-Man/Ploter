using MathNet.Numerics;
using Plotter.Parsing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Plotter
{
    public class Renderer
    {
        private readonly List<Definition> _context;

        private readonly List<Entry> _entries;

        private static readonly int SIZE = 2;

        public Renderer(List<Entry> entries, List<Definition>? context = null)
        {
            _context = context ?? new List<Definition>(StandardDefinitions.definitions);
            _entries = entries;
        }
        public Renderer(string[] entries, List<Definition>? context = null)
        {
            _context = context ?? new List<Definition>(StandardDefinitions.definitions);
            _entries = entries.Select(s => new Entry(s, _context)).ToList();
        }

        public Renderer(List<Definition>? context = null) : this(new List<Entry>())
        {
            _context = context ?? new List<Definition>(StandardDefinitions.definitions);
        }

        public Renderer AddEntry(Entry entry)
        {
            _entries.Add(entry);
            return this;
        }

        public Renderer AddEntry(string entry)
        {
            _entries.Add(new Entry(entry, _context));
            return this;
        }

        public Color[,] Render(int w, int h, double xmin, double xmax, double ymin, double ymax)
        {
            Color[,] texture = new Color[w, h];
            foreach (var entry in _entries)
            {
                if (!entry.Parsed()) 
                    entry.Parse();
                if (entry.CanRender())
                {
                    for (int i = 0; i < w; i++)
                    {
                        double x = (double)i / (double)w * (xmax - xmin) + xmin;
                        Complex val = entry.Invoke(x);
                        if (Math.Abs(val.Real / val.Imaginary) > 1_000_000_000)
                        {
                            double y = val.Real;
                            int j = (int) ((y - ymin)/(ymax - ymin) * h).Round(0);

                            for (int j2 = j - SIZE; j2 < j + SIZE; j2++)
                            {
                                for (int i2 = i - SIZE; i2 < i + SIZE; i2++)
                                {
                                    if (i2 < w && j2 < h && i2 > 0 && j2 > 0)
                                    {
                                        texture[i2, j2] = Color.Red;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return texture;
        }

    }
}
