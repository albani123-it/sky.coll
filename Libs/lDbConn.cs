using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sky.coll.Libs
{
    public class lDbConn
    {
        private lConvert lc = new lConvert();
        public string sqlprovider()
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            var config = builder.Build();
            return "" + config.GetSection("SqlProvider:provider").Value.ToString();
        }

        public string constringName(string cstr)
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            var config = builder.Build();
            return "" + config.GetSection("constringName:" + cstr).Value.ToString();
        }

        public string conString()
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            var config = builder.Build();
            return "" + config.GetSection("DbContextSettings:ConnectionString").Value.ToString();
        }

        public string conStringLog()
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            var config = builder.Build();
            return "" + config.GetSection("DbContextSettings:ConnectionString_log").Value.ToString();
        }

        #region -- connnection string by database --

        public string constringList(string dbprv, string strname)
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            var config = builder.Build();

            //var configPass = lc.decrypt(config.GetSection("configPass:passwordDB").Value.ToString());

            var configDB = config.GetSection("DbContextSettings:" + dbprv + ":" + strname).Value.ToString();

            //var repPass = configDB.Replace("{pass}", configPass);
            return "" + configDB;


        }


        public string constringList(string strname)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var config = builder.Build();
            //var configPass = lc.decrypt(config.GetSection("configPass:passwordDB").Value.ToString());
            var configDB = config.GetSection("DbContextSettings:" + strname).Value.ToString();

            //var repPass = configDB.Replace("{pass}", configPass);
            return "" + configDB;
        }

        public string conStringLogProcess()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var config = builder.Build();
            var configPass = lc.decrypt(config.GetSection("configPass:passwordDB").Value.ToString());
            var configDB = config.GetSection("DbContextSettings:ConnectionString_log").Value.ToString();

            var repPass = configDB.Replace("{pass}", configPass);
            return "" + repPass;
        }

        public string conString_v2()
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            var config = builder.Build();
            //var configPass = lc.decrypt(config.GetSection("configPass:passwordDB").Value.ToString());

            var configDB = config.GetSection("DbContextSettings:ConnectionString").Value.ToString();

           // var repPass = configDB.Replace("{pass}", configPass);
            return "" + configDB;

        }

        public string constringList_v2(string dbprv, string strname)
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            var config = builder.Build();

            //var configPass = lc.decrypt(config.GetSection("configPass:passwordDB").Value.ToString());

            var configDB = config.GetSection("DbContextSettings:" + dbprv + ":" + strname).Value.ToString();

            //var repPass = configDB.Replace("{pass}", configPass);
            return "" + configDB;


        }

        public string conStringLog_v2()
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            var config = builder.Build();
            var configPass = lc.decrypt(config.GetSection("configPass:passwordDB").Value.ToString());

            var configDB = config.GetSection("DbContextSettings:ConnectionString_log").Value.ToString();

            var repPass = configDB.Replace("{pass}", configPass);
            return "" + repPass;

        }
        #endregion

        public string getAppSettingParam(string group, string api)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var config = builder.Build();
            return "" + config.GetSection(group + ":" + api).Value.ToString();
        }

        public string domainGetApi(string api)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var config = builder.Build();
            return "" + config.GetSection("APISettings:" + api).Value.ToString();
        }

        public string domainGetTokenCredential(string param)
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            var config = builder.Build();
            return config.GetSection("TokenAuthentication:" + param).Value.ToString();
        }


        public string domainPostApi()
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            var config = builder.Build();
            return "" + config.GetSection("DomainSettings:urlPostDomainAPI").Value.ToString();
        }

        public string getFromAddress()
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            var config = builder.Build();
            return "" + config.GetSection("NotificationSetting:From").Value.ToString();
        }

        public string getTitleFrom()
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            var config = builder.Build();
            return "" + config.GetSection("NotificationSetting:TitleFrom").Value.ToString();
        }

        public string getTitleTo()
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            var config = builder.Build();
            return "" + config.GetSection("NotificationSetting:TitleTo").Value.ToString();
        }

        public string getSubjectNotification()
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            var config = builder.Build();
            return "" + config.GetSection("NotificationSetting:SubjectNotification").Value.ToString();
        }

        public string getLogo()
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            var config = builder.Build();
            return "" + config.GetSection("NotificationSetting:PathLogo").Value.ToString();
        }

        public string getSmtpServer()
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            var config = builder.Build();
            return "" + config.GetSection("NotificationSetting:SmtpServer").Value.ToString();
        }

        public string getSmtpPortNumber()
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            var config = builder.Build();
            return "" + config.GetSection("NotificationSetting:SmtpPortNumber").Value.ToString();
        }

        public string getAuthenticateUsr()
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            var config = builder.Build();
            return "" + config.GetSection("NotificationSetting:AuthenticateUsr").Value.ToString();
        }

        public string getAuthenticatePwd()
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            var config = builder.Build();
            return "" + config.GetSection("NotificationSetting:AuthenticatePwd").Value.ToString();
        }

        public string getByPassEmail()
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            var config = builder.Build();
            return "" + config.GetSection("NotificationSetting:ByPassEmail").Value.ToString();
        }

        public string getAnalystEmail()
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            var config = builder.Build();
            return "" + config.GetSection("NotificationSetting:AnalystEmail").Value.ToString();
        }

        public string getApprovalEmail()
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            var config = builder.Build();
            return "" + config.GetSection("NotificationSetting:ApprovalEmail").Value.ToString();
        }

        public string GetSettingBiroKredit(string data)
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            var config = builder.Build();
            return "" + config.GetSection("IntgrationSetting:" + data).Value.ToString();
        }

        public string domainGetCredential(string param)
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            var config = builder.Build();
            return config.GetSection("Credential:" + param).Value.ToString();
        }

        internal void CallAPiRequestLogs(string module, string code, string method, string path, string header, string body)
        {
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            string filename = "req_" + code + ".txt";
            string split = "|";

            string resHeader = "Timestamp" + split + "Method" + split + "Path" + split + "Header" + split + "Body";
            string resBody = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + split + method + split + path + split + (header).Replace("\r\n", "") + split + (body).Replace("\r\n", "");
            string folder = "File/logs/" + today + "/callotherapi/" + code + "/";

            string txtPath = Path.GetFullPath(folder);

            if (!Directory.Exists(txtPath))
            {
                Directory.CreateDirectory(txtPath);
            }

            string txt = resHeader + Environment.NewLine + resBody;
            if (!File.Exists(txtPath + filename))
            {
                txt = resHeader + Environment.NewLine + resBody;
            }
            else
            {
                txt = resBody;
            }
            //insert txt file
            var logWriter = new System.IO.StreamWriter(txtPath + filename, append: true);
            logWriter.WriteLine(txt);
            logWriter.Dispose();

        }

        public void CallAPiResponseLogs(string module, string code, string method, string path, string header, string body, string result)
        {
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            string filename = "res_" + code + ".txt";
            string split = "|";
            string folder = "File/logs/" + today + "/callotherapi/" + code + "/";

            string resHeader = "Timestamp" + split + "Method" + split + "Path" + split + "Header" + split + "Request" + split + "Response";
            string resBody = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + split + method + split + path + split + (header).Replace("\r\n", "") + split +
                (body).Replace("\r\n", "") + split + (result).Replace("\r\n", "");

            string txtPath = Path.GetFullPath(folder);

            if (!Directory.Exists(txtPath))
            {
                Directory.CreateDirectory(txtPath);
            }

            string txt = resHeader + Environment.NewLine + resBody;
            if (!File.Exists(txtPath + filename))
            {
                txt = resHeader + Environment.NewLine + resBody;
            }
            else
            {
                txt = resBody;
            }
            //insert txt file
            var logWriter = new System.IO.StreamWriter(txtPath + filename, append: true);
            logWriter.WriteLine(txt);
            logWriter.Dispose();
        }

        internal void SaveFile(JObject json, string rshid, string applname, string group, string prefix)
        {
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            var folder = "File/" + group + "/";
            var filepath = folder + today;

            var filename = "";
            if (string.IsNullOrWhiteSpace(rshid))
            {
                filename = prefix + applname + ".json";
            }
            else
            {
                filename = prefix + rshid + "_" + applname + ".json";
            }


            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }

            var filenamePath = filepath + "/" + filename;
            FileInfo fi = new FileInfo(filenamePath);

            //check 1st check file jika sudah ada di delete dan generate baru
            if (fi.Exists)
            {
                fi.Delete();
            }

            // Create a new file     
            using (FileStream fs = fi.Create())
            {
                Byte[] txt = new UTF8Encoding(true).GetBytes("New file.");
                fs.Write(txt, 0, txt.Length);
                Byte[] author = new UTF8Encoding(true).GetBytes("idxteam");
                fs.Write(author, 0, author.Length);
            }
            System.IO.File.WriteAllText(filenamePath, json.ToString());
        }

        public string getFilePath()
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            var config = builder.Build();
            return "" + config.GetSection("FileSetting:serverimg_path").Value.ToString();
        }
    }
}
