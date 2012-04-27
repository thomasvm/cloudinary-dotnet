using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Cloudinary.Mvc
{
    public static class CloudinaryUrlHelpers
    {
        public static MvcHtmlString CloudinaryImage(this UrlHelper url, string publicId)
        {
            return CloudinaryImage(url, AccountConfiguration.DefaultConfiguration, publicId);
        }
        
        public static MvcHtmlString CloudinaryImage(this UrlHelper url, string publicId, string format)
        {
            return CloudinaryImage(url, AccountConfiguration.DefaultConfiguration, publicId, format);
        }

        public static MvcHtmlString CloudinaryImage(this UrlHelper url, AccountConfiguration configuration, string publicId)
        {
            return CloudinaryImage(url, configuration, publicId, "jpg");
        }

        public static MvcHtmlString CloudinaryImage(this UrlHelper url, AccountConfiguration configuration, string publicId, string format)
        {
            string baseUrl = GetBaseUrl(configuration) + "/image/upload";

            return _(string.Format("{0}/{1}.{2}", baseUrl, publicId, format));
        }

        public static MvcHtmlString CloudinaryImage(this UrlHelper url, string publicId, ITransformation transformation)
        {
            return CloudinaryImage(url, AccountConfiguration.DefaultConfiguration, publicId, transformation);
        }

        public static MvcHtmlString CloudinaryImage(this UrlHelper url, AccountConfiguration configuration, string publicId, ITransformation transformation)
        {
            string baseUrl = GetBaseUrl(configuration) + "/image/upload";

            return _(string.Format("{0}/{1}/{2}.{3}", baseUrl, transformation.ToCloudinary(), publicId, transformation.GetFormat()));
        }

        private static MvcHtmlString _(string input)
        {
            return MvcHtmlString.Create(input);
        }

        private static string GetBaseUrl(AccountConfiguration accountConfiguration)
        {
            if (accountConfiguration.SharedCdn)
                return "http://res.cloudinary.com/" + accountConfiguration.CloudName;

            return string.Format("http://{0}-res.cloudinary.com/", accountConfiguration.CloudName);
        }
    }
}
