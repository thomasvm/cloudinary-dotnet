using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Cloudinary
{
    public class Angle
    {
        public static readonly Angle Auto = new Angle("auto");
        public static readonly Angle AutoLeft = new Angle("auto_left");
        public static readonly Angle AutoRight = new Angle("auto_right");

        public int? Value { get; private set; }

        public string ValueString { get; private set; }
        
        public Angle(int angle)
        {
            Value = angle;
            ValueString = angle.ToString(CultureInfo.InvariantCulture);
        }

        public Angle(string specialValue)
        {
            ValueString = specialValue;
        }

        public string ToCloudinaryString()
        {
            return "a_" + ValueString;
        }
    }

}
