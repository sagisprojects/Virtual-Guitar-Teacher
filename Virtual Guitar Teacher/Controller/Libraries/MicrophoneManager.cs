using System;
using Environment = Android.OS.Environment;
using Android.Media;
using Android.Util;

namespace Virtual_Guitar_Teacher.Controller.Libraries
{
    public delegate void FinishedSamplingEventHandler(object sender, FinishedSampalingEventArgs e);

    public sealed class MicrophoneManager : IDisposable
    {
        public event FinishedSamplingEventHandler FinishedSampling;

        private string _defaultRecordingFilePath = @"\VGT\Documents\Recordings\";

        private const ChannelIn RECORDER_CHANNELS = ChannelIn.Mono;
        private const Encoding RECORDER_AUDIO_ENCODING = Encoding.Pcm16bit;
        private const int RECORDER_SAMPLERATE = 44100;
        private const byte RECORDER_BPP = 16;
        private int _bufferSizeInBytes;

        private AudioRecord _recorder;

        /// <summary>
        /// Initializes all recording parameters
        /// and records sound input for 350 iterations.
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

        public void Listen()
        {
            int _numberOfReadBytes = 0;
            byte[] _audioBuffer = new byte[_bufferSizeInBytes]; //Analoge
            bool _recording = false;
            float[] _tempFloatBuffer = new float[3];
            int _tempIndex = 0;
            int _totalReadBytes = 0;
            byte[] _totalByteBuffer = new byte[60 * RECORDER_SAMPLERATE * 2]; //Digital
            float temp = 0.0f;
            short sample = 0;

            // Start Recording.
            _recorder.StartRecording();

            // While data is inputted from the microphone.
            while (true)
            {
                float totalAbsValue = 0.0f;
                sample = 0;

                _numberOfReadBytes = _recorder.Read(_audioBuffer, 0, _bufferSizeInBytes);

                // Analyze Sound.
                for (int i = 0; i < _bufferSizeInBytes; i += 2)
                {
                    sample = (short)((_audioBuffer[i]) | _audioBuffer[i + 1] << 8);
                    totalAbsValue += Math.Abs(sample) / (_numberOfReadBytes / 2);
                }

                //Fire event for passing back the recorded value(s).
                FinishedSampling(this, new FinishedSampalingEventArgs() { SoundSample = sample });

                // Analyze temp buffer.
                _tempFloatBuffer[_tempIndex % 3] = totalAbsValue;
                temp = 0.0f;
                for (int i = 0; i < 3; ++i)
                    temp += _tempFloatBuffer[i];

                if ((temp >= 0 && temp <= 350) && _recording == false)
                {
                    Log.Info("TAG", "1");
                    _tempIndex++;
                    continue;
                }

                if (temp > 350 && _recording == false)
                {
                    Log.Info("TAG", "2");
                    _recording = true;
                }

                #region Comment
                if ((temp >= 0 && temp <= 350) && _recording == true)
                {
                    Log.Info("TAG", "Save audio to file.");

                    // Save audio to file.
                    Java.IO.File filepath = Environment.GetExternalStoragePublicDirectory(
                        Environment.DataDirectory.AbsolutePath);
                    Java.IO.File file = new Java.IO.File(_defaultRecordingFilePath, "AudioRecorder");
                    if (!file.Exists())
                        file.Mkdirs();

                    string fileName = file.AbsolutePath + "/" + DateTime.Now.ToShortTimeString() + ".wav";

                    long totalAudioLen = 0;
                    long totalDataLen = totalAudioLen + 36;
                    long longSampleRate = RECORDER_SAMPLERATE;
                    int channels = 1;
                    long byteRate = RECORDER_BPP * RECORDER_SAMPLERATE * channels / 8;
                    totalAudioLen = _totalReadBytes;
                    totalDataLen = totalAudioLen + 36;
                    byte[] finalBuffer = new byte[_totalReadBytes + 44];

                    finalBuffer[0] = Convert.ToByte('R');  // RIFF/WAVE header
                    finalBuffer[1] = Convert.ToByte('I');
                    finalBuffer[2] = Convert.ToByte('F');
                    finalBuffer[3] = Convert.ToByte('F');
                    finalBuffer[4] = (byte)(totalDataLen & 0xff);
                    finalBuffer[5] = (byte)((totalDataLen >> 8) & 0xff);
                    finalBuffer[6] = (byte)((totalDataLen >> 16) & 0xff);
                    finalBuffer[7] = (byte)((totalDataLen >> 24) & 0xff);
                    finalBuffer[8] = Convert.ToByte('W');
                    finalBuffer[9] = Convert.ToByte('A');
                    finalBuffer[10] = Convert.ToByte('V');
                    finalBuffer[11] = Convert.ToByte('E');
                    finalBuffer[12] = Convert.ToByte('f');  // 'fmt ' chunk
                    finalBuffer[13] = Convert.ToByte('m');
                    finalBuffer[14] = Convert.ToByte('t');
                    finalBuffer[15] = Convert.ToByte(' ');
                    finalBuffer[16] = 16;  // 4 bytes: size of 'fmt ' chunk
                    finalBuffer[17] = 0;
                    finalBuffer[18] = 0;
                    finalBuffer[19] = 0;
                    finalBuffer[20] = 1;  // format = 1
                    finalBuffer[21] = 0;
                    finalBuffer[22] = (byte)channels;
                    finalBuffer[23] = 0;
                    finalBuffer[24] = (byte)(longSampleRate & 0xff);
                    finalBuffer[25] = (byte)((longSampleRate >> 8) & 0xff);
                    finalBuffer[26] = (byte)((longSampleRate >> 16) & 0xff);
                    finalBuffer[27] = (byte)((longSampleRate >> 24) & 0xff);
                    finalBuffer[28] = (byte)(byteRate & 0xff);
                    finalBuffer[29] = (byte)((byteRate >> 8) & 0xff);
                    finalBuffer[30] = (byte)((byteRate >> 16) & 0xff);
                    finalBuffer[31] = (byte)((byteRate >> 24) & 0xff);
                    finalBuffer[32] = (byte)(2 * 16 / 8);  // block align
                    finalBuffer[33] = 0;
                    finalBuffer[34] = RECORDER_BPP;  // bits per sample
                    finalBuffer[35] = 0;
                    finalBuffer[36] = Convert.ToByte('d');
                    finalBuffer[37] = Convert.ToByte('a');
                    finalBuffer[38] = Convert.ToByte('t');
                    finalBuffer[39] = Convert.ToByte('a');
                    finalBuffer[40] = (byte)(totalAudioLen & 0xff);
                    finalBuffer[41] = (byte)((totalAudioLen >> 8) & 0xff);
                    finalBuffer[42] = (byte)((totalAudioLen >> 16) & 0xff);
                    finalBuffer[43] = (byte)((totalAudioLen >> 24) & 0xff);

                    for (int i = 0; i < _totalReadBytes; ++i)
                        finalBuffer[44 + i] = _totalByteBuffer[i];

                    /*FileStream fileStream;
                    int offset = 0;

                    try
                    {
                        fileStream = new FileStream(fileName, FileMode.Create);
                        try
                        {
                            fileStream.Write(finalBuffer, offset, finalBuffer.Length);
                            fileStream.Close();
                        }
                        catch (IOException ioex)
                        {
                            Logger.Log(ioex);
                        }
                    }
                    catch (FileNotFoundException fnfex)
                    {
                        Logger.Log(fnfex);
                    }*/

                    _tempIndex++;
                    break;
                }
                #endregion

                Log.Info("TAG", "Recording Sound.");
                for (int i = 0; i < _numberOfReadBytes; i++)
                    _totalByteBuffer[_totalReadBytes + i] = _audioBuffer[i];
                _totalReadBytes += _numberOfReadBytes;

                _tempIndex++;
            }
        }

        public void Dispose()
        {
            ((IDisposable)_recorder).Dispose();
            GC.SuppressFinalize(this);
        }
    }

    public class FinishedSampalingEventArgs : EventArgs
    {
        public short SoundSample { get; set; }
    }
}