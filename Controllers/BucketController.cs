using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sky.coll.Libs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using Newtonsoft.Json;


namespace sky.coll.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("skycoll/[controller]")]
    public class BucketController : Controller
    {
        private BaseController bc = new BaseController();
        private BasetrxController bcx = new BasetrxController();
        private lDbConn dbconn = new lDbConn();
        private MessageController mc = new MessageController();
        private TokenController tc = new TokenController();
        private lConvert lc = new lConvert();
        private lDataLayer ldl = new lDataLayer();

        [HttpGet("list")]
        public JObject GetListBucket()
        {
            var retObject = new List<dynamic>();
            var data = new JObject();
            try
            {
                data = new JObject();
                retObject = ldl.GetlistBucket();

                data.Add("status", mc.GetMessage("api_output_ok"));
                data.Add("message", mc.GetMessage("process_success"));
                data.Add("data", lc.convertDynamicToJArray(retObject));
            }
            catch (Exception ex)
            {
                data = new JObject();
                data.Add("status", mc.GetMessage("api_output_not_ok"));
                data.Add("message", ex.Message);
            }

            return data;
        }

        [HttpPost("detail")]
        public JObject GetDetailBucket([FromBody] JObject json)
        {
            
            var data = new JObject();
            var jFormDetData = new JObject();
            var jaFormDet = new JArray();
            try
            {
                data = new JObject();
                var id = json.GetValue("id").ToString();
                var dtReturn1 = ldl.getdetailbucketbyid(id);
                if (dtReturn1.Count > 0)
                {
                    jFormDetData.Add("id", dtReturn1[0]["v_id"].ToString());
                    jFormDetData.Add("code", dtReturn1[0]["v_code"].ToString());
                    jFormDetData.Add("name", dtReturn1[0]["v_name"].ToString());
                    jFormDetData.Add("statusid", dtReturn1[0]["v_status_id"].ToString());

                    var detailbucket = ldl.getdetailbucketdetailbyid(id);

                    for (int i = 0; i < detailbucket.Count(); i++)
                    {
                        var jFormDetData1 = new JObject();
                        //jFormDetData1.Add("id", detailbucket[i]["v_bcd_id"].ToString());
                        //jFormDetData1.Add("header_id", detailbucket[i]["v_bcd_bct_id"].ToString());
                        jFormDetData1.Add("usrid", detailbucket[i]["v_bcd_userid"].ToString());
                        jaFormDet.Add(jFormDetData1);
                    }

                    jFormDetData.Add("detail", jaFormDet);

                    data.Add("status", mc.GetMessage("api_output_ok"));
                    data.Add("message", mc.GetMessage("process_success"));
                    data.Add("data", new JArray(jFormDetData));
                }
                else
                {
                    data = new JObject();
                    data.Add("status", mc.GetMessage("api_output_not_ok"));
                    data.Add("message", "bucket not found");

                }

                  
            }
            catch (Exception ex)
            {
                data = new JObject();
                data.Add("status", mc.GetMessage("api_output_not_ok"));
                data.Add("message", ex.Message);
            }

            return data;
        }

        [HttpPost("insert")]
        public JObject bucketinsert([FromBody] JObject json)
        {
            var data = new JObject();
            string strout = "";
            try
            {
                strout = bcx.insertbucket(json);
                if (strout == "success")
                {
                    data.Add("status", mc.GetMessage("api_output_ok"));
                    data.Add("message", mc.GetMessage("save_success"));
                }
                else
                {
                    data.Add("status", mc.GetMessage("api_output_not_ok"));
                    data.Add("message", strout);
                }

            }
            catch (Exception ex)
            {
                data.Add("status", mc.GetMessage("api_output_not_ok"));
                data.Add("message", ex.Message);
            }
            return data;
        }

        [HttpPost("update")]
        public JObject bucketupdate([FromBody] JObject json)
        {
            var data = new JObject();
            string strout = "";
            try
            {
                strout = bcx.updatebucket(json);
                if (strout == "success")
                {
                    data.Add("status", mc.GetMessage("api_output_ok"));
                    data.Add("message", mc.GetMessage("save_success"));
                }
                else
                {
                    data.Add("status", mc.GetMessage("api_output_not_ok"));
                    data.Add("message", strout);
                }

            }
            catch (Exception ex)
            {
                data.Add("status", mc.GetMessage("api_output_not_ok"));
                data.Add("message", ex.Message);
            }
            return data;
        }

        [HttpPost("delete")]
        public JObject bucketdelete([FromBody] JObject json)
        {
            var data = new JObject();
            string strout = "";
            try
            {
                strout = bcx.deletebucket(json);
                if (strout == "success")
                {
                    data.Add("status", mc.GetMessage("api_output_ok"));
                    data.Add("message", mc.GetMessage("save_success"));
                }
                else
                {
                    data.Add("status", mc.GetMessage("api_output_not_ok"));
                    data.Add("message", strout);
                }

            }
            catch (Exception ex)
            {
                data.Add("status", mc.GetMessage("api_output_not_ok"));
                data.Add("message", ex.Message);
            }
            return data;
        }
    }
}
