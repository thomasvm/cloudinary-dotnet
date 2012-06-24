using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cloudinary.Transformations;
using NUnit.Framework;

namespace Cloudinary.Tests
{
    [TestFixture]
    public class TransformationExtensionsTests
    {
        [Test]
        public void Chain_WithTwoNormalTransformations_CreatesChainedTransformation()
        {
            var first = new TransformationBase();
            var second = new TransformationBase();

            var chained = first.Chain(second);

            Assert.That(chained is ChainedTransformation);
        }

        [Test]
        public void Chain_OnChainedTransformation_ReUsesExisting()
        {
            var first = new TransformationBase();
            var second = new TransformationBase();

            ChainedTransformation chained = new ChainedTransformation(first, second);

            var third = new TransformationBase();

            var combined = chained.Chain(third);

            Assert.That(combined == chained);
            Assert.That(chained.InnerTransformations.Count() == 3);
        }

        [Test]
        public void Chain_WithSeconArgumentChained_ReUsesExistsing()
        {
            var first = new TransformationBase();
            var second = new TransformationBase();

            var chained = new ChainedTransformation(first, second);

            var third = new TransformationBase();

            var combined = third.Chain(chained);

            Assert.That(combined == chained);
        }
    }
}
