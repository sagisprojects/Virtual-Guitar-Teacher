using System;
using System.Numerics;
using System.Security;

namespace Virtual_Guitar_Teacher.Controller.Libraries
{
    /// <summary>
    /// The purpose of this task is to calculate the FFT (Fast Fourier Transform) of an input sequence. 
    /// The most general case allows for complex numbers at the input and results in a sequence of equal length, 
    /// again of complex numbers.If you need to restrict yourself to real numbers, 
    /// the output should be the magnitude (i.e.sqrt(re²+im²)) of the complex result.
    /// The classic version is the recursive Cooley–Tukey FFT. Wikipedia has pseudocode for that.
    /// Further optimizations are possible but not required.
    /// </summary>
    class CT_FFT
    {
        /// <summary>
        /// Performs a Bit Reversal Algorithm on a postive integer for given number of bits.
        /// e.g. 011 with 3 bits is reversed to 110
        /// </summary>
        /// <param name="n"></param>
        /// <param name="bits"></param>
        /// <returns></returns>
        public static int BitReverse(int n, int bits)
        {
            int reversedN = n;
            int count = bits - 1;

            n >>= 1;
            while (n > 0)
            {
                reversedN = (reversedN << 1) | (n & 1);
                count--;
                n >>= 1;
            }

            return ((reversedN << count) & ((1 << bits) - 1));
        }

        /// <summary>
        /// Uses Cooley-Tukey iterative in-place algorithm with radix-2 DIT case
        /// assumes number of points provided are a power of 2.
        /// </summary>
        /// <param name="buffer"></param>
        [SecurityCritical]
        public static void FFT(Complex[] buffer)
        {
            int bits = (int)Math.Log(buffer.Length, 2);
            for (int j = 1; j < buffer.Length / 2; j++)
            {
                int swapPos = BitReverse(j, bits);
                var temp = buffer[j];
                buffer[j] = buffer[swapPos];
                buffer[swapPos] = temp;
            }

            for (int N = 2; N <= buffer.Length; N <<= 1)
            {
                for (int i = 0; i < buffer.Length; i += N)
                {
                    for (int k = 0; k < N / 2; k++)
                    {
                        int evenIndex = i + k;
                        int oddIndex = i + k + (N / 2);
                        var even = buffer[evenIndex];
                        var odd = buffer[oddIndex];

                        double term = -2 * Math.PI * k / (double)N;
                        Complex exp = new Complex(Math.Cos(term), Math.Sin(term)) * odd;

                        buffer[evenIndex] = even + exp;
                        buffer[oddIndex] = even - exp;
                    }
                }
            }
        }
    }
}