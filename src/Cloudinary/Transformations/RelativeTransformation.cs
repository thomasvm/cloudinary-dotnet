using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Cloudinary
{
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
                                 Width.ToString(CultureInfo.InvariantCulture),
                                 Height.ToString(CultureInfo.InvariantCulture));
        }
    }
}
