// See https://aka.ms/new-console-template for more information

using Plotter;
using Plotter.Parsing;
using System.Drawing;

//List<Definition> definitions = new List<Definition>(StandardDefinitions.definitions);

Renderer renderer = new Renderer(["f(x) = sin(x)"]);

int w = 50;
int h = 20;

Color[,] texture = renderer.Render(w, h, -6, 6, -1, 1);

for (int j = h - 1; j >= 0; j--)
{
    for (int i = 0; i < w; i++)
    {
        if (texture[i, j] == Color.Red)
        {
            Console.Write('*');
        }
        else
        {
            Console.Write(' ');
        }
    }
    Console.WriteLine();
}