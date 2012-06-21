using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Cloudinary.Tests
{
    [TestFixture]
    public class AngleTests
    {
        [Test]
        public void ValueString_OnValueAngle_ReturnsAngle()
        {
            Angle a = new Angle(10);

            Assert.That(a.Value == 10);
        }
        
        [Test]
        public void ValueString_OnValueAngle_ReturnsAngleString()
        {
            Angle a = new Angle(10);

            Assert.That(a.ValueString == "10");
        }
 
        [Test]
        public void ValueString_OnSpecialAngle_ReturnsAngleString()
        {
            Angle a = new Angle("auto_left");

            Assert.That(a.ValueString == "auto_left");
        }
        
        [Test]
        public void ValueString_OnSpecialAngle_ReturnsNullValue()
        {
            Angle a = new Angle("auto_left");

            Assert.That(a.Value == null);
        }

        [Test]
        public void ToCloudinary_OnValueAngle_ReturnsExpected()
        {
            Angle a = new Angle(25);

            Assert.That(a.ToCloudinaryString() == "a_25");
        }
        
        [Test]
        public void ToCloudinary_OnSpecialAngle_ReturnsExpected()
        {
            Angle a = new Angle("auto_right");

            Assert.That(a.ToCloudinaryString() == "a_auto_right");
        }
    }
}
