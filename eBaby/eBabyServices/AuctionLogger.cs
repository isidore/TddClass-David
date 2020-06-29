using System;
using System.IO;

namespace eBabyServices
{
    public class AuctionLogger
    {
        private readonly FileInfo m_File;

        public AuctionLogger(String filename)
        {
            m_File = new FileInfo(filename);
        }

        public void Log(String message)
        {
            StreamWriter logWriter = OpenFileWriter();
            if (logWriter != null)
            {
                try
                {
                    logWriter.WriteLine(message);
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.StackTrace);
                }
                finally
                {
                    CloseWriter(logWriter);
                }
            }
        }

        public bool FindMessage(String message)
        {
            StreamReader logReader = OpenFileReader();
            if (logReader != null)
            {
                try
                {
                    while (!logReader.EndOfStream)
                    {
                        if (logReader.ReadLine().Equals(message))
                            return true;
                    }
                }
                finally
                {
                    CloseReader(logReader);
                }
            }
            return false;
        }

        public String ReturnMessage(String message)
        {
            StreamReader logReader = OpenFileReader();
            if (logReader != null)
            {
                try
                {
                    logReader.ReadLine();
                    while (!logReader.EndOfStream)
                    {
                        if (logReader.ReadLine().Equals(message))
                            return message;
                    }
                }
                finally
                {
                    CloseReader(logReader);
                }
            }
            return "";
        }

        private void CloseWriter(StreamWriter logWriter)
        {
            try
            {
                logWriter.Flush();
                logWriter.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        private void CloseReader(StreamReader logReader)
        {
            try
            {
                logReader.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        private StreamWriter OpenFileWriter()
        {
            StreamWriter logWriter = null;
            try
            {
                logWriter = m_File.AppendText();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
            }
            return logWriter;
        }

        public bool FileExists()
        {
            return m_File.Exists;
        }

        public void Clear()
        {
            if (FileExists())
            {
                m_File.Delete();
            }
        }

        private StreamReader OpenFileReader()
        {
            StreamReader logReader = null;
            try
            {
                logReader = m_File.OpenText();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
            }
            return logReader;
        }

        public string GetAllMessages()
        {
            return  File.ReadAllText(m_File.FullName)  ;
        }
    }
}