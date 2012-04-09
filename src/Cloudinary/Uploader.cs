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
                          + "Content-Disposition: form-data; " + LINE
                          // + "Content-Type: image/jpeg" + LINE 
                          + LINE;

            string closing_string = LINE + "--" + BOUNDARY + "--";

            // get bytes
            byte[] paramBytes = Encoding.Default.GetBytes(paramString);
            byte[] closingBytes = Encoding.Default.GetBytes(closing_string);

            byte[] fileBytes = ReadFully(upload.InputStream);

            // compose
            List<byte> byteList = new List<byte>(paramBytes.Length + fileBytes.Length + closingBytes.Length);
            byteList.AddRange(paramBytes);
            byteList.AddRange(fileBytes);
            byteList.AddRange(closingBytes);

            byte[] finalBytes = byteList.ToArray();

            // create request
            WebClient wc = new WebClient();
            wc.Headers.Set("Content-Type", "multipart/form-data; boundary=" + BOUNDARY);
            wc.Headers.Add("MIME-version", "1.0");

            // upload
            string url = string.Format("http://api.cloudinary.com/v1_1/{0}/image/upload", Configuration.CloudName);
            byte[] response = wc.UploadData(url, "POST", finalBytes);

            string result = Encoding.Default.GetString(response);

            Console.WriteLine(result);
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

        internal Parameter[] Sign(params Parameter[] parameters)
        {
            var list = new List<Parameter>(parameters)
                           {
                               new Parameter("timestamp", UnixTimeInSeconds(DateTime.UtcNow)),
                               new Parameter("api_key", Configuration.ApiKey)
                           };
            list.Sort();

            StringBuilder values = new StringBuilder();

            foreach (Parameter param in list)
                values.Append(param.ToString());

            values.Append(Configuration.ApiSecret);

            byte[] sha1Result = SHA1.Create().ComputeHash(Encoding.ASCII.GetBytes(values.ToString()));

            StringBuilder signatureBuilder = new StringBuilder();

            foreach (byte b in sha1Result)
                signatureBuilder.Append(b.ToString("x2"));

            list.Add(new Parameter("signature", signatureBuilder.ToString()));

            return list.ToArray();
        }
    }
}
