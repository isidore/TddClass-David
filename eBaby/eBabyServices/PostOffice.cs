using System;
using System.Collections.Generic;

namespace eBabyServices
{
    public class PostOffice
    {
        private static PostOffice instance;

        private readonly List<String> _log;

        private PostOffice()
        {
            _log = new List<String>();
        }

        public static PostOffice GetInstance()
        {
            if (instance == null)
            {
                instance = new PostOffice();
            }
            return instance;
        }

        public int Size()
        {
            return _log.Count;
        }

        public void SendEMail(String address, String message)
        {
            String logString = String.Format("<sendEMail address=\"{0:s}\" >{1:s}</sendEmail>\n", address, message);
            _log.Add(logString);
        }

        public String FindEmail(String to, String messageContains)
        {
            String ret = "";
            String log = "";
            for (int i = 0; i < _log.Count; i++)
            {
                log = _log[i];
                if (log.Contains(String.Format("address=\"{0:s}\"", to)))
                {
                    if (log.Contains(messageContains))
                        ret += log;
                }
            }
            return ret;
        }

        public bool DoesLogContain(String to, String message)
        {
            bool ret = false;
            String line = "";
            for (int i = 0; i < _log.Count; i++)
            {
                line = _log[i];
                if (line.Contains(to))
                {
                    if (line.Contains(message))
                        ret = true;
                }
            }
            return ret;
        }

        public void Clear()
        {
            _log.Clear();
        }
    }
}