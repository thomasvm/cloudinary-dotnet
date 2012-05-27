using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Cloudinary
{
    /// <summary>
    /// The default Transformation
    /// </summary>
    public class Transformation : ITransformation
    {
        /// <summary>
        /// Gets of sets the Width of the image
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the Height of the image
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets of sets Crop mode
        /// </summary>
        public CropMode? Crop { get; set; }

        /// <summary>
        /// Gets of sets Gravity of the tranformation
        /// </summary>
        public Gravity? Gravity { get; set; }

        /// <summary>
        /// Gets or sets the Format of the image
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets the border radius, int.MaxValue
        /// equals a circle
        /// </summary>
        public int? Radius { get; set; }

        public Transformation(int width, int height)
        {
            Width = width;
            Height = height;
            Crop = null;
        }

        public string GetFormat()
        {
            if (string.IsNullOrEmpty(Format))
                return "jpg";

            return Format;
        }

        public string ToCloudinary()
        {
            var cli = new StringBuilder();
            cli.AppendFormat(GetSize());

            if (Crop.HasValue)
                cli.AppendFormat(",c_{0}", Crop.Value.ToString().ToLowerInvariant());

            if (Gravity.HasValue)
                cli.AppendFormat(",g_{0}", Gravity.Value.ToString().ToLowerInvariant());

            if (Radius.HasValue)
            {
                string urlValue = Radius.Value.ToString(CultureInfo.InvariantCulture);

                if (Radius.Value == int.MaxValue)
                    urlValue = "max";
                    
                cli.AppendFormat(",r_{0}", urlValue);
            }

            return cli.ToString();
        }

        protected virtual string GetSize()
        {
            return string.Format("w_{0},h_{1}", Width, Height);
        }
    }
}
