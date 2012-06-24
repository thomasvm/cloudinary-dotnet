using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudinary.Transformations
{
    public static class TransformationExtensions
    {
        public static ITransformation Chain(this ITransformation first, ITransformation second)
        {
            if(first == null)
                throw new ArgumentNullException("first");

            if(second == null)
                throw new ArgumentNullException("second");

            // check if first is chained
            ChainedTransformation chained = first as ChainedTransformation;

            // no? try second argument
            if(chained == null)
            {
                chained = second as ChainedTransformation;
            }

            if(chained != null)
            {
                chained.Add(second);
                return chained;
            }

            return new ChainedTransformation(first, second);
        }
    }
}
