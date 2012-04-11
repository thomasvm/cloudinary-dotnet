using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudinary.Results
{
    public class UploadResult
    {
        public string Error { get; set; }

        public string public_id { get; set; }

        public string PublicId
        {
            get { return public_id; }
        }

        public string Version { get; set; }

        public string Url { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }
    }
}
