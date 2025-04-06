// See https://aka.ms/new-console-template for more information

using Plotter;
using Plotter.Parsing;
using System.Drawing;
using System.Text;

List<Definition> definitions = new List<Definition>(StandardDefinitions.definitions);

ReferenceDefinition<double> tick = new("tick", 0);

definitions.Add(tick);

Renderer renderer = new Renderer(
    [
    //"f(x) = real((phi^x - psi^x)/sqrt(5))",
    //"g(x) = tan(x)",
    //"h(x) = g(x)*cos(x)",
    //"f(x) = ((x/12)^2 - 0.5)20",
    //"g(x) = g(x-1) + 1; 1",
    //"fib(n) = fib(n-1) + fib(n-2); 0, 1",
    //"r(n) = (tick+3)*r(n-1)*(1-r(n-1)); 0.34771",
    "da(a, b) = b - a",
    "db(a, b, c) = (a + c - 2b)/2",
    "f(x) = 2f(x-1) - f(x-2) - 0.01f(x-1) - 0.3da(f(x-2), f(x-1)); -2, tick - 3",
    "g(x) = 3exp(-0.037x)"
    //"f(x) = sin(x + tick)",
    //"g(x) = sin(0.5x - tick)",
    //"h(x) = f(x) + g(x)",
    ], definitions);

// a = f(x)
// v = v(x-1) + a
// p = p(x-1) + v
// p = p(x-1) + v(x-1) + a
// p = p(x-1) + p(x-1) - p(x-2) + a

int w = 237;
int h = 125;

while (true)
{
    Color[,] texture = renderer.Render(w, h, 0, 237, -5, 5);

    ConsoleRender(w, h, texture);

    //Thread.Sleep(100);
    tick.Value += 0.001;
    //break;
}

static void ConsoleRender(int w, int h, Color[,] texture)
{
    Color last = Color.Black;
    StringBuilder sb = new StringBuilder();
    sb.Append($"\u001B[48;2;0;0;0m");
    Console.OutputEncoding = Encoding.Default;
    Console.SetCursorPosition(0, 0);
    for (int j = h - 1; j >= 0; j--)
    {
        for (int i = 0; i < w; i++)
        {
            if (texture[i, j] != last)
            {
                last = texture[i, j];
                sb.Append($"\u001B[48;2;{last.R};{last.G};{last.B}m");
            }
            sb.Append(' ');
        }
        sb.Append('\n');
    }
    Console.WriteLine(sb.ToString());
}