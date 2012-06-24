using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Cloudinary.Tests
{
    [TestFixture]
    public class ChainedTransformationTests
    {
        [Test]
        public void ToCloudinary_WithTwoTransformations_CombinesThem()
        {
            var first = new Transformation(20, 40) { Angle = new Angle(40)};
            var second = new TransformationBase
                             {
                                 Effect = "sepia"
                             };

            var chained = new ChainedTransformation(first, second);

            Assert.AreEqual("w_20,h_40,a_40/e_sepia", chained.ToCloudinary());
        }

        [Test]
        public void GetFormat_WithTwoTransformations_ReturnsFirstFormat()
        {
            var first = new TransformationBase()
                            {
                                Format = "gif"
                            };

            var second = new Transformation(20, 30)
                             {
                                 Format = "png"
                             };

            var chained = new ChainedTransformation(first, second);

            Assert.That(chained.GetFormat() == "gif");
        }
    }
}
