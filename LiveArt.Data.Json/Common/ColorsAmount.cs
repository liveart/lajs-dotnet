using System;
using System.Linq;

namespace LiveArt.Data.Json.Common
{
    public class ColorsAmount
    {
        public Int64? Value { get; set; }
        public bool IsManualProcessRequred {
            get { return Value == null; }
        }

        public static ColorsAmount Parse(string colorsAmount){
            string[] manualRequiredString={"processColors",string.Empty,null};
            int pasedNum;

            if (int.TryParse(colorsAmount, out pasedNum)) return new ColorsAmount(){Value=pasedNum};
            else if( manualRequiredString.Contains(colorsAmount))return new ColorsAmount(){Value=null};
            else throw new FormatException("Integer, 'processColors', null or empty string expected");
        }

        public static implicit operator ColorsAmount(string colorsAmount)// to prevent write custom desirealizer
        {
            return ColorsAmount.Parse(colorsAmount);
        }

        public static implicit operator ColorsAmount(Int64 colorsAmount)
        {
            return new ColorsAmount(){Value=colorsAmount};
        }


    }
}
