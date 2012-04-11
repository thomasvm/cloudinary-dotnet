using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Cloudinary
{
    public class UploadInformation
    {
        public Stream InputStream { get; private set; }

        public string Filename { get; private set; }

        public string PublicId { get; set; }

        public string Tags { get; set; }

        public Transformation Transformation { get; set; }

        public string Format { get; set; }

        public IEnumerable<Transformation> Eager { get; set; }

        public UploadInformation(string filename, Stream inputStream)
        {
            Filename = filename;
            InputStream = inputStream;
        }
    }
}
