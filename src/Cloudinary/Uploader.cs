using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;
using Cloudinary.Parameters;
using Cloudinary.Results;

namespace Cloudinary
{
    public class Uploader
    {
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
            
            Sign(parameterList);
            parameterList.Add(new FileParameter("file", upload.Filename, upload.InputStream));
            
            // create request
            string answer = ExecuteRequest("upload", parameterList);

            var serializer = new JavaScriptSerializer();
            var result = serializer.Deserialize<UploadResult>(answer);

            Console.WriteLine(answer);
        }

        public void Destroy(string publicId)
        {
            var parameters = new List<Parameter>
                                 {
                                     new Parameter("public_id", publicId)
                                 };
            Sign(parameters);

            string answer = ExecuteRequest("destroy", parameters);
            Console.WriteLine(answer);
        }

        internal string ExecuteRequest(string method, IEnumerable<Parameter> parameters)
        {
            string url = string.Format("http://api.cloudinary.com/v1_1/{0}/image/{1}", Configuration.CloudName, method);

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "multipart/form-data; boundary=" + Parameter.BOUNDARY;

            using (Stream requestStream = request.GetRequestStream())
            {
                using(var streamWriter = new StreamWriter(requestStream) { AutoFlush = true })
                {
                    foreach (Parameter p in parameters)
                        p.WriteTo(streamWriter);

                    streamWriter.Write("--" + Parameter.BOUNDARY + "--");
                }
            }

            WebResponse response = request.GetResponse();

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                string answer = reader.ReadToEnd();
                return answer;
            }
        }

        private static long UnixTimeInSeconds(DateTime now)
        {
            TimeSpan timeSinceEpoch = now - new DateTime(1970, 1, 1, 0, 0, 0);

            return Convert.ToInt64(timeSinceEpoch.TotalSeconds);
        }

        internal void Sign(List<Parameter> list)
        {
            list.Add(new Parameter("timestamp", UnixTimeInSeconds(DateTime.Now)));
            list.Sort();

            var combined = string.Join("&", list.Select(p => p.ToString()).ToArray());
            combined += Configuration.ApiSecret;
            
            byte[] sha1Result = SHA1.Create().ComputeHash(Encoding.ASCII.GetBytes(combined));

            StringBuilder signatureBuilder = new StringBuilder();
            
            foreach (byte b in sha1Result)
                signatureBuilder.Append(b.ToString("x2"));

            list.Add(new Parameter("signature", signatureBuilder.ToString()));
            list.Add(new Parameter("api_key", Configuration.ApiKey));
        }
    }
}
