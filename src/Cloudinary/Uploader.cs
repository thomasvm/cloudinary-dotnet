using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Cloudinary
{
    public class Uploader
    {
        private const string BOUNDARY = "SoMeTeXtWeWiLlNeVeRsEe";
        private const string LINE = "\r\n";

        public AccountConfiguration Configuration { get; private set; }

        public Uploader(AccountConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void Upload(UploadInformation upload)
        {
            var parameterList = new List<Parameter>();

            if(!string.IsNullOrEmpty(upload.PublicId))
                parameterList.Add(new Parameter("public_id", upload.PublicId));
            
            Parameter[] paramArray = Sign(parameterList.ToArray());
            string paramString = string.Empty;

            foreach (Parameter param in paramArray)
            {
                paramString += "--" + BOUNDARY + LINE;
                paramString += "Content-Disposition: form-data; name=\"" + param.Name + "\"" + LINE + LINE + param.Value + LINE;
            }

            paramString = paramString + "--" + BOUNDARY + LINE
                          + "Content-Disposition: form-data;  name=\"file\"; filename=\"" + upload.Filename + "\""+ LINE
                          + "Content-Type: application/octet-stream" + LINE
                          + LINE;

            string closingString = LINE + "--" + BOUNDARY + "--";

            // get bytes
            byte[] paramBytes = Encoding.Default.GetBytes(paramString);
            byte[] closingBytes = Encoding.Default.GetBytes(closingString);

            byte[] fileBytes = ReadFully(upload.InputStream);

            // compose
            byte[] finalBytes = paramBytes.Concat(fileBytes).Concat(closingBytes).ToArray();

            // create request
            string url = string.Format("http://api.cloudinary.com/v1_1/{0}/image/upload", Configuration.CloudName);

            var wc = new WebClient();
            wc.Headers.Set("Content-Type", "multipart/form-data; boundary=" + BOUNDARY);
            byte[] result = wc.UploadData(url, "POST", finalBytes);

            string answer = Encoding.Default.GetString(result);

            Console.WriteLine(answer);
        }

        public void Destroy(string publicId)
        {
            string url = string.Format("http://api.cloudinary.com/v1_1/{0}/image/destroy", Configuration.CloudName);

            var parameters = new List<Parameter>
                                 {
                                     new Parameter("public_id", publicId)
                                 };

            var finalParameters = Sign(parameters);
            string paramString = string.Empty;

            foreach (Parameter param in finalParameters)
            {
                paramString += "--" + BOUNDARY + LINE;
                paramString += "Content-Disposition: form-data; name=\"" + param.Name + "\"" + LINE + LINE + param.Value + LINE;
            }

            paramString += "--" + BOUNDARY + "--";
            byte[] paramBytes = Encoding.Default.GetBytes(paramString);

            var wc = new WebClient();
            wc.Headers.Set("Content-Type", "multipart/form-data; boundary=" + BOUNDARY);
            byte[] result = wc.UploadData(url, "POST", paramBytes);

            string answer = Encoding.Default.GetString(result);

            Console.WriteLine(answer);
        }

        private static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private static long UnixTimeInSeconds(DateTime now)
        {
            TimeSpan timeSinceEpoch = now - new DateTime(1970, 1, 1, 0, 0, 0);

            return Convert.ToInt64(timeSinceEpoch.TotalSeconds);
        }

        internal Parameter[] Sign(IEnumerable<Parameter> parameters)
        {
            var list = new List<Parameter>(parameters)
                           {
                               new Parameter("timestamp", UnixTimeInSeconds(DateTime.Now)),
                           };
            list.Sort();

            var combined = string.Join("&", list.Select(p => p.ToString()).ToArray());
            combined += Configuration.ApiSecret;
            
            byte[] sha1Result = SHA1.Create().ComputeHash(Encoding.ASCII.GetBytes(combined));

            StringBuilder signatureBuilder = new StringBuilder();
            
            foreach (byte b in sha1Result)
                signatureBuilder.Append(b.ToString("x2"));

            list.Add(new Parameter("signature", signatureBuilder.ToString()));
            list.Add(new Parameter("api_key", Configuration.ApiKey));
            
            return list.ToArray();
        }
    }
}
