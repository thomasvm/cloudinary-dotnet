using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudinary
{
    public class AccountConfiguration
    {
        public string CloudName { get; set; }

        public string ApiKey { get; set; }

        public string ApiSecret { get; set; }

        public AccountConfiguration(string cloudName, string apiKey, string apiSecret)
        {
            CloudName = cloudName;
            ApiKey = apiKey;
            ApiSecret = apiSecret;
        }
    }
}
