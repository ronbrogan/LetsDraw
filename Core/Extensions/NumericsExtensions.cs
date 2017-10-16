using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Core.Extensions
{
    public static class NumericsExtensions
    {
        public static Vector3 Xyz(this Vector3 input)
        {
            return new Vector3(input.X, input.Y, input.Z);
        }


        public static Vector3 Xyz(this Vector4 input)
        {
            return new Vector3(input.X, input.Y, input.Z);
        }
    }
}
