using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Cloudinary
{
    /// <summary>
    /// 
    /// </summary>
    public class TransformationBase : ITransformation
    {
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

        /// <summary>
        /// Gets or sets the angle the image should be rotated
        /// </summary>
        public Angle Angle { get; set; }

        /// <summary>
        /// apply an effect to the image
        /// </summary>
        public string Effect { get; set; }

        /// <summary>
        /// Image to use when the current image isn't available. 
        /// Format to use: publicid.format, 
        /// for example: avatar.jpg
        /// </summary>
        public string DefaultImage { get; set; }

        /// <summary>
        /// Gets the X for the fixed cropping coordinate, must 
        /// be set to SetFixedCroppingPosition
        /// </summary>
        public uint? X { get; private set; }

        /// <summary>
        /// Gets the X for the fixed cropping coordinate,
        /// be set to SetFixedCroppingPosition
        /// </summary>
        public uint? Y { get; private set; }

        /// <summary>
        /// Sets the fixed cropping position, note that
        /// the x and y values are against the original image
        /// </summary>
        /// <param name="x">the x value</param>
        /// <param name="y">the y value</param>
        public void SetFixedCroppingPosition(uint x, uint y)
        {
            X = x;
            Y = y;
        }

        public string GetFormat()
        {
            if (string.IsNullOrEmpty(Format))
                return "jpg";

            return Format;
        }

        public virtual string ToCloudinary()
        {
            var cli = new StringBuilder();

            if (X.HasValue && Y.HasValue)
                cli.AppendFormat(",x_{0},y_{1}", X.Value, Y.Value);

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

            if (!string.IsNullOrEmpty(DefaultImage))
                cli.AppendFormat(",d_{0}", DefaultImage);

            if (Angle != null)
                cli.AppendFormat("," + Angle.ToCloudinaryString());

            if (!string.IsNullOrEmpty(Effect))
                cli.AppendFormat(",e_{0}", Effect);

            return cli.ToString().Trim(',');
        }
    }
}
