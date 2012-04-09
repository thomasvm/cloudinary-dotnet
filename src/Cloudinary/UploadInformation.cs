using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Cloudinary
{
    public class UploadInformation
    {
        public Stream InputStream { get; set; }

        public string PublicId { get; set; }

        public UploadInformation(Stream inputStream)
        {
            InputStream = inputStream;
        }
    }
}
