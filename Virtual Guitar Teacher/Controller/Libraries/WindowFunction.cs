using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Numerics;
using System.Security;

namespace Virtual_Guitar_Teacher.Controller.Libraries
{
    class WindowFunction
    {
        /// <summary>
        /// The window is optimized to minimize the maximum (nearest) side lobe.
        /// </summary>
        /// <param name="inputWave">Raw data of the input wave.</param>
        /// <returns></returns>
        [SecurityCritical]
        public static Complex[] Hamming(byte[] inputWave)
        {
            Complex[] outputWave = new Complex[inputWave.Length];
            double omega = 2.0 * Math.PI / (inputWave.Length - 1);
            const float alpha = 0.54f;
            const float beta = 0.46f;

            // outputWave[i].Re = real number (raw wave data)
            // outputWave[i].Im = imaginary number (0 since it hasn't gone through FFT yet)
            for (int i = 1; i < outputWave.Length; i++)
                // Translated from c++ sample I found somewhere
                outputWave[i] = alpha - beta * (float)Math.Cos((2 * Math.PI * i) / (inputWave.Length - 1));
            //(alpha - beta * Math.Cos(omega * (i))); //* inputWave[i].Real;

            return outputWave;
        }
    }
}