using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using sky.coll.Libs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace sky.coll.Controllers
{
    public class TokenController : Controller
    {
        private BaseController bc = new BaseController();
        private lDbConn dbconn = new lDbConn();
        private int timeout = 5;
        public JObject GetToken(string module)
        {
            JObject jOutput = new JObject();
            var WebAPIURL = dbconn.domainGetApi(module);
            string requestStr = WebAPIURL + "token";

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("username", dbconn.domainGetTokenCredential("UserName"));
            client.DefaultRequestHeaders.Add("password", dbconn.domainGetTokenCredential("Password"));
            var contentData = new StringContent("", System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");

            HttpResponseMessage response = client.PostAsync(requestStr, contentData).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            jOutput = JObject.Parse(result);
            return jOutput;
        }

        public JObject GetCustToken()
        {
            JObject jOutput = new JObject();
            var WebAPIURL = dbconn.domainGetApi("urlAPI_skycore");
            string requestStr = WebAPIURL + "token";

            //var client = new HttpClient();
            //client.DefaultRequestHeaders.Add("username", dbconn.domainGetTokenCredential("UserName"));
            //client.DefaultRequestHeaders.Add("password", dbconn.domainGetTokenCredential("Password"));

            var serviceProvider = new ServiceCollection().AddHttpClient()
           .Configure<HttpClientFactoryOptions>("HttpClientWithSSLUntrusted", options =>
               options.HttpMessageHandlerBuilderActions.Add(builder =>
                   builder.PrimaryHandler = new HttpClientHandler
                   {
                       ServerCertificateCustomValidationCallback = (m, crt, chn, e) => true
                   }))
           .BuildServiceProvider();
            var _httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            HttpClient client = _httpClientFactory.CreateClient("HttpClientWithSSLUntrusted");
            client.BaseAddress = new Uri(requestStr);
            client.Timeout = TimeSpan.FromMinutes(timeout);
            if (client.DefaultRequestHeaders.Contains("username"))
            {
                client.DefaultRequestHeaders.Remove("username");
                client.DefaultRequestHeaders.Add("username", dbconn.domainGetTokenCredential("UserName"));
            }
            else
            {
                client.DefaultRequestHeaders.Add("username", dbconn.domainGetTokenCredential("UserName"));
            }

            if (client.DefaultRequestHeaders.Contains("password"))
            {
                client.DefaultRequestHeaders.Remove("password");
                client.DefaultRequestHeaders.Add("password", dbconn.domainGetTokenCredential("Password"));
            }
            else
            {
                client.DefaultRequestHeaders.Add("password", dbconn.domainGetTokenCredential("Password"));
            }


            var contentData = new StringContent("", System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");

            HttpResponseMessage response = client.PostAsync(requestStr, contentData).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            jOutput = JObject.Parse(result);
            client.Dispose();
            return jOutput;
        }

    }
}
