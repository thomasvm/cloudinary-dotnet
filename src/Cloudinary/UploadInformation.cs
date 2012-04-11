using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cloudinary.Parameters;

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

        internal IEnumerable<Parameter> GetParameters()
        {
            var parameterNames = new Dictionary<string, Func<string>>
                                     {
                                         { "public_id", () => PublicId },
                                         { "format", () => Format },
                                         { "transformation", () => GetTransformationValue() },
                                         { "tags", () => Tags },
                                         { "eager", () => GetEagerTransformationValues() }
                                     };
            
            foreach(var keyValue in parameterNames)
            {
                string result = keyValue.Value();

                if (string.IsNullOrEmpty(result))
                    continue;

                yield return new Parameter(keyValue.Key, result);
            }
        }

        public string GetTransformationValue()
        {
            return Transformation != null
                       ? Transformation.ToCloudinary()
                       : string.Empty;
        }

        public string GetEagerTransformationValues()
        {
            if (Eager == null)
                return null;

            return string.Join("|", Eager.Select(e => e.ToCloudinary()).ToArray());
        }
    }
}
