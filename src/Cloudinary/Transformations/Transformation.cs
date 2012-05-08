using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Cloudinary
{
    public class Transformation : ITransformation
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public CropMode? Crop { get; set; }

        public Gravity? Gravity { get; set; }

        public string Format { get; set; }

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
