using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;

namespace GETAPIContent
{
    class Json
    {
        protected static string JsonUrl;
        protected static JObject json = null;
        protected HttpStatusCode _statusCode;
 
        public Json(string url)
        {
            JsonUrl = url;
        }

        public JObject RequestContent()
        {
            var request = WebRequest.Create(JsonUrl);
            request.Method = WebRequestMethods.Http.Get;
            request.ContentType = "application/json; charset=utf-8";
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                _statusCode = response.StatusCode;
                var temp = (int)_statusCode;
                switch (temp)
                {
                    case 200:
                        using (var result = new StreamReader(response.GetResponseStream()))
                            json = JObject.Parse(result.ReadToEnd());
                        break;
                    default:
                        break;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
           
            return json;
        }
    }

}
