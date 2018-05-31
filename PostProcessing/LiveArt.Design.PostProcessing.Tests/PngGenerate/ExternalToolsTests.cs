using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LiveArt.Design.PostProcessing.PngGenerate;
using NUnit.Framework;

namespace LiveArt.Design.PostProcessing.Tests.PngGenerate
{
    [TestFixture]
    public class ExternalToolsTests
    {
        const string IpConfigPath= @"c:\Windows\System32\ipconfig.exe";

        private IExternalTools Tool;// class under test

        [SetUp]
        public void SetUp()
        {
            Tool=new ExternalTools(IpConfigPath);
        }

        [Test]
        public void CanExecute_ReturnTrue_If_FileExists()
        {
            //arrange
            // in SetUp() configured exists path

            //action
            Tool.CanExecute()
            // false
            .Should().BeTrue($" {IpConfigPath} is valid tool path");

        }

        [Test]
        public void CanExecute_ReturnFakse_If_FileNotExists()
        {
            //arrange
            var invalidPath = IpConfigPath.Replace("ipconfig", "ipNotConfig");
            Tool = new ExternalTools(invalidPath);
            // in SetUp() configured exists path

            //action
            Tool.CanExecute()
            // false
            .Should().BeFalse($" {invalidPath} is not valid tool path");

        }

        [Test]
        public void Execute_WithEmptyArgs_Should_ResultSuccess()
        {
            //action
            var result=Tool.Execute(string.Empty);

                //assert
            result.Success.Should().BeTrue();
        }

        [Test]
        public void Execute_WithEmptyArgs_Should_ReturnStdOut()
        {
            //action
            var result = Tool.Execute(string.Empty);

            //assert
            result.StdOut.Should()
                .Contain("Windows IP Configuration");
        }

        [Test]

        public void Execute_WithArgs_Should_ReturnStdOut()
        {
            //action
            var result = Tool.Execute("/help");

            //assert
            result.StdOut.Should()
                .Contain("Examples:");
        }
    }
}
