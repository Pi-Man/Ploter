// See https://aka.ms/new-console-template for more information


using Ploter.Parsing;

Parser p = new Parser("cbrt(x) = 3()", 1);

FunctionDef f = p.Parse();

Console.WriteLine(f.Invoke(-1));
