using System;

namespace Foundation
{
    public static class MathExtensions
    {
        public static float Min(float w, float x, float y, float z)
        {
            return Math.Min(Math.Min(Math.Min(w, x), y), z);
        }

        public static float Max(float w, float x, float y, float z)
        {
            return Math.Max(Math.Max(Math.Max(w, x), y), z);
        }
    }
}
