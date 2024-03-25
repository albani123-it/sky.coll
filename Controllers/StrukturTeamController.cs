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

    public class StrukturTeamController : Controller
    {
        private BaseController bc = new BaseController();
        private BasetrxController bcx = new BasetrxController();
        private lDbConn dbconn = new lDbConn();
        private MessageController mc = new MessageController();
        private TokenController tc = new TokenController();
        private lConvert lc = new lConvert();
        private lDataLayer ldl = new lDataLayer();

        [HttpGet("teamfc/list")]
        public JObject GetListTeamFC()
        {
            var retObject = new List<dynamic>();
            var data = new JObject();
           
            var jaFormDet = new JArray();
            try
            {
                data = new JObject();
                JObject jFormCont = new JObject();
                JArray jaDrp = new JArray();
                var dtretrun = ldl.Getlistteamspvfc();
                if (dtretrun.Count > 0)
                {
                    for (int i = 0; i < dtretrun.Count; i++)
                    {
                        jFormCont = new JObject();
                        var jFormDetData = new JObject();
                        var spvid = dtretrun[i]["v_usr_userid"].ToString();
                        var dtretrun1 = ldl.Getteamfc(spvid);
                      

                        jFormCont.Add("spvid", dtretrun[i]["v_spvid"].ToString());
                        jFormCont.Add("spvuserid", dtretrun[i]["v_usr_userid"].ToString());
                        jFormCont.Add("spvname", dtretrun[i]["v_usr_name"].ToString() + " (" + dtretrun[i]["v_status"].ToString() + ")" );
                        jFormCont.Add("spvrolename", dtretrun[i]["v_rol_name"].ToString());
                        //jFormCont.Add("spvstatus", dtretrun[i]["v_status"].ToString());
                        jFormCont.Add("agent", dtretrun1);
                        jaDrp.Add(jFormCont);
                    }

                 

                    data.Add("status", mc.GetMessage("api_output_ok"));
                    data.Add("message", mc.GetMessage("process_success"));
                    data.Add("data", jaDrp);

                }
                else
                {
                    data = new JObject();
                    data.Add("status", mc.GetMessage("api_output_not_ok"));
                    data.Add("message","data not found");
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

        [HttpGet("getDataUser/{p_userid}")]
        public JObject getDataUserdetail(string p_userid)
        {
            var data = new JObject();
            try
            {

                var dtReturn = ldl.getDataUserdetail(p_userid);
                data.Add("status", mc.GetMessage("api_output_ok"));
                data.Add("data", dtReturn);
            }
            catch (Exception ex)
            {
                data.Add("status", mc.GetMessage("api_output_not_ok"));
                data.Add("message", ex.Message);
            }
            return data;
        }


        [HttpGet("getspvfc/ddown")]
        public JObject Getspvfcddown()
        {
            var retObject = new List<dynamic>();
            var data = new JObject();
            try
            {
                data = new JObject();
                retObject = ldl.Getspvfcddown();

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

        [HttpPost("update")]
        public JObject Updatespvfc([FromBody] JObject json)
        {
            var data = new JObject();

            try
            {

                string strout = "";

                strout = bcx.Updatespvfc(json);
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

        [HttpGet("branch/list")]
        public JObject GetListBranch()
        {
            var retObject = new List<dynamic>();
            var data = new JObject();

            var jaFormDet = new JArray();
            try
            {
                data = new JObject();
                JObject jFormCont = new JObject();
                JArray jaDrp = new JArray();
                var dtretrun = ldl.Getlistteambranch();
                if (dtretrun.Count > 0)
                {
                    for (int i = 0; i < dtretrun.Count; i++)
                    {
                        jFormCont = new JObject();
                        var jFormDetData = new JObject();
                        var brcode = dtretrun[i]["lbrc_code"].ToString();
                        var dtretrun1 = ldl.Getuserteambranchbycode(brcode);


                        jFormCont.Add("id", dtretrun[i]["lbrc_id"].ToString());
                        jFormCont.Add("code", dtretrun[i]["lbrc_code"].ToString());
                        jFormCont.Add("name", dtretrun[i]["lbrc_name"].ToString());
                        jFormCont.Add("agent", dtretrun1);
                        jaDrp.Add(jFormCont);
                    }



                    data.Add("status", mc.GetMessage("api_output_ok"));
                    data.Add("message", mc.GetMessage("process_success"));
                    data.Add("data", jaDrp);

                }
                else
                {
                    data = new JObject();
                    data.Add("status", mc.GetMessage("api_output_not_ok"));
                    data.Add("message", "data not found");
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
    }
}
