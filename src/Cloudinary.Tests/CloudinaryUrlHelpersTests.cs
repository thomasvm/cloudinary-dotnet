using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Cloudinary.Mvc;
using NUnit.Framework;

namespace Cloudinary.Tests
{
    [TestFixture]
    public class CloudinaryUrlHelpersTests
    {
        private UrlHelper Url { get; set; }

        [TestFixtureSetUp]
        public void SetupConfiguration()
        {
            AccountConfiguration.Initialize(new AccountConfiguration("test", "an.api.key", "an.api.secret"));
            Url = new UrlHelper(new RequestContext());
        }

        [Test]
        public void CloudinaryImage_WithPublicIdOnly()
        {
            string url = Url.CloudinaryImage("public").ToString();

            Assert.AreEqual("http://res.cloudinary.com/test/image/upload/public.jpg", url);
        }
        
        [Test]
        public void CloudinaryImage_WithPublicIdAndFormat()
        {
            string url = Url.CloudinaryImage("public", "png").ToString();

            Assert.AreEqual("http://res.cloudinary.com/test/image/upload/public.png", url);
        }
        
        [Test]
        public void CloudinaryImage_WithSimpleTransformation()
        {
            string url = Url.CloudinaryImage("public", new Transformation(240, 120)).ToString();

            Assert.AreEqual("http://res.cloudinary.com/test/image/upload/w_240,h_120/public.jpg", url);
        }

        [Test]
        public void CloudinaryImage_WithRelativeTransformation_InsertsRelativeWidths()
        {
            string url = Url.CloudinaryImage("relative", new RelativeTransformation(0.4, 0.6)).ToString();

            Assert.AreEqual("http://res.cloudinary.com/test/image/upload/w_0.4,h_0.6/relative.jpg", url);
        }
        
        [Test]
        public void CloudinaryImag_WithFaceDetection()
        {
            var transformation = new Transformation(240, 240)
                                     {
                                         Gravity = Gravity.Face,
                                         Crop = CropMode.Crop
                                     };

            string url = Url.CloudinaryImage("face", transformation).ToString();

            Assert.AreEqual("http://res.cloudinary.com/test/image/upload/w_240,h_240,c_crop,g_face/face.jpg", url);
        }

        [Test]
        public void CloudinaryImage_WithDefaultImageSpecified_AddsDefaultImageToUrl()
        {
            var transformation = new Transformation(240, 240)
                                     {
                                         DefaultImage = "sample.jpg"
                                     };

            string url = Url.CloudinaryImage("face", transformation).ToString();

            Assert.AreEqual("http://res.cloudinary.com/test/image/upload/w_240,h_240,d_sample.jpg/face.jpg", url);
        }

        [Test]
        public void CloudinaryImage_WithFixedCroppingPositionSet_AddPosition()
        {
            var transformation = new Transformation(240, 240) { Crop = CropMode.Crop };
            transformation.SetFixedCroppingPosition(350, 510);

            string url = Url.CloudinaryImage("cropme", transformation).ToString();

            Assert.AreEqual("http://res.cloudinary.com/test/image/upload/w_240,h_240,x_350,y_510,c_crop/cropme.jpg", url);
        }

        [Test]
        public void CloudinaryImage_WithAngle_AddsPosition()
        {
            var transformation = new Transformation(240, 240) {Angle = new Angle(45)};

            string url = Url.CloudinaryImage("angled", transformation).ToString();

            Assert.AreEqual("http://res.cloudinary.com/test/image/upload/w_240,h_240,a_45/angled.jpg", url);
        }
        
        [Test]
        public void CloudinaryImage_WithAutoAngle_AddsPosition()
        {
            var transformation = new Transformation(240, 240) {Angle = Angle.Auto };

            string url = Url.CloudinaryImage("angled", transformation).ToString();

            Assert.AreEqual("http://res.cloudinary.com/test/image/upload/w_240,h_240,a_auto/angled.jpg", url);
        }
    }
}
