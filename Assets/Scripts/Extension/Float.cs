using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security;
using System.Text;

using IvoryCrow.Utilities;

namespace IvoryCrow.Extensions
{
    public static class FloatExtensions
    {
        private static float Epsilon = 0.001f;

        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static float EnsureNonNegative(this float value)
        {
            return (value < 0) ? 0 : value;
        }

        public static float Clamp(this float value, float min, float max)
        {
            if (value < min)
            {
                return min;
            }

            if (value > max)
            {
                return max;
            }

            return value;
        }

        public static bool IsZero(this float value)
        {
            return Math.Abs(value) < Epsilon;
        }

        public static float Lerp(float start, float end, float amount)
        {
            return start + (end - start) * amount;
        }
    }
}