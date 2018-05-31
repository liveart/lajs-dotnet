namespace LiveArt.Design.PostProcessing.Logging
{
    public interface ILogger
    {
        void Log(string messageFormatString, params object[] args);
        void Fail(string failMessage, params object[] args);
    }
}
