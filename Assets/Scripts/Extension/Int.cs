using System;

namespace IvoryCrow.Extensions
{
    public static class IntExtensions
    {
        public static int Remap(this int value, int from1, int to1, int from2, int to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static int Wrap(this int value, int min, int max)
        {
            if (value < min)
            {
                return max;
            }

            if (value > max)
            {
                return min;
            }

            return value;
        }

        public static int Clamp(this int value, int min, int max)
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

        public static int Lerp(int start, int end, float amount)
        {
            float lerpCastedValues = (float)start + ((float)end - (float)start) * amount;
            return (int)lerpCastedValues;
        }

        public static int MoveTowardsZero(int val, int magnitude)
        {
            if (val < 0)
            {
                val = val + Math.Abs(magnitude);
                if (val > 0)
                {
                    val = 0;
                }
            }
            else if (val > 0)
            {
                val = val - Math.Abs(magnitude);
                if (val < 0)
                {
                    val = 0;
                }
            }

            return val;
        }

        public static int DivRoundUp(int val, int div)
        {
            return val % div == 0 ? (val / div) : (val / div) + 1;
        }

        /// <summary>
        /// From http://www.codecodex.com/wiki/Calculate_an_integer_square_root
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int ISqrt(int num)
        {
            if (0 == num) { return 0; }  // Avoid zero divide  
            int n = (num / 2) + 1;       // Initial estimate, never low  
            int n1 = (n + (num / n)) / 2;
            while (n1 < n)
            {
                n = n1;
                n1 = (n + (num / n)) / 2;
            } // end while  
            return n;
        }

        public static int LSqrt(long num)
        {
            if (0 == num) { return 0; }  // Avoid zero divide  
            long n = (num / 2) + 1;       // Initial estimate, never low  
            long n1 = (n + (num / n)) / 2;
            while (n1 < n)
            {
                n = n1;
                n1 = (n + (num / n)) / 2;
            } // end while  

            return (int) n; // TODO: Should check this cast
        }
    }
}