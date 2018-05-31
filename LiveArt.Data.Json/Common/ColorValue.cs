using System;

namespace LiveArt.Data.Json.Common
{
    public class ColorValue     // we know about System.Drawing.Color structure
    {
        
        public Int32 Value{get;private set;}

        public byte R { get { return (byte)(this.Value>>16);} }
        public byte G { get { return (byte)(this.Value>>8);} }
        public byte B { get { return (byte)this.Value;} }
        

        public ColorValue(Int32 value=0)
        {
            this.Value = value;
        }

        #region  Object_methos_overrides

        public override int GetHashCode()
        {
            return this.Value;
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (System.Object.ReferenceEquals(this, obj)) return true;
            if(obj is ColorValue) return this.Value.Equals(((ColorValue)obj).Value);
            return this.Value.Equals(obj);
        }

        public static bool operator ==(ColorValue color1,ColorValue color2){

                   
            if (System.Object.ReferenceEquals(color1, color2)) return true;
            
            if( (object)color1==null || (object)color2 ==null)return false;
            return color1.Equals(color2);
        }

        public static bool operator !=(ColorValue color1, ColorValue color2)
        {
            return !(color1 == color2);
        }
        #endregion

        #region ToXXXString
        public override string ToString()
        {
            return ToHtmlHexString();
        }

        public string ToHtmlHexString()
        {
            return string.Format("#{0:X6}", this.Value);
        }
        #endregion

        #region From

        public static ColorValue FromRGB(byte r, byte g, byte b)
        {
            return new ColorValue(((Int32)r << 16) | (g << 8) | b);
        }

        public static ColorValue FromHtmlHexString(string htmlHexString)
        {
            //return new Color(0xF0E0D0);
            var htmlHexStringWitoutSpaces = htmlHexString.TrimStart(' ');
            if (htmlHexStringWitoutSpaces.StartsWith("#"))
            {
                var htmlHexStringWitoutSharp = htmlHexStringWitoutSpaces.Substring(1);
                var normHex = NormalizeHex(htmlHexStringWitoutSharp);
                var value = Int32.Parse(normHex, System.Globalization.NumberStyles.HexNumber);
                return new ColorValue(value);   
            }
            throw new FormatException();
            
        }

        private static string NormalizeHex(string hexStr){
            if (hexStr.Length != 3) return hexStr;
            // dublicate each char
            return new String(new[] { 
                                        hexStr[0], hexStr[0] ,
                                        hexStr[1], hexStr[1] ,
                                        hexStr[2], hexStr[2]
                                    });
        }

        public static implicit operator ColorValue(string str){ // prevent write custom json value converter
            return ColorValue.FromHtmlHexString(str);
        }

        #endregion




    }
    
}
