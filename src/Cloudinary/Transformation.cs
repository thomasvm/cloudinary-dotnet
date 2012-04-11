﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudinary
{
    public class Transformation
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public CropMode? Crop { get; set; }

        public Transformation(int width, int height)
        {
            Width = width;
            Height = height;
            Crop = null;
        }

        public string ToCloudinary()
        {
            var cli = new StringBuilder();
            cli.AppendFormat("w_{0},h_{1}", Width, Height);

            if (Crop.HasValue)
                cli.AppendFormat(",c_{0}", Crop.Value.ToString().ToLowerInvariant());

            return cli.ToString();
        }
    }
}
