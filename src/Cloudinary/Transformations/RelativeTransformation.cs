using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Cloudinary
{
    /// <summary>
    /// Transformation with sizing information specified relatively 
    /// instead of the usual pixel-based size
    /// </summary>
    public class RelativeTransformation : Transformation
    {
        public double RelativeWidth { get; private set; }
        public double RelativeHeight { get; private set; }

        public RelativeTransformation(double width, double height) 
            : base(0, 0)
        {
            RelativeWidth = width;
            RelativeHeight = height;
        }

        protected override string GetSize()
        {
            return string.Format("w_{0},h_{1}", 
                                 RelativeWidth.ToString(CultureInfo.InvariantCulture),
                                 RelativeHeight.ToString(CultureInfo.InvariantCulture));
        }
    }
}
