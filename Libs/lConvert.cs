using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace sky.coll.Libs
{
    public class lConvert
    {
        private const string INDENT_STRING = "  ";
        public JArray convertDynamicToJArray(List<dynamic> list)
        {
            var jsonObject = new JObject();
            dynamic data = jsonObject;
            data.Lists = new JArray() as dynamic;
            dynamic detail = new JObject();
            foreach (dynamic dr in list)
            {
                detail = new JObject();
                foreach (var pair in dr)
                {
                    detail.Add(pair.Key, pair.Value);
                }
                data.Lists.Add(detail);
            }
            return data.Lists;
        }
        public List<string> convertDynamicToString(List<dynamic> dynamic)
        {
            var list = new List<string>();
            foreach (dynamic dr in dynamic)
            {
                list.Add(dr.cname);
            }
            return list;
        }

        public JObject convertDynamicToSingleJObject(List<dynamic> list)
        {
            var jsonObject = new JObject();
            foreach (dynamic dr in list)
            {
                jsonObject.Add(dr.p_key, dr.p_val);
            }
            return jsonObject;
        }

        public string EncryptString(string encryptString, string EncryptionKey)
        {
            //string EncryptionKey = "idxp@rtn3rs";
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Dispose();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }

        public string DecryptString(string cipherText, string EncryptionKey)
        {
            //string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Dispose();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public string encrypt(string str)
        {
            var key = "SkyW0rxC0n5ult1n9";
            string encrypted = EncryptString(str, key);
            return encrypted;
        }

        public string decrypt(string str)
        {

            var key = "SkyW0rxC0n5ult1n9";
            string decrypted = DecryptString(str, key);
            return decrypted;
        }

        public string TdesDecrypt(string key, string str)
        {
            string decrypted = DecryptString(str, key);
            return decrypted;
        }

        public string ConvertStringMonth(string year, string month)
        {
            string strResult = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), 1).ToString("MMMM", CultureInfo.InvariantCulture);

            return strResult;
        }

        public string CheckIsJObject(string strData)
        {
            var str = "";
            try
            {
                var jToken = JToken.Parse(strData);
                if (jToken is JObject)
                {
                    var jodata = JObject.Parse(strData);
                    if (jodata.Property("@i:nil") == null)
                    {
                        str = strData;
                    }
                    else
                    {
                        str = "";
                    }
                }
                else
                {
                    str = strData;
                }
            }
            catch (Exception)
            {
                str = strData;
            }
            return str;
        }

        public string SetupDateTime(string strData)
        {
            var rtnData = "";

            if (!string.IsNullOrWhiteSpace(strData))
            {
                rtnData = Convert.ToDateTime(strData).ToString("yyyy-MM-dd HH:mm:ss");
                rtnData = rtnData.Replace(".", ":");
            }
            else
            {
                rtnData = strData;
            }
            return rtnData;
        }

        public string CheckValueData(JObject json, string prop)
        {
            var strReturn = "";
            if (json.Property(prop) != null)
            {

                if (prop.ToLower().IndexOf("date") >= 0)
                {
                    if (!string.IsNullOrWhiteSpace(json.GetValue(prop).ToString()))
                    {
                        if (json.GetValue(prop).ToString() == "{\r\n  \"@i:nil\": \"true\"\r\n}")
                        {
                            strReturn = "";
                        }
                        else
                        {
                            strReturn = Convert.ToDateTime(json.GetValue(prop)).ToString("yyyy-MM-dd HH:mm:ss");
                        }

                    }
                    else
                    {
                        strReturn = json.GetValue(prop).ToString();
                    }

                }
                else
                {
                    strReturn = json.GetValue(prop).ToString();
                }

            }
            return strReturn;
        }

        public string FormatJson(string json)
        {

            int indentation = 0;
            int quoteCount = 0;
            var result =
                from ch in json
                let quotes = ch == '"' ? quoteCount++ : quoteCount
                let lineBreak = ch == ',' && quotes % 2 == 0 ? ch + Environment.NewLine + String.Concat(Enumerable.Repeat(INDENT_STRING, indentation)) : null
                let openChar = ch == '{' || ch == '[' ? ch + Environment.NewLine + String.Concat(Enumerable.Repeat(INDENT_STRING, ++indentation)) : ch.ToString()
                let closeChar = ch == '}' || ch == ']' ? Environment.NewLine + String.Concat(Enumerable.Repeat(INDENT_STRING, --indentation)) + ch : ch.ToString()
                select lineBreak == null
                            ? openChar.Length > 1
                                ? openChar
                                : closeChar
                            : lineBreak;

            return String.Concat(result);
        }



    }
}
