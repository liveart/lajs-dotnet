using System;
using System.Linq;
using FluentAssertions;
using LiveArt.Design.PostProcessing.Packer;
using Moq;
using NUnit.Framework;

namespace LiveArt.Design.PostProcessing.Tests.PackerExtentions
{
    public abstract class BaseExtentionsTests
    {
        internal Mock<IDesignPackerWithActions> mPacker;
        protected IDesignPacker _Packer { get { return mPacker.Object; } }

        internal PackingContext context;


        internal Action<PackingContext> targetAction; // register action, code under test

      

        public virtual void SetUp(){
            var defaultDesign=TestData.DesignWithFrontLocation;
            context = new PackingContext()
            {
                CurrentDesign = defaultDesign,
                Result=new DesignPack(){
                    Locations=defaultDesign.locations.Select(l=>new DesignPack.Location(){Name=l.name}).ToArray()
                }

            };

            targetAction = null;
            this.mPacker = new Mock<IDesignPackerWithActions>();
            this.mPacker.Setup(m => m.RegBeforePackAction(It.IsAny<Action<PackingContext>>()))
                                .Callback<Action<PackingContext>>(action => this.targetAction = action);
            ;
        }

        protected abstract void InvokeExtentionMethod();


        [Test]
        public void InvokeExtention_ActionRegisterdInPacker()
        {

            //action
            this.InvokeExtentionMethod();
            this.targetAction.Should().NotBeNull();
        }

     

    }
}
