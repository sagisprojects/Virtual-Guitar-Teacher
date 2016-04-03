using System;
using Environment = Android.OS.Environment;
using Android.Media;
using Android.Util;
using System.Numerics;
using System.Security;

namespace Virtual_Guitar_Teacher.Controller.Libraries
{
    public delegate void FinishedSamplingEventHandler(object sender, FinishedSampalingEventArgs e);

    public class FinishedSampalingEventArgs : EventArgs
    {
        public Hz Frequency { get; set; }
    }

    public sealed class MicrophoneManager : IDisposable
    {
        public event FinishedSamplingEventHandler FinishedSampling;

        private string _defaultRecordingFilePath = @"\VGT\Documents\Recordings\"; //This should be in strings.xml, not here.

        private const ChannelIn RECORDER_CHANNELS = ChannelIn.Mono;
        private const Encoding RECORDER_AUDIO_ENCODING = Encoding.Pcm16bit;
        private const int RECORDER_SAMPLERATE = 44100;
        private const byte RECORDER_BPP = 16;
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
        }

        [SecurityCritical]
        public void Listen()
        {
            int numberOfReadBytes = 0;
            byte[] audioBuffer = new byte[_bufferSizeInBytes];              // input PCM data buffer
            Complex[] fftBuffer = new Complex[_bufferSizeInBytes * 2];      // FFT complex buffer (interleaved real/imag)
            double[] magnitude = new double[_bufferSizeInBytes / 2];            // power spectrum
            float frequency;

            for (int i = 0; i <= RECORDER_SAMPLERATE; i++)
            {
                //capture audio in data[] buffer.
                numberOfReadBytes = _recorder.Read(audioBuffer, i, _bufferSizeInBytes);
            }

            //apply window function to audioBuffer.
            Complex[] hamWindow = WindowFunction.Hamming(audioBuffer); //TODO: Need to apply to audioBuffer.

            //copy real input data to complex FFT buffer
            for (int i = 0; i < _bufferSizeInBytes - 1; i += 2)
            {
                fftBuffer[2 * i] = audioBuffer[i];
                fftBuffer[2 * i + 1] = 0;
            }

            //perform in-place complex - to - complex FFT on fft[] buffer
            CT_FFT.FFT(fftBuffer);

            //calculate power spectrum (magnitude) values from fft[]
            for (int i = 0; i < audioBuffer.Length / 2 - 1; i += 2)
            {
                double real = fftBuffer[2 * i].Real;
                double imaginary = fftBuffer[2 * i + 1].Imaginary;
                magnitude[i] = Math.Sqrt(real * real + imaginary * imaginary);
            }

            // find largest peak in power spectrum
            double max_magnitude = double.NegativeInfinity;
            double max_index = -1;
            for (int i = 0; i < audioBuffer.Length / 2 - 1; i++)
                if (magnitude[i] > max_magnitude)
                {
                    max_magnitude = magnitude[i];
                    max_index = i;
                }
            
            // convert index of largest peak to frequency
            frequency = (float)max_index * RECORDER_SAMPLERATE / audioBuffer.Length;

            //Fire event for passing back the recorded value.
            FinishedSampling(this, new FinishedSampalingEventArgs()
            {
                Frequency = new Hz() { CyclesPerSecond = frequency }
            });

        }

        public void Dispose()
        {
            _recorder.Stop();
            ((IDisposable)_recorder).Dispose();
            GC.SuppressFinalize(this);
        }
    }
}