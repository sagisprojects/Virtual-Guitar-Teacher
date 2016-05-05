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
        public static double[] Hann(byte[] inputWave)
        {
            double[] outputWave = new double[inputWave.Length];

            for (int i = 0; i < outputWave.Length; i++)
            {
                double multiplier = 0.5 * (1 - Math.Cos(2 * Math.PI * i / outputWave.Length - 1));
                outputWave[i] = multiplier * inputWave[i];
            }

            return outputWave;
        }
    }
}