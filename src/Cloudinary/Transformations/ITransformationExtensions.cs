using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudinary.Transformations
{
    public static class ITransformationExtensions
    {
        public static ITransformation Chain(this ITransformation first, ITransformation second)
        {
            if(first == null)
                throw new ArgumentNullException("first");

            if(second == null)
                throw new ArgumentNullException("second");

            ChainedTransformation chained = first as ChainedTransformation;

            if(chained != null)
            {
                chained.Add(second);
                return chained;
            }

            return new ChainedTransformation(first, second);
        }
    }
}
