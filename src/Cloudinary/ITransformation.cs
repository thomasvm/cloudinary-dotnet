using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudinary
{
    public interface ITransformation
    {
        string ToCloudinary();

        string GetFormat();
    }
}
