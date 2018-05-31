using System;
using System.Collections.Generic;

namespace LiveArt.Design.PostProcessing.Logging
{
    internal class LoggerInMemory:ILogger
    {
        private IList<LogLine> LogLines;

        public LoggerInMemory(){
            this.LogLines = new List<LogLine>();
        }

        public void Log(string messageFormatString, params object[] args)
        {
            this.AddLine(LogLineType.Log,messageFormatString,args);
        }

        public void Fail(string failMessageFormat,params object[] args){
            this.AddLine(LogLineType.Fail,failMessageFormat,args);
        }

        private void AddLine(LogLineType messageType, string messageFormatString, params object[] args){
            this.LogLines.Add(
                new LogLine(){
                    Time=DateTime.Now,
                    Message=string.Format(messageFormatString,args),
                    Type=messageType
                }
            );
        }

        

        public IEnumerable<LogLine> GetLog()
        {
            return this.LogLines;
        }

        void Clear()
        {
            this.LogLines.Clear();
        }
    }
}
