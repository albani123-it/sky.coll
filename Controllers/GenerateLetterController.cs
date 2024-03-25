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
    public class GenerateLetterController : Controller
    {
        private BaseController bc = new BaseController();
        private BasetrxController bcx = new BasetrxController();
        private lDbConn dbconn = new lDbConn();
        private MessageController mc = new MessageController();
        private TokenController tc = new TokenController();
        private lConvert lc = new lConvert();
        private lDataLayer ldl = new lDataLayer();

        [HttpPost("letter")]
        public JObject Getgenerateletter([FromBody] JObject json)
        {
            var retObject = new List<dynamic>();
            var data = new JObject();
            try
            {
               
                    var p_userid = json.GetValue("usr").ToString();
                    var dtReturn1 = ldl.checkbranchactivebyuserid(p_userid);
                    if (dtReturn1.Count > 0)
                    {
                        data = new JObject();
                        var branchcode = dtReturn1[0]["lbrc_code"].ToString();
                      
                        retObject = ldl.Getlistgenerateletter(branchcode);
                        data.Add("status", mc.GetMessage("api_output_ok"));
                        data.Add("message", mc.GetMessage("process_success"));
                        data.Add("data", lc.convertDynamicToJArray(retObject));
                    }
                    else
                    {
                        data = new JObject();
                        data.Add("status", mc.GetMessage("api_output_not_ok"));
                        data.Add("message", mc.GetMessage("process_not_success"));
                        data.Add("data", new JArray());
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

        [HttpPost("letter/history")]
        public JObject Getgenerateletterhistory([FromBody] JObject json)
        {
            var retObject = new List<dynamic>();
            var data = new JObject();
            try
            {
                var p_userid = json.GetValue("usr").ToString();
                var dtReturn = ldl.getDataUserdetail(p_userid);

                if (dtReturn[0]["usraccesslevel"].ToString() == "31")
                {
                    var dtReturn1 = ldl.checkbranchactivebyuserid(p_userid);
                    if (dtReturn1.Count > 0)
                    {
                        data = new JObject();
                        retObject = ldl.Getlistgenerateletterhistorybybranchname(dtReturn1[0]["lbrc_name"].ToString());
                    }
                    else
                    {
                        data = new JObject();
                        retObject = ldl.Getlistgenerateletterhistorybybrnkyz();
                    }
                }
                else
                {
                    data = new JObject();
                    retObject = ldl.Getlistgenerateletterhistory();
                }


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

        [HttpPost("letter/view")]
        public JObject Getgenerateletterview([FromBody] JObject json)
        {
            var retObject = new List<dynamic>();
            var data = new JObject();
            try
            {

                var loan_id = json.GetValue("loan_id").ToString();
                var dtReturn1 = ldl.checkloanmasterbyid(loan_id);
                if (dtReturn1.Count > 0)
                {
                    data = new JObject();
                    var dpd = dtReturn1[0]["v_dpd"].ToString();

                    retObject = ldl.Getdetailgenerateletter(loan_id, dpd);
                    data.Add("status", mc.GetMessage("api_output_ok"));
                    data.Add("message", mc.GetMessage("process_success"));
                    data.Add("data", lc.convertDynamicToJArray(retObject));
                }
                else
                {
                    data = new JObject();
                    data.Add("status", mc.GetMessage("api_output_not_ok"));
                    data.Add("message", "data loan not found");
                    data.Add("data", new JArray());
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

        [HttpPost("letter/download")]
        public JObject Getgenerateletterdownload([FromBody] JObject json)
        {
            var retObject = new List<dynamic>();
            var data = new JObject();
            try
            {

                var loan_id = json.GetValue("loan_id").ToString();
                var code = json.GetValue("code").ToString();
                var dtReturn1 = ldl.checkloanmasterbyid(loan_id);
                if (dtReturn1.Count > 0)
                {
                    data = new JObject();
                    if (code != "")
                    {
                        if (code == "ST")
                        {
                            retObject = ldl.Getdetailgenerateletterdownload(loan_id, code);
                            if (retObject.Count > 0)
                            {
                                data.Add("status", mc.GetMessage("api_output_ok"));
                                data.Add("message", mc.GetMessage("process_success"));
                                data.Add("data", lc.convertDynamicToJArray(retObject));
                            }
                            else
                            {
                                ldl.Getdetailgenerateletterinsert(loan_id, code);
                                data.Add("status", mc.GetMessage("api_output_ok"));
                                data.Add("message", mc.GetMessage("save_success"));
                                data.Add("data", new JArray());

                            }
                               
                        }
                        else if (code == "SP1")
                        {
                            retObject = ldl.Getdetailgenerateletterdownload(loan_id, code);
                            if (retObject.Count > 0)
                            {
                                data.Add("status", mc.GetMessage("api_output_ok"));
                                data.Add("message", mc.GetMessage("process_success"));
                                data.Add("data", lc.convertDynamicToJArray(retObject));
                            }
                            else
                            {
                                ldl.Getdetailgenerateletterinsert(loan_id, code);
                                data.Add("status", mc.GetMessage("api_output_ok"));
                                data.Add("message", mc.GetMessage("save_success"));
                                data.Add("data", new JArray());

                            }
                        }
                        else if (code == "SP2")
                        {
                            retObject = ldl.Getdetailgenerateletterdownload(loan_id, code);
                            if (retObject.Count > 0)
                            {
                                data.Add("status", mc.GetMessage("api_output_ok"));
                                data.Add("message", mc.GetMessage("process_success"));
                                data.Add("data", lc.convertDynamicToJArray(retObject));
                            }
                            else
                            {
                                ldl.Getdetailgenerateletterinsert(loan_id, code);
                                data.Add("status", mc.GetMessage("api_output_ok"));
                                data.Add("message", mc.GetMessage("save_success"));
                                data.Add("data", new JArray());

                            }
                        }
                        else if (code == "SP3")
                        {
                            retObject = ldl.Getdetailgenerateletterdownload(loan_id, code);
                            if (retObject.Count > 0)
                            {
                                data.Add("status", mc.GetMessage("api_output_ok"));
                                data.Add("message", mc.GetMessage("process_success"));
                                data.Add("data", lc.convertDynamicToJArray(retObject));
                            }
                            else
                            {
                                ldl.Getdetailgenerateletterinsert(loan_id, code);
                                data.Add("status", mc.GetMessage("api_output_ok"));
                                data.Add("message", mc.GetMessage("save_success"));
                                data.Add("data", new JArray());

                            }
                        }
                    }
                    else
                    {
                        data = new JObject();
                        data.Add("status", mc.GetMessage("api_output_not_ok"));
                        data.Add("message", "code not found");
                        data.Add("data", new JArray());
                    }

                }
                else
                {
                    data = new JObject();
                    data.Add("status", mc.GetMessage("api_output_not_ok"));
                    data.Add("message", "data loan not found");
                    data.Add("data", new JArray());
                }

            }
            catch (Exception ex)
            {
                data = new JObject();
                data.Add("status", mc.GetMessage("api_output_not_ok"));
                data.Add("message", ex.Message);
                data.Add("data", new JArray());
            }

            return data;
        }

    }
}
