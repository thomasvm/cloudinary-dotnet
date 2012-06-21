using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Cloudinary.Transformations;

namespace Cloudinary
{
    /// <summary>
    /// The default Transformation
    /// </summary>
    public class Transformation : TransformationBase
    {
        /// <summary>
        /// Gets of sets the Width of the image
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the Height of the image
        /// </summary>
        public int Height { get; set; }

        public Transformation(int width, int height)
        {
            Width = width;
            Height = height;
            Crop = null;
        }

        public override string ToCloudinary()
        {
            string size = GetSize();

            string baseTransform = base.ToCloudinary();

            if (string.IsNullOrEmpty(baseTransform))
                return size;

            return size + "," + baseTransform;
        }
       
        protected virtual string GetSize()
        {
            return string.Format("w_{0},h_{1}", Width, Height);
        }
    }
}
