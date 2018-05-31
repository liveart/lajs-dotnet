using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace LiveArt.Design.PostProcessing.Domain
{
    internal class Rect  //helper/common class to represent editablearea/viewbox e.t.c
    {
        public Double X1{get;set;}
        public Double Y1{get;set;}
        public Double X2{ get; set;}
        public Double Y2 { get; set; }


         
        public Rect(Double x1=0,Double y1=0,Double x2=0,Double y2=0)
        {
            this.X1 = x1;
            this.Y1 = y1;
            this.X2 = x2;
            this.Y2 = y2;
        }
       
        

        // short alias for X1,X2
        public Double X { get { return X1; } set { X1 = value; } }
        public Double Y { get { return Y1; } set { Y1 = value; } }

        public Double Width
        {
            get{return X2 - X1;}
            set{X2 = X1 + value;}
        }

        public Double Height
        {
            get { return Y2 - Y1; }
            set { Y2 = Y1 + value; }
        }

        public string ToTextAreaString()
        {
            return string.Format("{0} {1} {2} {3}",
                 X1,
                 Y1,
                 X2,
                 Y2
                ).Replace(",",".");

        }

        public string ToViewBoxString()
        {
            return string.Format("{0} {1} {2} {3}",
                X,
                Y,
                Width,
                Height
               ).Replace(",", ".");

        }

        #region Parse

        public static Regex regexFourDouble = new Regex(
                                                      "^\\s*(?<x1>-?\\d+[\\.,]?\\d*)\\s+(?<y1>-?\\d+[\\.,]?\\d*)\\s" +
                                                      "+(?<x2>-?\\d+[\\.,]?\\d*)\\s+(?<y2>-?\\d+[\\.,]?\\d*)\\s*$",
                                                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled
                                                    );

        private static Double[] parseFourDoubleString(string stringWithDoubles)
        {
            var m = regexFourDouble.Match(stringWithDoubles.Replace(",", "."));
            if (m.Success)
            {
                var coords = m.Groups.Cast<Group>()
                                .Skip(1) // ignore whole capture, only "captured groups" required
                                .Select(g => Double.Parse(g.Value, System.Globalization.CultureInfo.InvariantCulture))
                                .ToArray();

                return coords;
            }
            else
            {
                throw new FormatException("expected string with 4 double, space saparated");
            }
            
        }
        
        /// <param name="textAreaAttributeValue">input string in format "X1 Y1 X2 Y2"</param>
        public static Rect ParseFromTextArea(string textAreaAttributeValue)
        {
            var coords = parseFourDoubleString(textAreaAttributeValue);

            return new Rect()
            {
                X1 = coords[0],
                Y1 = coords[1],

                X2 = coords[2],
                Y2 = coords[3],
            };
        }

        /// <param name="textAreaAttributeValue">input string in format "X Y Width Height"</param>
        public static Rect ParseFromViewBox(string viewBoxAttributeValue)
        {

            var coords = parseFourDoubleString(viewBoxAttributeValue);

                return new Rect()
                {
                    X=coords[0],
                    Y=coords[1],
                    Width=coords[2],
                    Height=coords[3]
                };
        

        }
        #endregion

        #region Equals and ToString
        public override bool Equals(object obj)
        {
            var r = obj as Rect;
            if (r == null) return false;

            return r.X1 == this.X1
                && r.Y1 == this.Y1
                && r.X2 == this.X2
                && r.Y2 == this.Y2;
        }

        public static bool operator ==(Rect a, Rect b)
        {
            if (System.Object.ReferenceEquals(a, b)){return true;}
            if (((object)a == null) || ((object)b == null)){return false;}
            return a.Equals(b);
        }

        public static bool operator !=(Rect a,Rect b){
            return !(a == b);
        }

        public override string ToString()
        {
            return string.Format("X1:{0},Y2:{1},X2:{2},Y2:{3},Width:{4},Height:{5}",
                  X1, Y1, X2, Y2, Width, Height);
                ;

        }

        #endregion


    }
}
