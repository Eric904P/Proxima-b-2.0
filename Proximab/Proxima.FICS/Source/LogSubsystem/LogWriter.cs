﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxima.FICS.Source.LogSubsystem
{
    public class LogWriter
    {
        private string _directory;

        public LogWriter(string directory)
        {
            _directory = directory;
        }

        public void Write(string text)
        {
            using (var logWriter = OpenOrCreateLogFile())
            {
                var output = $"{DateTime.Now.ToShortTimeString()} - {text}";
                logWriter.WriteLine(output);
            }
        }

        private string GetLogNameForDateTime(DateTime dateTime)
        {
            return dateTime.ToLongDateString() + ".log";
        }

        private StreamWriter OpenOrCreateLogFile()
        {
            var logFileName = GetLogNameForDateTime(DateTime.Now);
            var fullLogFilePath = _directory + "/" + logFileName;

            return new StreamWriter(fullLogFilePath, true);
        }
    }
}
