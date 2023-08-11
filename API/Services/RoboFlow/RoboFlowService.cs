using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace API.Services.RoboFlow
{
    public class RoboFlowService : IRoboFlowService
    {
        private readonly IConfiguration _configuration;

        public RoboFlowService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string DetectImage(string imageName)
        {

            byte[] imageArray = System.IO.File.ReadAllBytes(imageName);
            string encoded = Convert.ToBase64String(imageArray);
            byte[] data = Encoding.ASCII.GetBytes(encoded);
            string urlDetect = _configuration.GetValue<string>("RoboFlow:detectUrl");
            string api_key = _configuration.GetValue<string>("RoboFlow:apiKey"); // Your API Key
            string model_endpoint = _configuration.GetValue<string>("RoboFlow:modelEndpoint"); ; // Set model endpoint modelEndpoint
            // Construct the URL
            string uploadURL =
                    urlDetect + model_endpoint + "?api_key=" + api_key
                + "&name=" + imageArray;

            // Service Request Config
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Configure Request
            WebRequest request = WebRequest.Create(uploadURL);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            // Write Data
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            // Get Response
            string responseContent = null;
            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader sr99 = new StreamReader(stream))
                    {
                        responseContent = sr99.ReadToEnd();
                    }
                }
            }

            Console.WriteLine(responseContent);
            return responseContent;
        }

        public string DetectURLImage(string imgURL)
        {
            string urlDetect = _configuration.GetValue<string>("RoboFlow:detectUrl");
            string api_key = _configuration.GetValue<string>("RoboFlow:apiKey"); // Your API Key
            string model_endpoint = _configuration.GetValue<string>("RoboFlow:modelEndpoint"); ; // Set model endpoint modelEndpoint

            // Construct the URL
            string uploadURL =
                    urlDetect + model_endpoint
                    + "?api_key=" + api_key
                    + "&image=" + HttpUtility.UrlEncode(imgURL);

            // Service Point Config
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Configure Http Request
            WebRequest request = WebRequest.Create(uploadURL);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = 0;

            // Get Response
            string responseContent = null;
            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader sr99 = new StreamReader(stream))
                    {
                        responseContent = sr99.ReadToEnd();
                    }
                }
            }

            return responseContent;
        }
    }
}
