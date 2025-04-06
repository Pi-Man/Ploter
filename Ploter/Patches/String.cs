using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plotter.Patches
{
    internal static class String
    {
        public static bool TrySplit(this string input, string split, out string left, out string right)
        {
            string[] arr = input.Split(split, 2);
            if (arr.Length < 2)
            {
                left = "";
                right = "";
                return false;
            }
            else
            {
                left = arr[0];
                right = arr[1];
                return true;
            }
        }
    }
}
