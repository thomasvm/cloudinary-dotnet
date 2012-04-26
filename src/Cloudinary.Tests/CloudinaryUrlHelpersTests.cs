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
    }
}
