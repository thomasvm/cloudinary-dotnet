using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudinary
{
    public class AccountConfiguration
    {
        public string CloudName { get; private set; }

        public string ApiKey { get; private set; }

        public string ApiSecret { get; private set; }

        public bool SharedCdn { get; set; }

        public AccountConfiguration(string cloudName, string apiKey, string apiSecret)
        {
            CloudName = cloudName;
            ApiKey = apiKey;
            ApiSecret = apiSecret;

            SharedCdn = true;
        }

        private static AccountConfiguration _defaultConfiguration;

        /// <summary>
        /// Gets the default Account configuration, can be set through
        /// the Initialize method
        /// </summary>
        public static AccountConfiguration DefaultConfiguration
        {
            get { return _defaultConfiguration; }
        }

        /// <summary>
        /// Sets the default configuration that can be used from anywhere
        /// in the application. This will also be the configuration used 
        /// by the HtmlHelper and UrlHelper extensions
        /// </summary>
        /// <param name="defaultConfiguration"></param>
        public static void Initialize(AccountConfiguration defaultConfiguration)
        {
            _defaultConfiguration = defaultConfiguration;
        }
    }
}
