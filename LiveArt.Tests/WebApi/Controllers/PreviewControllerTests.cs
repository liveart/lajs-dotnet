using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Routing;
using NUnit.Framework;
using FluentAssertions;
using Moq;
using LiveArt.Tests.TestData;
using LiveArt.WebAPI.Sample.Controllers;
using LiveArt.WebAPI.Sample.Repositories;

namespace LiveArt.Tests.WebApi.Controllers
{
    [TestFixture]
    public class PreviewControllerTests
    {

        Mock<IDesignRepository> repositoryMoq;
        PreviewController controller;
       

        [SetUp]
        public void SetUp()
        {
            repositoryMoq = new Mock<IDesignRepository>();

            repositoryMoq.Setup(r => r.DesignExists(It.IsAny<string>())).Returns(false); // for "invalidDesignID
            repositoryMoq.Setup(r => r.GetJson(It.IsAny<string>())).Returns<string>(null);

            repositoryMoq.Setup(r => r.DesignExists("existDesignID")).Returns(true);
            repositoryMoq.Setup(r => r.GetJson("existDesignID")).Returns(TestDataProvider.DesignJson);

            controller = new PreviewController(repositoryMoq.Object);

            SetupUrlHelper();
        }

        [Test]
        public async void GetSvg_ExistLocation()
        {
            //arrange
            

            //action
            var response = controller.Svg("existDesignID","Front");
            var svg = await response.Content.ReadAsStringAsync();

            //assert
            svg.Should().StartWith("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?><svg");
            svg.Should().EndWith("</svg>");
        }

        [Test]
        public void GetSvg_CheckXmlContentType()
        {

            //action
            var response = controller.Svg("existDesignID", "Front");
            var responseContentType = response.Content.Headers.ContentType.MediaType;

            //assert
            responseContentType.Should().Be("image/svg+xml");

        }


        [Test]
        public void GetSvg_ContentDisposition_NotForceDownload()
        {

            //action
            var response = controller.Svg("existDesignID", "Front");
            var contentDisposition = response.Content.Headers.ContentDisposition;

            //assert
            contentDisposition.DispositionType.Should().Be("inline");
            contentDisposition.FileName.Should().Be("existDesignID.Front.svg");
        }

        [Test]
        public void GetSvg_ContentDisposition_ForceDownload()
        {

            //action
            var response = controller.Svg("existDesignID", "Front",true);
            var contentDisposition = response.Content.Headers.ContentDisposition;

            //assert
            contentDisposition.DispositionType.Should().Be("attachment");
            contentDisposition.FileName.Should().Be("existDesignID.Front.svg");
        }
            


        [Test][ExpectedException(typeof(PreviewController.LocationNotFoundException))]
        public void GetSvg_NotExistLocation()
        {
            //arrange
            

            //action
            // top location not exist in sample of design
            // ExpectedException
            var svg = controller.Svg("existDesignID", "Top"); 
        }

        [Test]
        public async void GetSvg_LocationNameIsCaseInsensetive()
        {
            //arrange
            

            //action
            var response = controller.Svg("existDesignID", "fRONt");
            var svg = await response.Content.ReadAsStringAsync();

            //assert
            svg.Should().NotBeNullOrEmpty();
        }

        [Test][ExpectedException(typeof(DesignController.DesignNotFoundException))]
        public void GetSvg_NotExistDesign()
        {
            //arrange
            //action
            var svg = controller.Svg("invalidDesignID", "Front"); // exception expected
        }

        [Test, Sequential]
        public async void GetSvg_ShouldFixRelativeImagesUrl(
            [Values(
              "href=\"../_LiveArtJS/products/",
              "href=\"../../fonts.css"
            )]string wrongUrl,

            [Values(
            "href=\"http://localhost/_LiveArtJS/products/",
            "href=\"http://localhost/_LiveArtJS/fonts/fonts.css"
            )]string rightUrl
            )
        {

             //action
             var response = controller.Svg("existDesignID", "Front");
            var svg = await response.Content.ReadAsStringAsync();

            //asserts
            svg.Should().NotContain(wrongUrl); // not fixed
            svg.Should().Contain(rightUrl); // not fixed
        }


        private void SetupUrlHelper()
        {
            var mUrlHelper = new Mock<UrlHelper>();
            mUrlHelper.Setup(m => m.Content("~/_LiveArtJS/")).Returns("http://localhost/_LiveArtJS/");
            mUrlHelper.Setup(m => m.Content("../../")).Returns("http://localhost/_LiveArtJS/");
            
            this.controller.Url = mUrlHelper.Object;

            this.controller.Request = new HttpRequestMessage();
            this.controller.Request.RequestUri = new Uri("http://localhost");


        }




    }
}
