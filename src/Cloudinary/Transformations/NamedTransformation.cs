using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudinary
{
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
