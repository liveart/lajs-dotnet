using System;
using FluentAssertions;
using LiveArt.Design.PostProcessing.Domain;
using NUnit.Framework;

namespace LiveArt.Design.PostProcessing.Tests.Domain
{
    [TestFixture]
    public class RectTests
    {
        [Test]
        public void XY_is_aliases_for_X1Y1(){
            var r1 = new Rect(){X1 = 10,Y1 = 20,X2=30,Y2=40};
            var r2 = new Rect(10,20,30,40) ;

            //(r1.Equals(r2)).Should().BeTrue();
           // (r1 == r2).Should().BeTrue();
            //r1.Should().Be(r2);
            Assert.AreEqual(r1, r2);
        }

        [Test]
        public void Width_WhouldBeCalculatedFrom_X1X2()
        {
            var r = new Rect() { X1=10,X2=30};

            r.Width.Should().Be(20);
        }

        [Test]
        public void Height_WhouldBeCalculatedFrom_X1X2()
        {
            var r = new Rect() { Y1 = 10, Y2 = 30 };

            r.Height.Should().Be(20);
        }

        [Test]
        public void Width_ShoulUpdate_X2()
        {
            var r = new Rect() { X1 = 10, X2 = 20 };

            //action
            r.Width = 40;

            //assert
            r.X2.Should().Be(50);

        }

        [Test]
        public void Height_ShoulUpdate_Y2()
        {
            var r = new Rect() { Y1 = 10, Y2 = 20 };

            //action
            r.Height = 40;

            //assert
            r.Y2.Should().Be(50);

        }

        #region Parse

        //input string in format "X1 Y1 X2 Y2"</param>
        [TestCase("10 20 30 40", 10,20,30,40)]
        [TestCase("-10 20 -30 40", -10, 20, -30, 40)]
        [TestCase("   10 20 30 40   ", 10, 20, 30, 40)]
        [TestCase("10      20     30      40", 10, 20, 30, 40)]
        [TestCase("10.1 20.2 30.3 40.4", 10.1, 20.2, 30.3, 40.4)]
        [TestCase("10,1 20,2 30,3 40,4", 10.1, 20.2, 30.3, 40.4)]
        public void ParseFromTextArea(string inputString,double eX1,double eY1,double eX2,double eY2)
        {
            var expectedRect = new Rect(eX1, eY1, eX2, eY2);
            Rect.ParseFromTextArea(inputString)
                .Should().Be(expectedRect);
        }


        //input string in format "X Y Width Height"
        [TestCase("10 -20.3 30,3 40", 10, -20.3, 30.3, 40)]
        public void ParseFromViewBox(string inputString, double eX, double eY, double eWidth, double eHeight)
        {
            var expectedRect = new Rect(){X=eX,Y=eY,Width=eWidth,Height=eHeight};
            Rect.ParseFromViewBox(inputString)
                .Should().Be(expectedRect);
        }


        [TestCase("10 20 30")]
        [TestCase("10 20 30 40 50")]
        [TestCase("10 20bbb30 40 50")]
        public void ParseFromTextArea_OnInvalidStrings_ShouldRaiseException(string invalidInputString)
        {
            Action action = () =>
            {
                Rect.ParseFromTextArea(invalidInputString);
            };
            action.ShouldThrow<FormatException>();
        }

        [TestCase("10 20 30")]
        [TestCase("10 20 30 40 50")]
        [TestCase("10 20bbb30 40 50")]
        public void ParseFromViewBox_OnInvalidStrings_ShouldRaiseException(string invalidInputString)
        {
            Action action = () =>
            {
                Rect.ParseFromViewBox(invalidInputString);
            };
            action.ShouldThrow<FormatException>();
        }

    #endregion //Parse

        #region ToStrings

        [Test]
        public void ToTextAreaString(){

            var r=new Rect(10.1,-20.2,30,40);
            r.ToTextAreaString()
                .Should().Be("10.1 -20.2 30 40");
        }

        [Test]
        public void ToViewBoxString(){

            var r=new Rect(10.1,-20.2,30,40); //width 19.9, height 60.2
            r.ToViewBoxString()
                .Should().Be("10.1 -20.2 19.9 60.2");
        }
        
        #endregion //ToStrings


    }
}
