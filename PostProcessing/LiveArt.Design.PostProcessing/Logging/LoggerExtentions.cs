using System;

namespace LiveArt.Design.PostProcessing.Logging
{
    internal static class LoggerExtentions
    {
        public static  void DoActionWithLog(this ILogger logger,string actionName, string targetName, Action action)
        {
            logger.Log("Start '{0}' for '{1}'", actionName, targetName);
            try
            {
                action();
                logger.Log("Finish '{0}' for '{1}'", actionName, targetName);
            }
            catch (System.Exception ex)
            {
                logger.Fail("Failed '{0}' for '{1}':{2}", actionName, targetName, ex.ToString());
            }

        }
    }
}
