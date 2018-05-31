using System;

namespace LiveArt.Design.PostProcessing.Logging
{
    public enum LogLineType { Log,Fail}
    public class LogLine
    {
        public DateTime Time;
        public string Message;
        public Enum Type;
    }
}
