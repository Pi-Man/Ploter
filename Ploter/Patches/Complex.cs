namespace Plotter.Patches
{
    public static class Complex
    {
        public static System.Numerics.Complex Exp(System.Numerics.Complex value, int k)
        {
            double expReal = Math.Exp(value.Real);
            double cosImaginary = expReal * Math.Cos(value.Imaginary);
            double sinImaginary = expReal * Math.Sin(value.Imaginary);
            return new System.Numerics.Complex(cosImaginary, sinImaginary);
        }

        public static System.Numerics.Complex Sqrt(System.Numerics.Complex value, int k)
        {
            return System.Numerics.Complex.Sqrt(value) * ((k & 1) * -2 + 1);
        }

        public static System.Numerics.Complex Pow(System.Numerics.Complex value, System.Numerics.Complex power, int k)
        {
            if (power == System.Numerics.Complex.Zero)
            {
                return System.Numerics.Complex.One;
            }

            if (value == System.Numerics.Complex.Zero)
            {
                return System.Numerics.Complex.Zero;
            }

            double valueReal = value.Real;
            double valueImaginary = value.Imaginary;
            double powerReal = power.Real;
            double powerImaginary = power.Imaginary;

            double rho = System.Numerics.Complex.Abs(value);
            double theta = Math.Atan2(valueImaginary, valueReal) + 2 * Math.PI * k;
            double newRho = powerReal * theta + powerImaginary * Math.Log(rho);

            double t = Math.Pow(rho, powerReal) * Math.Pow(Math.E, -powerImaginary * theta);

            return new System.Numerics.Complex(t * Math.Cos(newRho), t * Math.Sin(newRho));
        }

        public static System.Numerics.Complex Pow(System.Numerics.Complex value, double power, int k)
        {
            return Pow(value, new System.Numerics.Complex(power, 0), k);
        }
    }
}