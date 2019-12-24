using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Signature
{
    static class FastMath
    {
        public static ulong FastPow(ulong number, ulong power, ulong modul)
        {
            ulong x = 1;
            while (power != 0)
            {
                while (power % 2 == 0)
                {
                    power /= 2;
                    number = number * number % modul;
                }
                power--;
                x = (x * number) % modul;
            }
            return x;
        }

        public static ulong Gcd(ulong a, ulong b)
        {
            if (b == 0)
                return a;
            return Gcd(b, a % b);
        }

        public static bool Ferma(ulong x)
        {
            if (x == 2)
                return true;

            var rand = new Random();

            for (int i = 0; i < 100; i++)
            {
                ulong a = ((ulong)(rand.Next()) % (x - 2)) + 2;
                if (Gcd(a, x) != 1)
                    return false;
                if (FastPow(a, x - 1, x) != 1)
                    return false;
            }
            return true;
        }

    }
}
