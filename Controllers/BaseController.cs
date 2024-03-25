using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Data;
using System.Dynamic;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Globalization;
using sky.coll.Libs;
using Npgsql;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using System.Text.Json;
using System.Net.Http.Headers;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace sky.coll.Controllers
{
    public class BaseController : Controller
    {
        private lDbConn dbconn = new lDbConn();
        private MessageController mc = new MessageController();
        private int timeout = 5;
        public string execExtAPIPost(string api, string path, string json)
        {
            var WebAPIURL = dbconn.domainGetApi(api);
            string requestStr = WebAPIURL + path;

            var client = new HttpClient();
            var contentData = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(requestStr, contentData).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            return result;
        }

        public string ExecPostAPI(string url, string allText, string Signature, string Timestamp)
        {

            var WebAPIURL = dbconn.domainGetApi(url);
            var api_key = dbconn.domainGetTokenCredential("API_Key");
            //var Timestamp = dbconn.domainGetTokenCredential("Timestamp");
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
            client.BaseAddress = new Uri(WebAPIURL);
            client.Timeout = TimeSpan.FromMinutes(timeout);
            client.DefaultRequestHeaders.Add("API-Key", api_key);
            client.DefaultRequestHeaders.Add("Timestamp", Timestamp);
            client.DefaultRequestHeaders.Add("Signature", Signature);

            var contentData = new StringContent(allText, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(WebAPIURL, contentData).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            client.Dispose();
            return result;
        }

        public string ExecPostAPILoginBrikerbox(string WebAPIURL, JObject req)
        {

          
            var usersid = dbconn.domainGetCredential("usersid");
            var passwd = dbconn.domainGetCredential("password");
            //var Timestamp = dbconn.domainGetTokenCredential("Timestamp");
            //var req = JObject.Parse(allText.ToString());

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("adm", usersid);
            client.DefaultRequestHeaders.Add("pass", passwd);
            var contentData = new StringContentWithoutCharset(req.ToString(), "application/json");

            HttpResponseMessage response = client.PostAsync(WebAPIURL, contentData).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            return result;
        }

        public bool LoginAgent(string ticket, string agent, string code, string WebAPIURL)
        {

            var jreq = new JObject();

            jreq.Add("cmd", "agentcc");
            jreq.Add("event", "login");
            jreq.Add("queue", "1000");
            jreq.Add("agent", agent);
            jreq.Add("device", code);

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("ticket", ticket);
            var contentData = new StringContentWithoutCharset(jreq.ToString(), "application/json");

            HttpResponseMessage response = client.PostAsync(WebAPIURL, contentData).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            if (result != "")
            {
               
                return true;
            }
            else
            {
                return false;
            }
            

        }

        public bool DoCall(string ticket, string nohp, string agent, string code, string WebAPIURL)
        {

            var jreq = new JObject();

            jreq.Add("cmd", "ctc");
            jreq.Add("dialto", nohp);
            jreq.Add("timeout", "30");
            jreq.Add("cid", agent);
            jreq.Add("v", nohp + "#" + code);


            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("ticket", ticket);
            var contentData = new StringContentWithoutCharset(jreq.ToString(), "application/json");

            HttpResponseMessage response = client.PostAsync(WebAPIURL, contentData).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            if (result != "")
            {

                return true;
            }
            else
            {
                return false;
            }


        }

        public bool LogoutAgent(string ticket, string agent, string code, string WebAPIURL)
        {

            var jreq = new JObject();

            jreq.Add("cmd", "agentcc");
            jreq.Add("event", "logoff");
            jreq.Add("queue", "1000");
            jreq.Add("agent", agent);
            jreq.Add("device", code);


            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("ticket", ticket);
            var contentData = new StringContentWithoutCharset(jreq.ToString(), "application/json");

            HttpResponseMessage response = client.PostAsync(WebAPIURL, contentData).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            if (result != "")
            {

                return true;
            }
            else
            {
                return false;
            }


        }

        public class StringContentWithoutCharset : StringContent
        {
            public StringContentWithoutCharset(string content) : base(content)
            {
            }

            public StringContentWithoutCharset(string content, Encoding encoding) : base(content, encoding)
            {
                Headers.ContentType.CharSet = "";
            }

            public StringContentWithoutCharset(string content, Encoding encoding, string mediaType) : base(content, encoding, mediaType)
            {
                Headers.ContentType.CharSet = "";
            }

            public StringContentWithoutCharset(string content, string mediaType) : base(content, Encoding.UTF8, mediaType)
            {
                Headers.ContentType.CharSet = "";
            }
        }
        public async Task<string> execExtAPIPostWithTokenAsync(string api, string path, string json, string credential)
        {
            var WebAPIURL = dbconn.domainGetApi(api);
            string requestStr = WebAPIURL + path;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", credential);
            var contentData = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            //contentData.Headers.Add("Authorization", credential);   

            HttpResponseMessage response = client.PostAsync(requestStr, contentData).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            return result;
        }

        public string execExtAPIGetWithToken(string api, string path, string json, string credential)
        {
            var WebAPIURL = dbconn.domainGetApi(api);
            string requestStr = WebAPIURL + path;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", credential);
            HttpResponseMessage response = client.GetAsync(requestStr).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            return result;
        }

        public string postConnection(string spname, params string[] list)
        {
            //var retObject = new List<dynamic>();
            string retObject = "";
            var parameter = spname;
            if (list != null && list.Count() > 0)
            {
                for (int i = 0; i < list.Count(); i++)
                {
                    parameter += ";" + list[i];
                }
            }
            retObject = postDataFromApi(parameter);
            return retObject;
        }

        public string postDataFromApi(string parameter)
        {
            var conn = dbconn.domainPostApi();
            WebRequest request = WebRequest.Create(conn + parameter);
            WebResponse response = request.GetResponseAsync().Result;
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();

            return responseFromServer;
        }

        public void execSqlWithExecption(string spname, params string[] list)
        {
            //var conn = dbconn.conString();
            var conn = dbconn.conString_v2();
            string message = "";
            NpgsqlConnection nconn = new NpgsqlConnection(conn);
            nconn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand(spname, nconn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (list != null && list.Count() > 0)
            {
                foreach (var item in list)
                {
                    var pars = item.Split(',');

                    if (pars.Count() > 2)
                    {
                        if (pars[2] == "i")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToInt32(pars[1]));
                        }
                        else if (pars[2] == "s")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToString(pars[1]));
                        }
                        else if (pars[2] == "d")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToDecimal(pars[1]));
                        }
                        else if (pars[2] == "b")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToBoolean(pars[1]));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(pars[0], pars[1]);
                        }
                    }
                    else if (pars.Count() > 1)
                    {
                        cmd.Parameters.AddWithValue(pars[0], pars[1]);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue(pars[0], pars[0]);
                    }
                }
            }
            try
            {
                cmd.ExecuteNonQuery();
                message = "success";
            }
            catch (NpgsqlException e)
            {
                message = e.Message;
            }
            finally
            {
                nconn.Close();
            }
            //return message;
        }

        public List<dynamic> getDataToObject(string dbprv, string strname, string spname, params string[] list)
        {
            //var conn = dbconn.conString();
            var conn = dbconn.constringList_v2(dbprv, strname);
            StringBuilder sb = new StringBuilder();
            NpgsqlConnection nconn = new NpgsqlConnection(conn);
            var retObject = new List<dynamic>();

            nconn.Open();
            //NpgsqlTransaction tran = nconn.BeginTransaction();
            NpgsqlCommand cmd = new NpgsqlCommand(spname, nconn);
            cmd.CommandType = CommandType.StoredProcedure;

            if (list != null && list.Count() > 0)
            {
                foreach (var item in list)
                {
                    var pars = item.Split(',');

                    if (pars.Count() > 2)
                    {
                        if (pars[2] == "i")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToInt32(pars[1]));
                        }
                        else if (pars[2] == "bg")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToInt64(pars[1]));
                        }
                        else if (pars[2] == "s")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToString(pars[1]));
                        }
                        else if (pars[2] == "d")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToDecimal(pars[1]));
                        }
                        else if (pars[2] == "dt")
                        {
                            cmd.Parameters.AddWithValue(pars[0], DateTime.ParseExact(pars[1], "yyyy-MM-dd", CultureInfo.InvariantCulture));
                        }
                        else if (pars[2] == "b")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToBoolean(pars[1]));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(pars[0], pars[1]);
                        }
                    }
                    else if (pars.Count() > 1)
                    {
                        cmd.Parameters.AddWithValue(pars[0], pars[1]);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue(pars[0], pars[0]);
                    }
                }
            }

            NpgsqlDataReader dr = cmd.ExecuteReader();

            if (dr == null || dr.FieldCount == 0)
            {
                return retObject;
            }

            retObject = GetDataObj(dr);

            nconn.Close();
            return retObject;
        }

        public List<dynamic> getDataToObjectSemicolon(string dbprv, string strname, string spname, params string[] list)
        {
            var conn = dbconn.constringList_v2(dbprv, strname);
            StringBuilder sb = new StringBuilder();
            NpgsqlConnection nconn = new NpgsqlConnection(conn);
            var retObject = new List<dynamic>();

            nconn.Open();
            //NpgsqlTransaction tran = nconn.BeginTransaction();
            NpgsqlCommand cmd = new NpgsqlCommand(spname, nconn);
            cmd.CommandType = CommandType.StoredProcedure;

            if (list != null && list.Count() > 0)
            {
                foreach (var item in list)
                {
                    var pars = item.Split(';');

                    if (pars.Count() > 2)
                    {
                        if (pars[2] == "i")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToInt32(pars[1]));
                        }
                        else if (pars[2] == "bg")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToInt64(pars[1]));
                        }
                        else if (pars[2] == "s")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToString(pars[1]));
                        }
                        else if (pars[2] == "d")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToDecimal(pars[1]));
                        }
                        else if (pars[2] == "b")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToBoolean(pars[1]));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(pars[0], pars[1]);
                        }
                    }
                    else if (pars.Count() > 1)
                    {
                        cmd.Parameters.AddWithValue(pars[0], pars[1]);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue(pars[0], pars[0]);
                    }
                }
            }

            NpgsqlDataReader dr = cmd.ExecuteReader();

            if (dr == null || dr.FieldCount == 0)
            {
                nconn.Close();
                NpgsqlConnection.ClearPool(nconn);
                return retObject;
            }

            retObject = GetDataObj(dr);

            nconn.Close();
            NpgsqlConnection.ClearPool(nconn);
            return retObject;
        }

        public string execSqlWithExecption(string dbprv, string strname, string spname, params string[] list)
        {

            var conn = dbconn.constringList_v2(dbprv, strname);
            string message = "";
            NpgsqlConnection nconn = new NpgsqlConnection(conn);
            nconn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand(spname, nconn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (list != null && list.Count() > 0)
            {
                foreach (var item in list)
                {
                    var pars = item.Split(',');

                    if (pars.Count() > 2)
                    {
                        if (pars[2] == "i")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToInt32(pars[1]));
                        }
                        else if (pars[2] == "bg")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToInt64(pars[1]));
                        }
                        else if (pars[2] == "s")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToString(pars[1]));
                        }
                        else if (pars[2] == "d")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToDecimal(pars[1]));
                        }
                        else if (pars[2] == "b")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToBoolean(pars[1]));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(pars[0], pars[1]);
                        }
                    }
                    else if (pars.Count() > 1)
                    {
                        cmd.Parameters.AddWithValue(pars[0], pars[1]);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue(pars[0], pars[0]);
                    }
                }
            }
            try
            {
                cmd.ExecuteNonQuery();
                message = "success";
            }
            catch (NpgsqlException e)
            {
                message = e.Message;
            }
            finally
            {
                if (nconn.State.Equals(ConnectionState.Open))
                {
                    nconn.Close();
                }
                NpgsqlConnection.ClearPool(nconn);
            }
            return message;
        }

        public void execSqlWithSplitPipeline(string dbprv, string strname, string spname, params string[] list)
        {
            var conn = dbconn.constringList_v2(dbprv, strname);
            string message = "";
            NpgsqlConnection nconn = new NpgsqlConnection(conn);
            nconn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand(spname, nconn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (list != null && list.Count() > 0)
            {
                foreach (var item in list)
                {
                    var pars = item.Split('|');

                    if (pars.Count() > 2)
                    {
                        if (pars[2] == "i")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToInt32(pars[1]));
                        }
                        else if (pars[2] == "bg")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToInt64(pars[1]));
                        }
                        else if (pars[2] == "s")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToString(pars[1]));
                        }
                        else if (pars[2] == "d")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToDecimal(pars[1]));
                        }
                        else if (pars[2] == "b")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToBoolean(pars[1]));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(pars[0], pars[1]);
                        }
                    }
                    else if (pars.Count() > 1)
                    {
                        cmd.Parameters.AddWithValue(pars[0], pars[1]);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue(pars[0], pars[0]);
                    }
                }
            }
            try
            {
                cmd.ExecuteNonQuery();
                message = "success";
            }
            catch (NpgsqlException e)
            {
                message = e.Message;
            }
            finally
            {
                if (nconn.State.Equals(ConnectionState.Open))
                {
                    nconn.Close();
                }
                NpgsqlConnection.ClearPool(nconn);
            }
        }

        public List<dynamic> getDynamicDataToObject(string dbprv, string strname, string spname, string parameter)
        {
            var conn = dbconn.constringList_v2(dbprv, strname);
            StringBuilder sb = new StringBuilder();
            NpgsqlConnection nconn = new NpgsqlConnection(conn);
            NpgsqlCommand cmd = new NpgsqlCommand(spname, nconn);
            cmd.CommandType = CommandType.StoredProcedure;
            var retObject = new List<dynamic>();

            try
            {
                nconn.Open();
                var data = parameter.Split('|');
                if (data.Count() > 1)
                {
                    for (int i = 0; i < data.Count() - 1; i++)
                    {
                        var pars = data[i].Split(',');
                        if (pars[2] == "i")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToInt32(pars[1]));
                        }
                        else if (pars[2] == "s")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToString(pars[1]));
                        }
                        else if (pars[2] == "d")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToDecimal(pars[1]));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(pars[0], pars[1]);
                        }
                    }
                }

                NpgsqlDataReader dr = cmd.ExecuteReader();

                if (dr == null || dr.FieldCount == 0)
                {
                    if (nconn.State.Equals(ConnectionState.Open))
                    {
                        nconn.Close();
                    }
                    NpgsqlConnection.ClearPool(nconn);

                    return retObject;
                }

                retObject = GetDataObj(dr);
                if (nconn.State.Equals(ConnectionState.Open))
                {
                    nconn.Close();
                }
                NpgsqlConnection.ClearPool(nconn);
                return retObject;
            }
            catch (Exception ex)
            {
                dynamic DyObj = new ExpandoObject();
                DyObj.success = false;
                DyObj.message = ex.Message;
                retObject.Add(DyObj);

                if (nconn.State.Equals(ConnectionState.Open))
                {
                    nconn.Close();
                }
                NpgsqlConnection.ClearPool(nconn);
                return retObject;
            }

        }

        public List<dynamic> getDataToObjectCustomSplit(string dbprv, string strname, string split, string spname, params string[] list)
        {
            var conn = dbconn.constringList_v2(dbprv, strname);
            StringBuilder sb = new StringBuilder();
            NpgsqlConnection nconn = new NpgsqlConnection(conn);
            var retObject = new List<dynamic>();

            nconn.Open();
            //NpgsqlTransaction tran = nconn.BeginTransaction();
            NpgsqlCommand cmd = new NpgsqlCommand(spname, nconn);
            cmd.CommandType = CommandType.StoredProcedure;

            if (list != null && list.Count() > 0)
            {
                foreach (var item in list)
                {
                    var pars = item.Split(split);

                    if (pars.Count() > 2)
                    {
                        if (pars[2] == "i")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToInt32(pars[1]));
                        }
                        else if (pars[2] == "s")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToString(pars[1]));
                        }
                        else if (pars[2] == "d")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToDecimal(pars[1]));
                        }
                        else if (pars[2] == "b")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToBoolean(pars[1]));
                        }
                        else if (pars[2] == "bg")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToInt64(pars[1]));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(pars[0], pars[1]);
                        }
                    }
                    else if (pars.Count() > 1)
                    {
                        cmd.Parameters.AddWithValue(pars[0], pars[1]);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue(pars[0], pars[0]);
                    }
                }
            }
            try
            {
                NpgsqlDataReader dr = cmd.ExecuteReader();
                if (dr == null || dr.FieldCount == 0)
                {
                    if (nconn.State.Equals(ConnectionState.Open))
                    {
                        nconn.Close();
                    }
                    NpgsqlConnection.ClearPool(nconn);
                    return retObject;
                }

                retObject = GetDataObj(dr);

                if (nconn.State.Equals(ConnectionState.Open))
                {
                    nconn.Close();
                }
                NpgsqlConnection.ClearPool(nconn);
            }
            catch (Exception ex)
            {
                if (nconn.State.Equals(ConnectionState.Open))
                {
                    nconn.Close();
                }
                NpgsqlConnection.ClearPool(nconn);

                retObject = new List<dynamic>();
                dynamic row = new ExpandoObject();
                row.status = "Invalid";
                row.message = "Invalid (" + ex.Message + ").";
                retObject.Add((ExpandoObject)row);
            }
            return retObject;
        }

        public List<dynamic> getDynamicDataToObjectv2(string dbprv, string strname, string spname, string parameter)
        {
            var conn = dbconn.constringList_v2(dbprv, strname);
            StringBuilder sb = new StringBuilder();
            NpgsqlConnection nconn = new NpgsqlConnection(conn);
            NpgsqlCommand cmd = new NpgsqlCommand(spname, nconn);
            cmd.CommandType = CommandType.StoredProcedure;
            var retObject = new List<dynamic>();

            try
            {
                nconn.Open();
                var data = parameter.Split('^');
                if (data.Count() > 1)
                {
                    for (int i = 0; i < data.Count() - 1; i++)
                    {
                        var pars = data[i].Split(',');
                        if (pars[2] == "i")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToInt32(pars[1]));
                        }
                        else if (pars[2] == "s")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToString(pars[1]));
                        }
                        else if (pars[2] == "d")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToDecimal(pars[1]));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(pars[0], pars[1]);
                        }
                    }
                }

                NpgsqlDataReader dr = cmd.ExecuteReader();

                if (dr == null || dr.FieldCount == 0)
                {
                    if (nconn.State.Equals(ConnectionState.Open))
                    {
                        nconn.Close();
                    }
                    NpgsqlConnection.ClearPool(nconn);

                    return retObject;
                }

                retObject = GetDataObj(dr);

                if (nconn.State.Equals(ConnectionState.Open))
                {
                    nconn.Close();
                }
                NpgsqlConnection.ClearPool(nconn);
                return retObject;
            }
            catch (Exception ex)
            {
                dynamic DyObj = new ExpandoObject();
                DyObj.success = false;
                DyObj.message = ex.Message;
                retObject.Add(DyObj);

                if (nconn.State.Equals(ConnectionState.Open))
                {
                    nconn.Close();
                }
                NpgsqlConnection.ClearPool(nconn);
                return retObject;
            }

        }

        public List<dynamic> GetDataObj(NpgsqlDataReader dr)
        {
            var retObject = new List<dynamic>();
            while (dr.Read())
            {
                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    dataRow.Add(
                           dr.GetName(i),
                           dr.IsDBNull(i) ? null : dr[i] // use null instead of {}
                   );
                }
                retObject.Add((ExpandoObject)dataRow);
            }

            return retObject;
        }

        public List<dynamic> GetDataObjPgsql(NpgsqlDataReader dr)
        {
            var retObject = new List<dynamic>();
            while (dr.Read())
            {
                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    dataRow.Add(
                           dr.GetName(i),
                           dr.IsDBNull(i) ? null : dr[i] // use null instead of {}
                   );
                }
                retObject.Add((ExpandoObject)dataRow);
            }

            return retObject;
        }

        public List<dynamic> GetDataObjSqlsvr(SqlDataReader dr)
        {
            var retObject = new List<dynamic>();
            while (dr.Read())
            {
                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    dataRow.Add(
                           dr.GetName(i),
                           dr.IsDBNull(i) ? null : dr[i] // use null instead of {}
                   );
                }
                retObject.Add((ExpandoObject)dataRow);
            }

            return retObject;
        }

        public void execSqlWithSplitSemicolon(string dbprv, string strname, string spname, params string[] list)
        {
            var conn = dbconn.constringList_v2(dbprv, strname);
            string message = "";
            NpgsqlConnection nconn = new NpgsqlConnection(conn);
            nconn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand(spname, nconn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (list != null && list.Count() > 0)
            {
                foreach (var item in list)
                {
                    var pars = item.Split(';');

                    if (pars.Count() > 2)
                    {
                        if (pars[2] == "i")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToInt32(pars[1]));
                        }
                        else if (pars[2] == "s")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToString(pars[1]));
                        }
                        else if (pars[2] == "d")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToDecimal(pars[1]));
                        }
                        else if (pars[2] == "b")
                        {
                            cmd.Parameters.AddWithValue(pars[0], Convert.ToBoolean(pars[1]));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(pars[0], pars[1]);
                        }
                    }
                    else if (pars.Count() > 1)
                    {
                        cmd.Parameters.AddWithValue(pars[0], pars[1]);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue(pars[0], pars[0]);
                    }
                }


            }
            try
            {
                cmd.ExecuteNonQuery();
                message = "success";
            }
            catch (NpgsqlException e)
            {
                message = e.Message;
            }
            finally
            {
                nconn.Close();
            }
            //return message;
        }

        public List<dynamic> ExecSqlWithReturnCustomSplit(string dbprv, string strname, string cstsplit, string schema, string spname, params string[] list)
        {
            var retObject = new List<dynamic>();
            StringBuilder sb = new StringBuilder();
            var conn = dbconn.constringList_v2(dbprv, strname);

            if (dbprv == "postgresql")
            {
                spname = schema + "." + spname;
                NpgsqlConnection nconn = new NpgsqlConnection(conn);
                nconn.Open();
                //NpgsqlTransaction tran = nconn.BeginTransaction();
                NpgsqlCommand cmd = new NpgsqlCommand(spname, nconn);
                cmd.CommandType = CommandType.StoredProcedure;

                if (list != null && list.Count() > 0)
                {
                    foreach (var item in list)
                    {
                        var pars = item.Split(cstsplit);

                        if (pars.Count() > 2)
                        {
                            if (pars[2] == "i")
                            {
                                cmd.Parameters.AddWithValue((pars[0].ToString()).Replace("@", "p_"), Convert.ToInt32(pars[1]));
                            }
                            else if (pars[2] == "bg")
                            {
                                cmd.Parameters.AddWithValue((pars[0].ToString()).Replace("@", "p_"), Convert.ToInt64(pars[1]));
                            }
                            else if (pars[2] == "s")
                            {
                                cmd.Parameters.AddWithValue((pars[0].ToString()).Replace("@", "p_"), (Convert.ToString(pars[1])));
                            }
                            else if (pars[2] == "d")
                            {
                                cmd.Parameters.AddWithValue((pars[0].ToString()).Replace("@", "p_"), Convert.ToDecimal(pars[1]));
                            }
                            else if (pars[2] == "dt")
                            {
                                cmd.Parameters.AddWithValue((pars[0].ToString()).Replace("@", "p_"), DateTime.ParseExact(pars[1], "yyyy-MM-dd", CultureInfo.InvariantCulture));
                            }
                            else if (pars[2] == "b")
                            {
                                cmd.Parameters.AddWithValue((pars[0].ToString()).Replace("@", "p_"), Convert.ToBoolean(pars[1]));
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue((pars[0].ToString()).Replace("@", "p_"), pars[1]);
                            }
                        }
                        else if (pars.Count() > 1)
                        {
                            cmd.Parameters.AddWithValue((pars[0].ToString()).Replace("@", "p_"), pars[1]);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue((pars[0].ToString()).Replace("@", "p_"), pars[0]);
                        }
                    }
                }

                NpgsqlDataReader dr = cmd.ExecuteReader();

                if (dr == null || dr.FieldCount == 0)
                {
                    return retObject;
                }

                retObject = GetDataObjPgsql(dr);
                nconn.Close();
            }
            else if (dbprv == "sqlserver")
            {
                SqlConnection nconn = new SqlConnection(conn);
                nconn.Open();
                SqlCommand cmd = new SqlCommand(spname, nconn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = nconn.ConnectionTimeout;

                if (list != null && list.Count() > 0)
                {
                    foreach (var item in list)
                    {
                        var pars = item.Split(cstsplit);

                        if (pars.Count() > 2)
                        {
                            if (pars[2] == "i")
                            {
                                cmd.Parameters.AddWithValue(pars[0], Convert.ToInt32(pars[1]));
                            }
                            else if (pars[2] == "s")
                            {
                                cmd.Parameters.AddWithValue(pars[0], Convert.ToString(pars[1]));
                            }
                            else if (pars[2] == "d")
                            {
                                cmd.Parameters.AddWithValue(pars[0], Convert.ToDecimal(pars[1]));
                            }
                            else if (pars[2] == "dt")
                            {
                                cmd.Parameters.AddWithValue(pars[0], DateTime.ParseExact(pars[1], "yyyy-MM-dd", CultureInfo.InvariantCulture));
                            }
                            else if (pars[2] == "b")
                            {
                                cmd.Parameters.AddWithValue(pars[0], Convert.ToBoolean(pars[1]));
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue(pars[0], pars[1]);
                            }
                        }
                        else if (pars.Count() > 1)
                        {
                            cmd.Parameters.AddWithValue(pars[0], pars[1]);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(pars[0], pars[0]);
                        }
                    }
                }

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr == null || dr.FieldCount == 0)
                {
                    return retObject;
                }

                retObject = GetDataObjSqlsvr(dr);
                nconn.Close();
            }

            return retObject;
        }

        public void ExecSqlWithoutReturnCustomSplit(string dbprv, string strname, string cstsplit, string schema, string spname, params string[] list)
        {
            var retObject = new List<dynamic>();
            string message = "";
            StringBuilder sb = new StringBuilder();
            //var conn = dbconn.constringList(dbprv, strname);
            var conn = dbconn.constringList_v2(dbprv, strname);

            if (dbprv == "postgresql")
            {
                spname = schema + "." + spname;
                NpgsqlConnection nconn = new NpgsqlConnection(conn);
                nconn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(spname, nconn);
                cmd.CommandType = CommandType.StoredProcedure;
                if (list != null && list.Count() > 0)
                {
                    foreach (var item in list)
                    {
                        var pars = item.Split(cstsplit);

                        if (pars.Count() > 2)
                        {
                            if (pars[2] == "i")
                            {
                                cmd.Parameters.AddWithValue((pars[0].ToString()).Replace("@", "p_"), Convert.ToInt32(pars[1]));
                            }
                            else if (pars[2] == "s")
                            {
                                cmd.Parameters.AddWithValue((pars[0].ToString()).Replace("@", "p_"), Convert.ToString(pars[1]));
                            }
                            else if (pars[2] == "d")
                            {
                                cmd.Parameters.AddWithValue((pars[0].ToString()).Replace("@", "p_"), Convert.ToDecimal(pars[1]));
                            }
                            else if (pars[2] == "b")
                            {
                                cmd.Parameters.AddWithValue((pars[0].ToString()).Replace("@", "p_"), Convert.ToBoolean(pars[1]));
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue((pars[0].ToString()).Replace("@", "p_"), pars[1]);
                            }
                        }
                        else if (pars.Count() > 1)
                        {
                            cmd.Parameters.AddWithValue((pars[0].ToString()).Replace("@", "p_"), pars[1]);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue((pars[0].ToString()).Replace("@", "p_"), pars[0]);
                        }
                    }
                }
                try
                {
                    cmd.ExecuteNonQuery();
                    message = mc.GetMessage("execdb_success");
                }
                catch (NpgsqlException e)
                {
                    message = e.Message;
                }
                finally
                {
                    nconn.Close();
                }
            }
            else if (dbprv == "sqlserver")
            {
                SqlConnection nconn = new SqlConnection(conn);
                nconn.Open();
                SqlCommand cmd = new SqlCommand(spname, nconn);
                cmd.CommandType = CommandType.StoredProcedure;
                if (list != null && list.Count() > 0)
                {
                    foreach (var item in list)
                    {
                        var pars = item.Split(cstsplit);

                        if (pars.Count() > 2)
                        {
                            if (pars[2] == "i")
                            {
                                cmd.Parameters.AddWithValue(pars[0], Convert.ToInt32(pars[1]));
                            }
                            else if (pars[2] == "s")
                            {
                                cmd.Parameters.AddWithValue(pars[0], Convert.ToString(pars[1]));
                            }
                            else if (pars[2] == "d")
                            {
                                cmd.Parameters.AddWithValue(pars[0], Convert.ToDecimal(pars[1]));
                            }
                            else if (pars[2] == "b")
                            {
                                cmd.Parameters.AddWithValue(pars[0], Convert.ToBoolean(pars[1]));
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue(pars[0], pars[1]);
                            }
                        }
                        else if (pars.Count() > 1)
                        {
                            cmd.Parameters.AddWithValue(pars[0], pars[1]);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue(pars[0], pars[0]);
                        }
                    }
                }
                try
                {
                    cmd.ExecuteNonQuery();
                    message = mc.GetMessage("execdb_success");
                }
                catch (NpgsqlException e)
                {
                    message = e.Message;
                }
                finally
                {
                    nconn.Close();
                }
            }
        }

        public JObject CheckValidation(string dbprv, string strname, string qry)
        {
            var data = new JObject();
            var retObject = new List<dynamic>();
            StringBuilder sb = new StringBuilder();
            
            var conn = dbconn.constringList_v2(dbprv, strname);

            if (dbprv.ToLower() == "postgresql")
            {
                NpgsqlConnection nconn = new NpgsqlConnection(conn);
                nconn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(qry, nconn);
                cmd.CommandType = CommandType.Text;
                try
                {
                    NpgsqlDataReader dr = cmd.ExecuteReader();
                    data.Add("status", mc.GetMessage("api_output_ok"));
                    data.Add("message", "Valid");

                    nconn.Close();
                    NpgsqlConnection.ClearPool(nconn);
                }
                catch (Exception ex)
                {
                    data.Add("status", mc.GetMessage("api_output_not_ok"));
                    data.Add("message", ex.Message);
                    nconn.Close();
                    NpgsqlConnection.ClearPool(nconn);
                }
            }

            return data;
        }

        public string CheckValueData(JObject json, string prop)
        {
            var strReturn = "";
            if (json.Property(prop) != null)
            {
                strReturn = json.GetValue(prop).ToString();
            }
            return strReturn;
        }

        public void ExecuteQueryScript(string dbprv, string strname, string qry)
        {
            var retObject = new List<dynamic>();
            StringBuilder sb = new StringBuilder();
            //var conn = dbconn.constringList(dbprv, strname);
            var conn = dbconn.constringList_v2(dbprv, strname);

            if (dbprv.ToLower() == "postgresql")
            {
                NpgsqlConnection nconn = new NpgsqlConnection(conn);
                nconn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(qry, nconn);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                nconn.Close();
            }
            else
            {
                SqlConnection nconn = new SqlConnection(conn);
                nconn.Open();
                SqlCommand cmd = new SqlCommand(qry, nconn);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                nconn.Close();

            }
        }

        public List<dynamic> ExecuteQueryScriptWithReturn(string dbprv, string strname, string qry)
        {
            var retObject = new List<dynamic>();
            StringBuilder sb = new StringBuilder();
            //var conn = dbconn.constringList(dbprv, strname);
            var conn = dbconn.constringList_v2(dbprv, strname);

            if (dbprv.ToLower() == "postgresql")
            {
                NpgsqlConnection nconn = new NpgsqlConnection(conn);
                nconn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(qry, nconn);
                cmd.CommandType = CommandType.Text;
                NpgsqlDataReader dr = cmd.ExecuteReader();
                if (dr == null || dr.FieldCount == 0)
                {
                    nconn.Close();
                    return retObject;
                }

                retObject = GetDataObjPgsql(dr);
                nconn.Close();
            }

            return retObject;
        }

        public List<dynamic> execSQLWithOutputDynamicPrm(string strname, string str)
        {
            var conn = dbconn.constringList(strname);

            SqlConnection nconn = new SqlConnection(conn);
            var retObject = new List<dynamic>();
            nconn.Open();

            SqlCommand cmd = new SqlCommand(str, nconn);
            cmd.CommandType = CommandType.Text;

            try
            {


                SqlDataReader dr = cmd.ExecuteReader();

                if (dr == null || dr.FieldCount == 0)
                {
                    nconn.Close();
                    if (nconn.State.Equals(ConnectionState.Open))
                    {
                        nconn.Close();
                    }
                    SqlConnection.ClearPool(nconn);
                    return retObject;
                }

                retObject = GetDataObjtosqlserver(dr);
                nconn.Close();
                if (nconn.State.Equals(ConnectionState.Open))
                {
                    nconn.Close();
                }

                SqlConnection.ClearPool(nconn);

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                nconn.Close();
                if (nconn.State.Equals(ConnectionState.Open))
                {
                    nconn.Close();
                }

                SqlConnection.ClearPool(nconn);
            }


            return retObject;
        }

        
        public List<dynamic> GetDataObjtosqlserver(SqlDataReader dr)
        {
            var retObject = new List<dynamic>();
            while (dr.Read())
            {
                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    dataRow.Add(
                           dr.GetName(i),
                           dr.IsDBNull(i) ? null : dr[i] // use null instead of {}
                   );
                }
                retObject.Add((ExpandoObject)dataRow);
            }

            return retObject;
        }

        public void execSql(string sql, string dbprv,string strname)
        {
            //var conn = dbconn.conString();
            var retObject = new List<dynamic>();
  
            var conn = dbconn.constringList_v2(dbprv, strname);


            using (SqlConnection nconn = new SqlConnection(conn))
            {
                
                nconn.Open();
                SqlCommand cmd = new SqlCommand(sql, nconn);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                nconn.Close();
            }
          
        }


        public string execExtAPIPostWithToken(string api, string path, string json, string credential)
        {
            string result = "";
            dbconn.CallAPiRequestLogs("", api, "POST", path, "", json);

            result = execExtAPIPostWithTokenAwait(api, path, json, credential).Result;
            dbconn.CallAPiResponseLogs("", api, "POST", path, "", json, result);
            return result;
        }


        public async Task<string> execExtAPIPostWithTokenAwait(string api, string path, string json, string credential)
        {
            #region call others api version
            string result = "";
            var WebAPIURL = dbconn.domainGetApi(api);
            string requestStr = WebAPIURL + path;

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


            //var client = new HttpClient();
            //client.DefaultRequestHeaders.Add("Authorization", credential);
            if (!client.DefaultRequestHeaders.Contains("Authorization"))
            {
                client.DefaultRequestHeaders.Add("Authorization", credential);
            }
            else
            {
                client.DefaultRequestHeaders.Remove("Authorization");
                client.DefaultRequestHeaders.Add("Authorization", credential);
            }
            var contentData = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(requestStr, contentData);
            result = await response.Content.ReadAsStringAsync();
            #endregion

            client.Dispose();
            return result;
        }

        public string execExtAPIGetWithToken(string api, string path, string credential)
        {
            string result = "";
            dbconn.CallAPiRequestLogs("", api, "GET", path, "", "");
            #region call others api version v.1 diremark dulu
            //var WebAPIURL = dbconn.domainGetApi(api);
            //string requestStr = WebAPIURL + path;

            //var client = new HttpClient();
            //client.DefaultRequestHeaders.Add("Authorization", credential);
            //HttpResponseMessage response = client.GetAsync(requestStr).Result;
            //result = response.Content.ReadAsStringAsync().Result;
            #endregion
            result = execExtAPIGetWithTokenAwait(api, path, credential).Result;

            dbconn.CallAPiResponseLogs("", api, "GET", path, "", "", result);
            return result;
        }

        public async Task<string> execExtAPIGetWithTokenAwait(string api, string path, string credential)
        {
            string result = "";
            var WebAPIURL = dbconn.domainGetApi(api);
            string requestStr = WebAPIURL + path;

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

            //var client = new HttpClient();
            //client.DefaultRequestHeaders.Add("Authorization", credential);
            if (!client.DefaultRequestHeaders.Contains("Authorization"))
            {
                client.DefaultRequestHeaders.Add("Authorization", credential);
            }
            else
            {
                client.DefaultRequestHeaders.Remove("Authorization");
                client.DefaultRequestHeaders.Add("Authorization", credential);
            }

            HttpResponseMessage response = await client.GetAsync(requestStr);
            result = await response.Content.ReadAsStringAsync();
            client.Dispose();
            return result;
        }

    }
}
