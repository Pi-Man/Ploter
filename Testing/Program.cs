// See https://aka.ms/new-console-template for more information

using Plotter;
using Plotter.Parsing;
using System.Drawing;

//List<Definition> definitions = new List<Definition>(StandardDefinitions.definitions);

Renderer renderer = new Renderer(
    [
    "f(x) = real((phi^x - psi^x)/sqrt(5))",
    "g(x) = tan(x)",
    "h(x) = g(x)*cos(x)"
    ]);

int w = 236;
int h = 63;

Color[,] texture = renderer.Render(w, h, -12, 12, -10, 10);

for (int j = h - 1; j >= 0; j--)
{
    for (int i = 0; i < w; i++)
    {
        if (texture[i, j] == Color.Red)
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        else if (texture[i, j] == Color.Blue)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
        }
        else if (texture[i, j] == Color.Yellow)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
        }
        else if (texture[i, j] == Color.Black)
        {
            Console.ForegroundColor = ConsoleColor.Black;
        }
        else if (texture[i, j] == Color.Green)
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }
        else
        {
            Console.Write(" ");
            continue;
        }
        Console.Write("*");
    }
    Console.WriteLine();
}