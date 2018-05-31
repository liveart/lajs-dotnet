using System;
using System.Globalization;

namespace LiveArt.Data.Json.Common
{
    public class Money 
    {
        public double Value { get; set; }

        private static CultureInfo LiveArtCurrencyCulture;
        

        static Money()
        {
            LiveArtCurrencyCulture = (CultureInfo)CultureInfo.GetCultureInfo("en").Clone();
            LiveArtCurrencyCulture.NumberFormat = new NumberFormatInfo()
            {
                CurrencySymbol = "$ ",
                CurrencyGroupSeparator = " "

            };
        }
        

        public Money(double value = 0)
        {
            this.Value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj is Money) return this.Value.Equals(((Money)obj).Value);
            return this.Value.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public override string ToString() 
        {
            return this.Value.ToString("C", LiveArtCurrencyCulture);
        }

        public static Money Parse(string moneyString)
        {
            if (moneyString == null) throw new ArgumentNullException("moneyString"); // just for pass paramName to exception
            return new Money(double.Parse(moneyString, NumberStyles.Currency, LiveArtCurrencyCulture));

        }

        #region Operators
        public Money Add(Money m)
        {
            if (m == null) throw new ArgumentNullException("m");
            return new Money(this.Value+ m.Value);
        }

        public static Money operator +(Money m1,Money m2)
        {
            if (m1 == null) throw new ArgumentNullException("m1");
            if (m2 == null) throw new ArgumentNullException("m2");
            return m1.Add(m2);
        }

        public Money Multiplication(Double factor)
        {
            return new Money(this.Value * factor);
        }

        public static Money operator *(Money m, Double factor)
        {
            if (m == null) throw new ArgumentNullException("m");
            return m.Multiplication(factor);
        }
        public static Money operator *(Double multiFactor, Money m)
        {
            return m * multiFactor;
        }

        public Money Subtraction(Money m){
            if (m == null) throw new ArgumentNullException("m");
            return new Money(this.Value - m.Value);
        }

        public static Money operator -(Money m1, Money m2)
        {
            if (m1 == null) throw new ArgumentNullException("m1");
            if (m2 == null) throw new ArgumentNullException("m2");
            return m1.Subtraction(m2);
        }

        public Money Divide(Double factor)
        {
            return new Money(this.Value / factor);
        }

        public static Money operator /(Money m, Double factor)
        {
            if (m == null) throw new ArgumentNullException("m");
            return m.Divide(factor);
        }

        public static bool operator ==(Money m1, Money m2)
        {
           
            if (System.Object.ReferenceEquals(m1, m2)) return true;
            
            if( (object)m1==null || (object)m2 ==null)return false;
            return m1.Equals(m2);

        }

        public static bool operator !=(Money m1, Money m2)
        {
            return !(m1 == m2);
        }

        public static bool operator <(Money m1, Money m2)
        {
            if (m1 == null) throw new ArgumentNullException("m1");
            if (m2 == null) throw new ArgumentNullException("m2");
            return m1.Value < m2.Value;
        }

        public static bool operator >(Money m1, Money m2)
        {
            if (m1 == null) throw new ArgumentNullException("m1");
            if (m2 == null) throw new ArgumentNullException("m2");
            return m1.Value > m2.Value;
        }

        public static bool operator <=(Money m1, Money m2)
        {
            return (m1 < m2) || (m1==m2);
        }

        public static bool operator >=(Money m1, Money m2)
        {
            return (m1 > m2) || (m1 == m2);
        }

        #endregion

        //inverse
        public static Money operator -(Money m)
        {
            if (m == null) throw new ArgumentNullException("m");
            return new Money(-m.Value);
        }

        // implicit from string to avoid write custom json custom converter
        public static implicit operator Money(String moneyString){
            if(string.IsNullOrEmpty(moneyString))return new Money();
            else return Money.Parse(moneyString);
        }


    }
}
