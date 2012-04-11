using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Mono.Options;

namespace Cloudinary.CommandLine
{
    class Program
    {
        static void Main(string[] args)
        {
            string filename = "octocat.jpg";
            bool showHelp = false;

            var options = new OptionSet()
                              {
                                  { "filename=|f=", "File to upload",(v) => filename = v },
                                  { "help|h", "Show help",(v) => showHelp = true }
                              };
            options.Parse(args);

            if(showHelp)
            {
                options.WriteOptionDescriptions(Console.Out);
                return;
            }

            if (!File.Exists(filename))
            {
                Console.WriteLine("File does not exist. Exiting...");
                return;
            }

            var settings = ConfigurationManager.AppSettings;

            var configuration = new AccountConfiguration(settings["cloudinary.cloud"],
                                                         settings["cloudinary.apikey"],
                                                         settings["cloudinary.apisecret"]);

            var uploader = new Uploader(configuration);

            string publicId = Path.GetFileNameWithoutExtension(filename);

            using(var stream =new FileStream(filename, FileMode.Open))
            {
                var uploadResult = uploader.Upload(new UploadInformation(filename, stream)
                                    {
                                        PublicId = publicId,
                                        Format = "png",
                                        Transformation = new Transformation(120, 120)
                                                             {
                                                                 Crop = CropMode.Scale
                                                             },
                                        Eager = new[]
                                                    {
                                                        new Transformation(240, 240),
                                                        new Transformation(120, 360) { Crop = CropMode.Limit },
                                                    }
                                    });

                Console.WriteLine("Version: {0}, PublicId {1}", uploadResult.Version, uploadResult.PublicId);
                Console.WriteLine("Url: {0}", uploadResult.Url);
            }
            Console.WriteLine("Successfully uploaded file");

            uploader.Destroy(publicId);

            if(Debugger.IsAttached)
                Console.ReadLine();
        }
    }
}
