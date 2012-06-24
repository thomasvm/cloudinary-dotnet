using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudinary
{
    public class ChainedTransformation : ITransformation
    {
        private readonly IList<ITransformation> _transformations =  new List<ITransformation>();

        public IEnumerable<ITransformation> InnerTransformations
        {
            get { return _transformations.AsEnumerable(); }
        }

        public ChainedTransformation(ITransformation first, ITransformation second)
        {
            _transformations.Add(first);
            _transformations.Add(second);
        }

        public void Add(ITransformation transformation)
        {
            _transformations.Add(transformation);
        }

        public string ToCloudinary()
        {
            return string.Join("/", _transformations.Select(t => t.ToCloudinary()).ToArray());
        }

        /// <summary>
        /// Returns the Format, for a Chained transformation this returns
        /// the format of the first transformation
        /// </summary>
        /// <returns></returns>
        public string GetFormat()
        {
            if (_transformations.Any())
                return _transformations.First().GetFormat();

            return "jpg";
        }
    }
}
