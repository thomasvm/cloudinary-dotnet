using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudinary
{
    /// <summary>
    /// Interface that any transformation needs to exposed
    /// if it wants to be used by the Url Helpers, or passed
    /// onto the uploader
    /// </summary>
    public interface ITransformation
    {
        /// <summary>
        /// Get the cloudinary jargon for manipulating the image
        /// </summary>
        /// <returns>a sring with the information</returns>
        string ToCloudinary();

        /// <summary>
        /// The format the image needs to be in
        /// </summary>
        /// <returns>Any of jpg, gif, png</returns>
        string GetFormat();
    }
}
