using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Environment = Android.OS.Environment;

namespace Virtual_Guitar_Teacher.Controller.Libraries
{
    static class Logger
    {
        static readonly object loggerLock = new object();
        const string fileName = "events.log";
        static string dataDirectoryAbsPath = Environment.DataDirectory.AbsolutePath;
        static string filePath = dataDirectoryAbsPath.ToString() + "\\" + fileName;

        /// <summary>
        /// Logs an exception message with the date and time to a file.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        /// <returns>Returns false in case of an exception, otherwise returns true.</returns>
        public static bool Log(Exception ex)
        {
            //Compose the entery.
            string strMsg = DateTime.Now.ToShortDateString() + ", " + DateTime.Now.ToShortTimeString() + ": " + ex.Message;

            try
            {
                lock (loggerLock)
                {
                    if (File.Exists(filePath))
                        //If the file is larger than 65,535 bytes - delete it.
                        if (new FileInfo(filePath).Length > ushort.MaxValue)
                            File.Delete(filePath);

                    //Create a new file or append to it.
                    StreamWriter sw = new StreamWriter(filePath, true, Encoding.UTF8);

                    //Append the new entery.
                    sw.WriteLine(strMsg);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}