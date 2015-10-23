using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading;
using ThreadState = System.Threading.ThreadState;

namespace Utility.Logging
{
    public sealed class MultiProcessFileTraceListener : TraceListener
    {
        private const long _def_fileSize = 5242880L;
        public const string MAX_LOG_FILES = "maxlogfiles";
        private readonly bool _disposed = false;
        private readonly MultiProcessLogFileWriter _logFile;
        private readonly Thread _writeThread;

        public MultiProcessFileTraceListener(string input)
        {
            var strArray = input.Split('?');
            long fileSize;
            if (strArray.Length > 1)
            {
                try
                {
                    fileSize = long.Parse(strArray[1]);
                }
                catch
                {
                    fileSize = 5242880L;
                }
            }
            else
                fileSize = 5242880L;
            _logFile = new MultiProcessLogFileWriter(strArray[0], fileSize);
            _writeThread = new Thread(_logFile.ThreadStartProc);
            _writeThread.IsBackground = true;
            _writeThread.Start();
        }

        public string LogFile
        {
            get { return _logFile.LogFile.FullName; }
        }

        ~MultiProcessFileTraceListener()
        {
            Dispose(false);
        }

        public override void Write(string message)
        {
            _logFile.Write(message);
        }

        public override void WriteLine(string message)
        {
            _logFile.WriteLine(message);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (_disposed)
                    return;
                _logFile.Dispose();
                Thread.Sleep(1000);
                if (_writeThread != null && _writeThread.ThreadState != ThreadState.Aborted)
                    _writeThread.Abort();
            }
            else if (!_disposed && (_writeThread != null && _writeThread.ThreadState != ThreadState.Aborted))
                _writeThread.Abort();
        }

        private class MultiProcessLogFileWriter : IDisposable
        {
            private readonly AutoResetEvent _addedToList;
            private readonly bool _disposed = false;
            private readonly long _fileSize;
            private readonly ArrayList _logStatementBuffer;
            private bool _kill;
            private Mutex _mutex;

            public MultiProcessLogFileWriter(string fileName, long fileSize)
            {
                _fileSize = fileSize;
                _addedToList = new AutoResetEvent(false);
                _logStatementBuffer = new ArrayList();
                LogFile = new FileInfo(fileName);
                InitializeMutex(fileName);
            }

            public FileInfo LogFile { get; private set; }

            public void Dispose()
            {
                Dispose(true);
            }

            ~MultiProcessLogFileWriter()
            {
                Dispose(false);
            }

            public void ThreadStartProc()
            {
                try
                {
                    while (!_kill)
                    {
                        _addedToList.WaitOne();
                        FlushBuffer();
                    }
                }
                catch (ThreadAbortException ex)
                {
                }
                catch (Exception ex)
                {
                    MessageLog.LogLogFailure(ex.ToString());
                }
            }

            public void Write(string message)
            {
                lock (_logStatementBuffer)
                    _logStatementBuffer.Add(new LogStatement(false, message));
                _addedToList.Set();
            }

            public void WriteLine(string message)
            {
                lock (_logStatementBuffer)
                    _logStatementBuffer.Add(new LogStatement(true, message));
                _addedToList.Set();
            }

            private void InitializeMutex(string fileName)
            {
                _mutex = new Mutex(false,
                    fileName.Replace(":", "_").Replace("\\", "_").Replace("/", "_").Replace(".", "_"));
            }

            private ArrayList GetBufferCopy()
            {
                ArrayList arrayList = null;
                lock (_logStatementBuffer)
                {
                    arrayList = new ArrayList(_logStatementBuffer);
                    _logStatementBuffer.Clear();
                }
                return arrayList;
            }

            private void FlushBuffer()
            {
                _mutex.WaitOne();
                try
                {
                    var bufferCopy = GetBufferCopy();
                    if (bufferCopy == null)
                        return;
                    WriteStatements(bufferCopy);
                    LogFile.Refresh();
                    lock (LogFile)
                    {
                        if (LogFile.Exists && LogFile.Length > _fileSize)
                        {
                            var local_1 = "*" + Path.GetFileNameWithoutExtension(LogFile.Name) + "*" + LogFile.Extension;
                            var local_2 = LogFile.Directory;
                            var local_3 = Path.GetFileNameWithoutExtension(LogFile.Name) + (object) "_" +
                                          DateTime.UtcNow.ToString("yyyyMMdd-hhmmss") + "_" +
                                          (string) (object) Guid.NewGuid() + LogFile.Extension;
                            File.Move(LogFile.FullName, Path.Combine(local_2.FullName, local_3));
                            DeleteOldFiles(local_1, local_2, LogFile);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageLog.LogLogFailure(ex.ToString());
                }
                finally
                {
                    _mutex.ReleaseMutex();
                }
            }

            private static int CompareFiles(FileInfo f1, FileInfo f2)
            {
                return f1.LastWriteTime.CompareTo(f2.LastWriteTime);
            }

            private static void DeleteOldFiles(string filePattern, DirectoryInfo workingDirectory, FileInfo _file)
            {
                var length = workingDirectory.GetFiles(filePattern).Length;
                var s = ConfigurationManager.AppSettings["maxlogfiles"];
                var result = 5;
                if (!int.TryParse(s, out result))
                    result = 5;
                if (length > result)
                {
                    var list = new List<FileInfo>();
                    foreach (var fileInfo in workingDirectory.GetFiles(filePattern))
                        list.Add(fileInfo);
                    list.Sort(CompareFiles);
                    for (var index = 0; index < list.Count; ++index)
                    {
                        if (index < list.Count - result)
                        {
                            try
                            {
                                list[index].Delete();
                                MessageLog.LogAlways(string.Format("Log File Deleted: {0} ", list[index].FullName));
                            }
                            catch (Exception ex)
                            {
                                MessageLog.LogLogFailure(ex.ToString());
                            }
                        }
                    }
                }
                foreach (var fileInfo in workingDirectory.GetFiles(filePattern))
                {
                    try
                    {
                        if (DateTime.Now.Subtract(fileInfo.LastWriteTime).TotalDays > 7.0)
                            fileInfo.Delete();
                    }
                    catch (Exception ex)
                    {
                        MessageLog.LogLogFailure(ex.ToString());
                    }
                }
            }

            private void WriteStatements(ArrayList logStatements)
            {
                if (!LogFile.Directory.Exists)
                    LogFile.Directory.Create();
                var streamWriter = LogFile.Exists ? LogFile.AppendText() : LogFile.CreateText();
                using (streamWriter)
                {
                    foreach (LogStatement logStatement in logStatements)
                    {
                        if (logStatement.WriteLine)
                            streamWriter.WriteLine(logStatement.Message);
                        else
                            streamWriter.Write(logStatement.Message);
                    }
                }
            }

            protected virtual void Dispose(bool disposing)
            {
                if (_disposed)
                    return;
                _kill = true;
                if (_addedToList != null)
                    _addedToList.Set();
            }

            private struct LogStatement
            {
                public readonly bool WriteLine;
                public readonly string Message;

                public LogStatement(bool writeLine, string message)
                {
                    WriteLine = writeLine;
                    Message = message;
                }
            }
        }
    }
}