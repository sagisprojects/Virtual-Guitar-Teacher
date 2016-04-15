using System;
using Environment = Android.OS.Environment;
using Android.Media;
using Android.Util;
using System.Numerics;
using System.Security;
using Java.Nio;

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

        [SecurityCritical]
        public void Listen()
        {
            int numberOfReadBytes = 0;
            byte[] audioBuffer = new byte[_bufferSizeInBytes];              // input PCM data buffer
            Complex[] fftBuffer = new Complex[_bufferSizeInBytes];      // FFT complex buffer (interleaved real/imag)
            double[] magnitude = new double[_bufferSizeInBytes];        // power spectrum - Does this need to be half the size?
            float frequency;

            //for (int i = 0; i <= RECORDER_SAMPLERATE; i++)

            //Capture audio in audioBuffer.
            numberOfReadBytes = _recorder.Read(audioBuffer, 0, _bufferSizeInBytes);

            //Apply window function to audioBuffer.
            fftBuffer = WindowFunction.Hann(audioBuffer);

            //Copy real input data to complex fftBuffer.
            /*for (int i = 0; i < _bufferSizeInBytes - 1; i += 2)
            {
                fftBuffer[2 * i] = hannWindow[i];
                fftBuffer[2 * i + 1] = 0;
            }*/

            //Perform in-place complex - to - complex FFT on fftBuffer.
            CT_FFT.Transform(ref fftBuffer); //Does this use complex numbers correctly? (arr[x].Real arr[x].Imaginary)

            //Calculate power spectrum (magnitude) values from fftBuffer.
            for (int i = 0; i < fftBuffer.Length; i++)
            {
                double real = fftBuffer[i].Real;
                double imaginary = fftBuffer[i].Imaginary;
                magnitude[i] = Math.Sqrt(real * real + imaginary * imaginary);
            }

            //Find largest peak in power spectrum.
            double max_magnitude = double.NegativeInfinity;
            double max_index = -1;
            for (int i = 0; i < magnitude.Length; i++)
                if (magnitude[i] > max_magnitude)
                {
                    max_magnitude = magnitude[i];
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