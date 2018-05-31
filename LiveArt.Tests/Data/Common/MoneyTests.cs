using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FluentAssertions;
using LiveArt.Data.Json.Common;

namespace LiveArt.Tests.Data.Common
{
    [TestFixture]
    public class MoneyTests
    {
        #region Equals
        [Test]
        public void Equals_AssertAreEquals()
        {
            Assert.AreEqual(new Money(12.15), new Money(12.15));
        }

        [Test]
        public void Equals_Money2Money()
        {
            Assert.IsTrue(new Money(12.15).Equals(new Money(12.15)));
        }
        [Test]
        public void Equals_Money2Double()
        {
            Assert.IsTrue(new Money(12.15).Equals(12.15));
        }

        

        #endregion

        #region ToString()
        [Test]
        public void ToStringToDigitsAfterDot()
        {
            new Money(12.15).ToString().Should().Be("$ 12.15");
        }

        [Test]
        public void ToStringZeroAfterDot()
        {
            new Money(12).ToString().Should().Be("$ 12.00");
        }

        [Test]
        public void ToStringTotalZero()
        {
            new Money(0).ToString().Should().Be("$ 0.00");
        }

        [Test]
        public void ToStringBigNumber()
        {
            new Money(1234).ToString().Should().Be("$ 1 234.00");
        }
        #endregion

        #region Parse()

        [Test]
        public void Parse_ToDigitsAfterDot()
        {
            Money.Parse("$ 12.15").Should().Be(new Money(12.15));
        }

        
        [Test]
        public void Parse_ZeroAfterDot()
        {
            Money.Parse("$ 12.00").Should().Be(new Money(12));
        }

        [Test]
        public void Parse_TotalZero()
        {
            
            Money.Parse("$ 0.00").Should().Be(new Money(0));
        }

        [Test]
        public void Parse_BigNumber()
        {
            Money.Parse("$ 1 234.00").Should().Be(new Money(1234));
        }

        [Test][ExpectedException(typeof(ArgumentNullException))]
        public void Parse_Null()
        {
            Money.Parse(null);
        }


        #endregion

        #region Operators
        [Test]
        public void Operator_Add()
        {
            //arrange
            var m1=new Money(11.11);
            var m2=new Money(22.22);

            //action
            var sum=m1+m2;
            
            //assert
            sum.Should().Be(33.33);
        }

        [Test]
        public void Operator_Money_Multi_Double()
        {
            var m = new Money(100);

            //action
            var result = m * 2;

            //assert
            result.Should().Be(200.00);
        }

        [Test]
        public void Operator_Double_Multi_Money()
        {
            var m = new Money(100);

            //action
            var result = 2 * m ;

            //assert
            result.Should().Be(200.00);
        }

        [Test]
        public void Operator_MultiNegative()
        {
            (-1*(new Money(100))).Should().Be(-100.0);
        }

        [Test]
        public void Operator_Minus()
        {
            //arrange
            
            var m2 = new Money(22.22);
            var sum = new Money(33.33);
            //action
            var m1 = sum - m2;

            //assert
            m1.Should().Be(11.11);
        }

        [Test]
        public void Operator_Priority()
        {
            var m = new Money(100.00);
            
            
            //action
            var result = m + 2 * m; // expected  (100+(100*2))==300, not (100+100)*2==400

            //assert
            result.Should().Be(100.0 + 2 * 100);
        }

        [Test]
        public void Operator_Devide()
        {
            var m = new Money(100.00);
            var result = m / 2;
            result.Should().Be(50.0);
        }

        #endregion

        #region Linq
        [Test]
        public void Linq_Sum()
        {
            //arrange
            var ms=from v in (new []{100,200,300})
                   select new Money(v);
            //action
            var sum = ms.Sum();

            //assert
            sum.Should().Be(600.0);
        }

        [Test]
        public void Linq_Sum_WithSelector()
        {
            //arrange
            var values = (new[] { 100, 200, 300 });
                     
            //action
            var sum = values.Sum(v=>new Money(v));

            //assert
            sum.Should().Be(600.0);
        }

        #endregion

        #region Compare Operator
        [Test]
        public void Compare_EqualExpectedTrue()
        {
            var m1 = new Money(100);
            var m2 = new Money(100);

            Assert.IsTrue(m1 == m2);

        }

        [Test]
        public void Compare_EqualExpectedFalse()
        {
            var m1 = new Money(100);
            var m2 = new Money(200);

            Assert.IsFalse(m1 == m2);

        }

        [Test]
        public void Compare_NotEqualExpectedTrue()
        {
            var m1 = new Money(100);
            var m2 = new Money(200);

            Assert.IsTrue(m1 != m2);

        }

        [Test]
        public void Compare_NotEqualExpectedFalse()
        {
            var m1 = new Money(100);
            var m2 = new Money(100);

            Assert.IsFalse(m1 != m2);

        }

        [Test]
        public void Compare_WithNull()
        {
            var m = new Money();

          //  Assert.IsFalse(m == null);
        }

        [Test]
        public void Compare_LessTrue()
        {
            var m1 = new Money(100);
            var m2 = new Money(200);

            Assert.IsTrue(m1 < m2);

        }

        [Test]
        public void Compare_LessFalse()
        {
            var m1 = new Money(200);
            var m2 = new Money(100);

            Assert.IsFalse(m1 < m2);

        }

        [Test]
        public void Compare_GreatTrue()
        {
            var m1 = new Money(200);
            var m2 = new Money(100);

            Assert.IsTrue(m1 > m2);

        }

        [Test]
        public void Compare_GreatFalse()
        {
            var m1 = new Money(100);
            var m2 = new Money(200);

            Assert.IsFalse(m1 > m2);

        }

        [Test]
        public void Compare_LessOrEqual_EqualTrue()
        {
            var m1 = new Money(100);
            var m2 = new Money(100);

            Assert.IsTrue(m1 <= m2);
        }

        #endregion

        [Test]
        public void Inverse()
        {
            var m = new Money(100);

            //action
            var negativeM = -m;
            
            //assert
            negativeM.Should().Be(-100.0);
        }

        #region Implicit
        [Test]
        public void Implicit_FromString(){
            
            var moneyString="$ 12.15";

            //action
            Money m=moneyString;

            //assert
            m.Should().Be(12.15);

        }
        #endregion

    }
}
