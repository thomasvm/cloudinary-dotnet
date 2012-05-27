using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudinary
{
    /// <summary>
    /// Instead of specifying the size of the image, this transformation
    /// allows you to use a transformation specified in the admin 
    /// </summary>
    public class NamedTransformation : Transformation
    {
        public string Name { get; set; }

        public NamedTransformation(string name) 
            : base(0, 0)
        {
            Name = name;
        }

        protected override string GetSize()
        {
            return string.Format("t_{0}", Name);
        }
    }
}
