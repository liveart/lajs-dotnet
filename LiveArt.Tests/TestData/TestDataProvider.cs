using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LiveArt.Tests.TestData
{
    public class TestDataProvider
    {
        public static string DesignJson
        {
            get { return File.ReadAllText("TestData/Design.json"); }
        }

        public static string QuoteInfoJson
        {
            get { return File.ReadAllText("TestData/QuoteInfo.json"); }
        }

    }
}
