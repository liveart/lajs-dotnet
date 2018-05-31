using System.Linq;
using FluentAssertions;
using LiveArt.Design.PostProcessing.Packer;
using Moq;
using NUnit.Framework;

namespace LiveArt.Design.PostProcessing.Tests
{
    [TestFixture]
    public class DesignPackerTests
    {

        private DesignPacker target;//class under test
        

        internal Mock<PostProcessing.IWorkingFolder> mFolder;
        private PackingContext convertContext;

        private Design.PostProcessing.Domain.Design CurrentDesign{
            get { return ((DesignPacker)target).PackContext.CurrentDesign; }
        }

        [SetUp]
        public void SetUp()
        {
            this.target = new DesignPacker();
            this.mFolder = new Mock<PostProcessing.IWorkingFolder>();
            convertContext = new PackingContext()
            {
                WorkingFolder = mFolder.Object
            };
        }

        

     




        [Test]
        public void PackTo_LocationName_ShouldCopy_FromOrig()
        {

            var pack = target.FromDesign(TestData.DesignWithFrontLocation).PackTo("some fodller"); //action
           //asserts
           pack.Locations.Should().HaveCount(1);
           var location=pack.Locations.First();
            location.Name.Should()
                .Be("Front");

        }


        [Test]
        public void PackTo_ShouldEnsureForkingWolderExists()
        {
            //action
            var pack = target.FromDesign(TestData.DesignWithFrontLocation).PackTo(mFolder.Object); //action

            //asserts
            mFolder.Verify(m => m.EnsureExists());

        }

        [Test]
        public void Static_FromDesignJson_ShouldGeneratePacker()
        {
            DesignPacker.FromJsonStr("{}")
                .Should().NotBeNull();

        }


        [Test]
        public void PackTo_ShouldPass_LocationResultsInContext()
        {
            DesignPack.Location resultLocation=null;
            this.target.RegBeforePackAction((context) =>
            {
                resultLocation = context.Result.Locations.FirstOrDefault();
            });

            //arrange
            target.FromDesign(TestData.DesignWithFrontLocation)
                .PackTo(mFolder.Object);

            //asserts
            resultLocation.Should().NotBeNull();


        }


        #region Logging
        

        [Test]
        public void PackTo_Should_BeginEndOfLogging()
        {
            //action
            target.FromDesign(TestData.DesignWithFrontLocation).PackTo(@"c:\somefolder") //action

            //asserts
            .LogLines.Messages()
                .Should().Contain(new[]{
                    @"Start 'PackTo' for 'c:\somefolder'",
                    @"Finish 'PackTo' for 'c:\somefolder'",
                });
        }
            


        
        #endregion //Logging



    }



}
