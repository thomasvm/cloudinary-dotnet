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
        /// <summary>
        /// Stream of the Image to upload
        /// </summary>
        public Stream InputStream { get; private set; }

        /// <summary>
        /// Filename of the image
        /// </summary>
        public string Filename { get; private set; }

        /// <summary>
        /// Public Id, if empty a new public id will be generated
        /// by Cloudinary
        /// </summary>
        public string PublicId { get; set; }

        /// <summary>
        /// Comma-separated list of tags to attach to file
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Transformation to apply directly onto uploaded image
        /// </summary>
        public Transformation Transformation { get; set; }

        /// <summary>
        /// Format to store the image in
        /// (jpg, png, gif, bmp, ico)
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Transformations to eagerly apply to the images
        /// at upload time. (Instead of request time)
        /// </summary>
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

        /// <summary>
        /// Gets the string to perform the default transformation
        /// </summary>
        /// <returns>a string containing the default transformation</returns>
        public string GetTransformationValue()
        {
            return Transformation != null
                       ? Transformation.ToCloudinary()
                       : string.Empty;
        }

        /// <summary>
        /// Combines the eager transformations with a pipe
        /// </summary>
        /// <returns>a string containing the eager transformations</returns>
        public string GetEagerTransformationValues()
        {
            if (Eager == null)
                return null;

            return string.Join("|", Eager.Select(e => e.ToCloudinary()).ToArray());
        }
    }
}
