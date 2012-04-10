using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Cloudinary.Parameters
{
    internal class FileParameter : Parameter
    {
        public string Filename { get; set; }

        public Stream FileStream { get; set; }

        public FileParameter(string name, string filename, Stream fileStream) 
            : base(name)
        {
            Filename = filename;
            FileStream = fileStream;
        }

        public override void WriteTo(StreamWriter w)
        {
            w.WriteLine("--" + BOUNDARY);
            w.WriteLine("Content-Disposition: form-data;  name=\"" + Name + "\"; filename=\"" + Filename + "\"");
            w.WriteLine("Content-Type: application/octet-stream");
            w.WriteLine();

            byte[] imageBytes = ReadFully(FileStream);
            w.BaseStream.Write(imageBytes, 0, imageBytes.Length);

            w.WriteLine();
        }

        private static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
