# Ploter

Library for rendering various functions to a texture.

Supports Complex numbers, variables, multivariable functions, inequalities (WIP), recursive functions, derivatives (WIP), simple integrals (WIP)

Simple usecase is to create a `Renderer` with a list of equations, and then call `Renderer.Render(textureWidth, textureHeight, xmin, xmax, ymin, ymax)`

Example:
```cs
Renderer renderer = new Renderer([
	"f(x) = x^2 + 3x - 2",
	"g(x) = x + 7",
	"h(x) = f(x)/g(x)",
]);

Color[,] texture = renderer.Render(200,200, -10, 10, -10, 10);
```