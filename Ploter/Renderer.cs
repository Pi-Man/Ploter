using MathNet.Numerics;
using Plotter.Parsing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        private double _len;

        private static readonly int SIZE = 0;

        private static readonly double LENGTH = 0;

        private static readonly Color[] colors = [Color.Red, Color.Blue, Color.Yellow, Color.Black, Color.Green];

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
            int c = 0;
            foreach (var entry in _entries)
            {
                _len = 0;
                if (!entry.Parsed()) 
                    entry.Parse();
                if (entry.HasError())
                {
                    Console.Error.WriteLine(entry.GetError());
                }
                if (entry.CanRender())
                {
                    entry.Reset();
                    int pj = int.MaxValue;
                    for (int i = 0; i < w; i++)
                    {
                        double x = (double)i / (double)w * (xmax - xmin) + xmin;
                        Complex val = entry.Invoke(x);
                        if (val.IsReal() || Math.Abs(val.Real / val.Imaginary) > 1_000_000_000)
                        {
                            double y = val.Real;

                            int j = (int)Math.Floor((y - ymin) / (ymax - ymin) * h);
                            if (pj == int.MaxValue) pj = j;

                            if (pj >= 0 && pj < h || j >= 0 && j < h)
                            {
                                int s = Math.Sign(j - pj);
                                double d = Math.Sqrt(2);
                                for (int j2 = pj + s; j2 != j; j2 += s)
                                {
                                    Fill(texture, i, j2, w, h, colors[c], d);
                                    d = 1;
                                }
                            }

                            Fill(texture, i, j, w, h, colors[c], Math.Abs(pj - j) == 1 ? Math.Sqrt(2) : 1);

                            pj = j;
                        }
                        else
                        {
                            pj = int.MaxValue;
                        }
                    }
                    c++;
                }
            }
            return texture;
        }

        private void Fill(Color[,] texture, int x, int y, int w, int h, Color color, double d)
        {
            if (LENGTH > 0) _len = (_len + d) % (LENGTH * 2);
            for (int j = y - SIZE; j <= y + SIZE; j++)
            {
                for (int i = x - SIZE; i <= x + SIZE; i++)
                {
                    if (i < w && j < h && i >= 0 && j >= 0)
                    {
                        if (LENGTH <= 0 || _len < LENGTH)
                        {
                            texture[i, j] = color;
                        }
                    }
                }
            }
        }

    }
}
