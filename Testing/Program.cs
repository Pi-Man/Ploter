// See https://aka.ms/new-console-template for more information


using Ploter.Parsing;

Parser p = new Parser("cbrt(x) = x^(1/3)", 1);

FunctionDef f = (FunctionDef) p.Parse();

Console.WriteLine(f.Invoke(-1));
