using System.Collections.Generic;
using System.Linq;
using LiveArt.Design.PostProcessing.Logging;

namespace LiveArt.Design.PostProcessing.Tests
{
    public static class FluentAssertionCustom
    {

      /*  public static void ContainsMessage(this GenericCollectionAssertions<LogLine> logLines, string expectedMessage)
        {
            var messages=logLines.Subject.Select(logLine=>logLine.Message);
            messages.Should().Contain(expectedMessage);
            
        }

        public static void ContainsMessage(this GenericCollectionAssertions<LogLine> logLines, IEnumerable<string> expectedMessage)
        {
            
            var messages = logLines.Subject.Select(logLine => logLine.Message);
            messages.Should().Contain(expectedMessage);

        }
        */
        public static IEnumerable<string> Messages(this IEnumerable<LogLine> logLines)
        {
            return logLines.Select(logLine => logLine.Message);
        }


    }
}
