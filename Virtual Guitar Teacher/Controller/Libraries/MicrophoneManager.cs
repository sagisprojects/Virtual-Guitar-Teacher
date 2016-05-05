using System;
using Environment = Android.OS.Environment;
using Android.Media;
using Android.Util;
using System.Numerics;
using System.Security;
using MathNet.Numerics.IntegralTransforms;
using FftGuitarTuner;

namespace Virtual_Guitar_Teacher.Controller.Libraries
{
    public delegate void FinishedSamplingEventHandler(object sender, FinishedSampalingEventArgs e);

    public class FinishedSampalingEventArgs : EventArgs
    {
        public Hz Frequency { get; set; }
        public double Volume { get; set; }
    }

    public sealed class MicrophoneManager : IDisposable
    {
        public event FinishedSamplingEventHandler FinishedSampling;
        
        private const ChannelIn RECORDER_CHANNELS = ChannelIn.Mono;
        private const Encoding RECORDER_AUDIO_ENCODING = Encoding.Pcm16bit;
        private const int RECORDER_SAMPLERATE = 44100;
        //private const byte RECORDER_BPP = 16;
        private int _bufferSizeInBytes;

        const double MinFreq = 16.35;
        const double MaxFreq = 880.0;

        private AudioRecord _recorder;

        /// <summary>
        /// Initializes all recording parameters.
        /// </summary>
        public void RecorderInit()
        {
            // Get the minimum buffer size required for the successful creation of an AudioRecord object.
            _bufferSizeInBytes = AudioRecord.GetMinBufferSize(RECORDER_SAMPLERATE, RECORDER_CHANNELS,
                    RECORDER_AUDIO_ENCODING);

            // Initialize Audio Recorder.
            _recorder = new AudioRecord(AudioSource.Mic, RECORDER_SAMPLERATE,
                    RECORDER_CHANNELS, RECORDER_AUDIO_ENCODING, _bufferSizeInBytes);

            _recorder.StartRecording();
        }

        public void Listen()
        {
            byte[] audioBuffer = new byte[_bufferSizeInBytes];
            int numberOfReadBytes = _recorder.Read(audioBuffer, 0, _bufferSizeInBytes);

            double[] x = new double[audioBuffer.Length];

            for (int i = 0; i < x.Length; i++)
            {
                x[i] = audioBuffer[i] / 32768.0;
            }

            double frequency = FrequencyUtils.FindFundamentalFrequency(x, RECORDER_SAMPLERATE, MinFreq, MaxFreq);

            //Fire event for passing back the recorded value.
            FinishedSampling(this, new FinishedSampalingEventArgs()
            {
                Frequency = new Hz((float)frequency * 2),
                //Volume = max_magnitude
            });
        }

        [SecurityCritical]
        public void Listen2()
        {
            int numberOfReadBytes = 0;
            byte[] audioBuffer = new byte[_bufferSizeInBytes];              // input PCM data buffer
            double[] hannWindow;
            Complex[] fftBuffer = new Complex[_bufferSizeInBytes];      // FFT complex buffer (interleaved real/imag)
            double[] magnitude = new double[_bufferSizeInBytes];        // power spectrum - Does this need to be half the size?
            float frequency;

            //for (int i = 0; i <= RECORDER_SAMPLERATE; i++)

            //Capture audio in audioBuffer.
            numberOfReadBytes = _recorder.Read(audioBuffer, 0, _bufferSizeInBytes);

            //Apply window function to audioBuffer.
            /*hannWindow = WindowFunction.Hann(audioBuffer);

            //Copy real and imaginary input data to complex fftBuffer.
            for (int i = 0; i < _bufferSizeInBytes; i += 2)
            {
                fftBuffer[i] = hannWindow[i];
                fftBuffer[i + 1] = hannWindow[i + 1];
            }*/

            //Perform in-place complex - to - complex FFT on fftBuffer.
            //CT_FFT.Transform(ref fftBuffer); //Does this use complex numbers correctly? (arr[x].Real arr[x].Imaginary)
            Fourier.Forward(fftBuffer);

            //Calculate power spectrum (magnitude) values from fftBuffer.
            /*for (int i = 0; i < fftBuffer.Length; i++)
            {
                double real = fftBuffer[i].Real;
                double imaginary = fftBuffer[i].Imaginary;
                magnitude[i] = Math.Sqrt(real * real + imaginary * imaginary);
            }*/

            //Find largest peak in power spectrum.
            double max_magnitude = double.NegativeInfinity;
            double max_index = -1;
            for (int i = 0; i < fftBuffer.Length; i++)
                if (fftBuffer[i].Magnitude > max_magnitude)
                {
                    max_magnitude = fftBuffer[i].Magnitude;
                    max_index = i;
                }
            
            //Convert index of largest peak to frequency.
            frequency = (float)max_index * RECORDER_SAMPLERATE / _bufferSizeInBytes;

            //Fire event for passing back the recorded value.
            FinishedSampling(this, new FinishedSampalingEventArgs()
            {
                Frequency = new Hz(frequency),
                Volume = max_magnitude
            });
        }
        
        public void Dispose()
        {
            _recorder.Stop();
            _recorder.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}