using sky.coll.Controllers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sky.coll.Libs
{
    public class lGeneral
    {
        private lDbConn dbconn = new lDbConn();
        private lConvert lc = new lConvert();
        private BaseController bc = new BaseController();
        private MessageController mc = new MessageController();

        internal string GetAppliedData(JObject json)
        {
            string strResult = "";
            if (json.ContainsKey("data_applied"))
            {
                strResult = json.GetValue("data_applied").ToString();
            }
            else
            {
                strResult = "application_data";
            }
            if (string.IsNullOrWhiteSpace(strResult))
            {
                strResult = "application_data";
            }
            return strResult;
        }


        internal JObject ValidationByCategory(string data_applied, string category, string condition)
        {
            var data = new JObject();
            string strQry = "";
    
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skyen");

            try
            {
                if (category.ToLower() == "rule")
                {
                    if (data_applied.ToLower() == "application_data" || data_applied == "")
                    {
                        if (string.IsNullOrWhiteSpace(condition))
                        {
                            condition = "1=1";
                        }
                        strQry = "select * from workflow.decflow_log_history where " + condition + " limit 1;";
                    }
                   
                }
                else
                {
                    if (data_applied.ToLower() == "application_data" || data_applied == "")
                    {
                        if (string.IsNullOrWhiteSpace(condition))
                        {
                            condition = "";
                        }
                        strQry = "select (" + condition + ") :: varchar from workflow.decflow_log_history  limit 1";
                    }
                  
                }

                data = bc.CheckValidation(provider, cstrname, strQry);
            }
            catch (Exception ex)
            {
                data.Add("status", mc.GetMessage("api_output_not_ok"));
                data.Add("message", "Invalid");
                data.Add("err", ex.Message);
            }

            return data;
        }

        internal JObject ValidateRuleGB(JObject json)
        {
            var good_rul_id = "";
            var bad_rul_id = "";

            if (json.ContainsKey("good_rul_id"))
            {
                good_rul_id = json.GetValue("good_rul_id").ToString();
                if (string.IsNullOrWhiteSpace(good_rul_id))
                {
                    json["good_rul_id"] = "0";
                }
            }
            else
            {
                json.Add("good_rul_id", "0");
            }

            if (json.ContainsKey("bad_rul_id"))
            {
                bad_rul_id = json.GetValue("bad_rul_id").ToString();
                if (string.IsNullOrWhiteSpace(bad_rul_id))
                {
                    json["bad_rul_id"] = "0";
                }
            }
            else
            {
                json.Add("bad_rul_id", "0");
            }

            return json;
        }
    }
}
