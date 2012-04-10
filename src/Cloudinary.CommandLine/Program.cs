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

            using(var stream =new FileStream(filename, FileMode.Open))
            {
                uploader.Upload(new UploadInformation(filename, stream)
                                    {
                                        PublicId = filename
                                    });
            }
            Console.WriteLine("Successfully uploaded file");

            uploader.Destroy(filename);

            if(Debugger.IsAttached)
                Console.ReadLine();
        }
    }
}
