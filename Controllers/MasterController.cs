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
    public class MasterController : Controller
    {
        private BaseController bc = new BaseController();
        private BasetrxController bcx = new BasetrxController();
        private lDbConn dbconn = new lDbConn();
        private MessageController mc = new MessageController();
        private TokenController tc = new TokenController();
        private lConvert lc = new lConvert();
        private lDataLayer ldl = new lDataLayer();
        

        [HttpGet("list")]
        public JObject GetList()
        {


            var retObject = new List<dynamic>();
            var data = new JObject();
            try
            {
                data = new JObject();
                var provider = dbconn.sqlprovider();
                var cstrname = dbconn.constringName("skycoll");
                string spname = "version_crud.get_smscontent_list";

                retObject = bc.getDataToObject(provider, cstrname, spname);
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

        [HttpGet("list/active")]
        public JObject GetListActive()
        {


            var retObject = new List<dynamic>();
            var data = new JObject();
            try
            {
                data = new JObject();
                var provider = dbconn.sqlprovider();
                var cstrname = dbconn.constringName("skycoll");
                string spname = "version_crud.get_smscontent_list_active";

                retObject = bc.getDataToObject(provider, cstrname, spname);
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
        public JObject GetDetail([FromBody] JObject json)
        {


            var retObject = new List<dynamic>();
            var data = new JObject();
            try
            {
                data = new JObject();
                var ids = Int32.Parse(json.GetValue("id").ToString());
                retObject = ldl.GetDetailColl(ids);
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

        [HttpPost("insert")]
        public JObject Post([FromBody] JObject json)
        {

            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            var cstrname_en = dbconn.constringName("skyen");
            // check version 

            var current_date = DateTime.Now.ToString("yy");
            var version = current_date + "." + "0001";


            string spname = "version_crud.insert_smscontent";
            string p1 = "p_code|" + json.GetValue("code") + "|s";
            string p2 = "p_name|" + json.GetValue("name") + "|s";
            string p3 = "p_content|" + json.GetValue("content") + "|s";
            string p4 = "p_day|" + json.GetValue("day") + "|s";
            string p5 = "p_user|" + json.GetValue("user") + "|s";            
            string p6 = "p_version|" + version + "|s";
            string p7 = "p_idori|" + "0" + "|bg";
            var data = new JObject();
            try
            {
                var retObject = new List<dynamic>();
                var jaData = new JArray();
                //collect detail info score card by code
                retObject = ldl.CheckSmsContentByCode(json.GetValue("code").ToString());
                jaData = lc.convertDynamicToJArray(retObject);

                data = new JObject();

                if (jaData.Count > 0)
                {
                    data.Add("status", mc.GetMessage("api_output_not_ok"));
                    data.Add("message", "code " + json.GetValue("name").ToString() + " Already Exist.");
                }
                else
                {
                    bc.execSqlWithSplitPipeline(provider, cstrname, spname, p1, p2, p3, p4, p5, p6, p7);

                    //collect detail info score card by code
                    retObject = ldl.CheckSmsContentByCode(json.GetValue("code").ToString()); 
                    jaData = lc.convertDynamicToJArray(retObject);

                    for (int i = 0; i < jaData.Count; i++)
                    {
                        // insert master data log

                        var jsonData = new JObject();
                        jsonData.Add("id", jaData[i]["lsc_id"].ToString());
                        jsonData.Add("lsc_code", jaData[i]["lsc_code"].ToString());
                        jsonData.Add("lsc_name", jaData[i]["lsc_name"].ToString());
                        jsonData.Add("lsc_content", jaData[i]["lsc_content"].ToString());
                        jsonData.Add("lsc_day", jaData[i]["lsc_day"].ToString());
                        jsonData.Add("lsc_is_active", jaData[i]["lsc_is_active"].ToString());
                        jsonData.Add("is_active", jaData[i]["lsc_is_active"].ToString());
                        jsonData.Add("created_by", jaData[i]["lsc_created_by"].ToString());
                        jsonData.Add("modified_by", jaData[i]["lsc_modified_by"].ToString());
                        jsonData.Add("approved_status", jaData[i]["lsc_approved_status"].ToString());
                        jsonData.Add("p_user", json.GetValue("user"));
                        jsonData.Add("p_logid", json.GetValue("p_logid"));
                        jsonData.Add("p_version", version);

                        retObject = this.InsertLog(jsonData, "INSERT");
                    }

                    string spname1 = "masters.insert_parameter_versions";
                    string p11 = "p_module|" + "smscontent" + "|s";
                    string p12 = "p_version|" + version + "|s";
                    bc.execSqlWithSplitPipeline(provider, cstrname_en, spname1, p11, p12);

                    data.Add("status", mc.GetMessage("api_output_ok"));
                    data.Add("message", mc.GetMessage("save_success"));
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

        [HttpPost("update")]
        public JObject PostUpdate([FromBody] JObject json)
        {
            var retObject = new List<dynamic>();
            var jaData = new JArray();
            var data = new JObject();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            var cstrname_en = dbconn.constringName("skyen");

            //check id param_version
            var jaRetrune1 = new JArray();
            var jaObject1 = new List<dynamic>();
            jaObject1 = ldl.Checkversion(json.GetValue("id").ToString(), "smscontent");
            jaRetrune1 = lc.convertDynamicToJArray(jaObject1);
            var version = jaRetrune1[0]["versions"].ToString();

            // check id param_version
            var jaRetrune = new JArray();
            var jaObject = new List<dynamic>();

            jaObject = ldl.Checkdataparamversion(json.GetValue("id").ToString(), "smscontent");
            jaData = lc.convertDynamicToJArray(jaObject);
            var checkdata = jaData[0]["retrundata"].ToString();

            if (checkdata == "0")
            {
                string spname = "version_crud.insert_smscontent_update";
                string p1 = "p_code|" + json.GetValue("code") + "|s";
                string p2 = "p_name|" + json.GetValue("name") + "|s";
                string p3 = "p_content|" + json.GetValue("content") + "|s";
                string p4 = "p_day|" + json.GetValue("day") + "|s";
                string p5 = "p_user|" + json.GetValue("user") + "|s";
                string p6 = "p_version|" + version + "|s";
                string p7 = "p_idori|" + json.GetValue("id").ToString() + "|bg";

                try
                {
                    //collect detail info score card by code
                    retObject = ldl.CheckSmsContentByCode(json.GetValue("code").ToString());
                    jaData = lc.convertDynamicToJArray(retObject);

                    data = new JObject();
                    bc.execSqlWithSplitPipeline(provider, cstrname, spname, p1, p2, p3, p4, p5, p6, p7);

                    for (int i = 0; i < jaData.Count; i++)
                    {
                        // insert master data log

                        var jsonData = new JObject();
                        jsonData.Add("id", jaData[i]["lsc_id"].ToString());
                        jsonData.Add("lsc_code", jaData[i]["lsc_code"].ToString());
                        jsonData.Add("lsc_name", jaData[i]["lsc_name"].ToString());
                        jsonData.Add("lsc_content", jaData[i]["lsc_content"].ToString());
                        jsonData.Add("lsc_day", jaData[i]["lsc_day"].ToString());
                        jsonData.Add("lsc_is_active", jaData[i]["lsc_is_active"].ToString());
                        jsonData.Add("is_active", jaData[i]["lsc_is_active"].ToString());
                        jsonData.Add("created_by", jaData[i]["lsc_created_by"].ToString());
                        jsonData.Add("modified_by", jaData[i]["lsc_modified_by"].ToString());
                        jsonData.Add("approved_status", jaData[i]["lsc_approved_status"].ToString());
                        jsonData.Add("user", json.GetValue("user"));
                        jsonData.Add("p_logid", json.GetValue("p_logid"));
                        jsonData.Add("p_version", version);

                        retObject = this.InsertLog(jsonData, "INSERT");
                    }


                    string spname2 = "masters.insert_update_parameter_versions";
                    string p14 = "p_id|" + json.GetValue("id").ToString() + "|s";
                    string p15 = "p_module|" + "smscontent" + "|s";
                    string p16 = "p_version|" + version + "|s";
                    bc.execSqlWithSplitPipeline(provider, cstrname_en, spname2, p14, p15, p16);


                    data.Add("status", mc.GetMessage("api_output_ok"));
                    data.Add("message", mc.GetMessage("save_success"));
                    //}
                }
                catch (Exception ex)
                {
                    data = new JObject();
                    data.Add("status", mc.GetMessage("api_output_not_ok"));
                    data.Add("message", ex.Message);
                }
            }

            else
            {
                //collect detail info score card by code
                retObject = ldl.CheckSmsContentByCode(json.GetValue("code").ToString());
                jaData = lc.convertDynamicToJArray(retObject);

                if (jaData.Count > 0)
                {
                    for (int i = 0; i < jaData.Count; i++)
                    {
                        // insert master data log

                        var jsonData = new JObject();
                        jsonData.Add("id", jaData[i]["lsc_id"].ToString());
                        jsonData.Add("lsc_code", jaData[i]["lsc_code"].ToString());
                        jsonData.Add("lsc_name", jaData[i]["lsc_name"].ToString());
                        jsonData.Add("lsc_content", jaData[i]["lsc_content"].ToString());
                        jsonData.Add("lsc_day", jaData[i]["lsc_day"].ToString());
                        jsonData.Add("lsc_is_active", jaData[i]["lsc_is_active"].ToString());
                        jsonData.Add("is_active", jaData[i]["lsc_is_active"].ToString());
                        jsonData.Add("created_by", jaData[i]["lsc_created_by"].ToString());
                        jsonData.Add("modified_by", jaData[i]["lsc_modified_by"].ToString());
                        jsonData.Add("approved_status", jaData[i]["lsc_approved_status"].ToString());
                        jsonData.Add("user", json.GetValue("user"));
                        jsonData.Add("p_logid", json.GetValue("p_logid"));
                        jsonData.Add("p_version", json.GetValue("version"));
                        retObject = this.InsertLog(jsonData, "BEFORE UPDATE");
                    }
                }

                string spname = "version_crud.update_sms_content";
                string p1 = "p_id|" + Int32.Parse(json.GetValue("id").ToString()) + "|bg";
                string p2 = "p_code|" + json.GetValue("code") + "|s";
                string p3 = "p_name|" + json.GetValue("name") + "|s";
                string p4 = "p_content|" + json.GetValue("content") + "|s";
                string p5 = "p_day|" + json.GetValue("day") + "|s";
                string p6 = "p_user|" + json.GetValue("user") + "|s";
                string p7 = "p_version|" + json.GetValue("version") + "|s";

                try
                {
                    data = new JObject();
                    bc.execSqlWithSplitPipeline(provider, cstrname, spname, p1, p2, p3, p4, p5, p6, p7);

                    //collect detail info score card by code
                    retObject = ldl.CheckSmsContentByCode(json.GetValue("code").ToString());
                    jaData = lc.convertDynamicToJArray(retObject);

                    if (jaData.Count > 0)
                    {
                        for (int i = 0; i < jaData.Count; i++)
                        {
                            // insert master data log

                            var jsonData = new JObject();
                            jsonData.Add("id", jaData[i]["lsc_id"].ToString());
                            jsonData.Add("lsc_code", jaData[i]["lsc_code"].ToString());
                            jsonData.Add("lsc_name", jaData[i]["lsc_name"].ToString());
                            jsonData.Add("lsc_content", jaData[i]["lsc_content"].ToString());
                            jsonData.Add("lsc_day", jaData[i]["lsc_day"].ToString());
                            jsonData.Add("lsc_is_active", jaData[i]["lsc_is_active"].ToString());
                            jsonData.Add("is_active", jaData[i]["lsc_is_active"].ToString());
                            jsonData.Add("created_by", jaData[i]["lsc_created_by"].ToString());
                            jsonData.Add("modified_by", jaData[i]["lsc_modified_by"].ToString());
                            jsonData.Add("approved_status", jaData[i]["lsc_approved_status"].ToString());
                            jsonData.Add("user", json.GetValue("user"));
                            jsonData.Add("p_logid", json.GetValue("p_logid"));
                            jsonData.Add("p_version", json.GetValue("version"));
                            retObject = InsertLog(jsonData, "AFTER UPDATE");
                        }
                    }

                    string spname2 = "masters.update_parameter_versions";
                    string p12 = "p_id|" + json.GetValue("id").ToString() + "|s";
                    string p13 = "p_module|" + "smscontent" + "|s";
                    string p14 = "p_status|" + "updated" + "|s";
                    bc.execSqlWithSplitPipeline(provider, cstrname_en, spname2, p12, p13, p14);

                    data.Add("status", mc.GetMessage("api_output_ok"));
                    data.Add("message", mc.GetMessage("update_success"));

                }
                catch (Exception ex)
                {
                    data = new JObject();
                    data.Add("status", mc.GetMessage("api_output_not_ok"));
                    data.Add("message", ex.Message);
                }

            }

            return data;
        }

        [HttpPost("smscontent/approval")]
        public JObject ApprovalSmsContent([FromBody] JObject json)
        {
            var data = new JObject();

            try
            {

                string strout = "";

                strout = bcx.UpdateSmsContentApprovalStatus(json);
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
        public JObject DeleteRows([FromBody] JObject json)
        {
            var retObject = new List<dynamic>();
            var data = new JObject();
            // check id param_version
            var jaRetrune = new JArray();
            var jaObject = new List<dynamic>();
            jaObject = ldl.Checkdataparamversion(json.GetValue("id").ToString(), "smscontent");
            jaRetrune = lc.convertDynamicToJArray(jaObject);
            var checkdata = jaRetrune[0]["retrundata"].ToString();
            if (checkdata != "0")
            {
                retObject = ldl.GetSmsContentById(json.GetValue("id").ToString());
                string spname = "";
                if (retObject[0].lsc_is_used == true)
                {
                    spname = "version_crud.delete_soft_smscontent";
                }
                else
                {
                    spname = "version_crud.delete_hard_smscontent";
                }
                string p1 = "p_id," + json.GetValue("id").ToString() + ",bg";

                try
                {

                    var jaData = new JArray();
                    //collect detail info master data by code
                    retObject = ldl.GetSmsContentById(json.GetValue("id").ToString());
                    jaData = lc.convertDynamicToJArray(retObject);

                    if (jaData.Count > 0)
                    {
                        for (int i = 0; i < jaData.Count; i++)
                        {
                            // insert master data log
                            var jsonData = new JObject();
                            jsonData.Add("id", jaData[i]["lsc_id"].ToString());
                            jsonData.Add("lsc_code", jaData[i]["lsc_code"].ToString());
                            jsonData.Add("lsc_name", jaData[i]["lsc_name"].ToString());
                            jsonData.Add("lsc_content", jaData[i]["lsc_content"].ToString());
                            jsonData.Add("lsc_day", jaData[i]["lsc_day"].ToString());
                            jsonData.Add("lsc_is_active", jaData[i]["lsc_is_active"].ToString());
                            jsonData.Add("is_active", jaData[i]["lsc_is_active"].ToString());
                            jsonData.Add("created_by", jaData[i]["lsc_created_by"].ToString());
                            jsonData.Add("modified_by", jaData[i]["lsc_modified_by"].ToString());
                            jsonData.Add("approved_status", jaData[i]["lsc_approved_status"].ToString());
                            jsonData.Add("user", json.GetValue("user"));
                            jsonData.Add("p_logid", json.GetValue("p_logid"));
                            jsonData.Add("p_version", jaData[i]["lsc_version"].ToString());


                            retObject = InsertLog(jsonData, "BEFORE DELETE");
                        }
                    }

                    data = new JObject();
                    var provider = dbconn.sqlprovider();
                    var cstrname = dbconn.constringName("skycoll");
                    var cstrname_en = dbconn.constringName("skyen");

                    bc.getDataToObject(provider, cstrname, spname, p1);

                    string spname2 = "masters.delete_version_smscontent";
                    string p12 = "p_id|" + json.GetValue("id").ToString() + "|s";
                    
                    bc.execSqlWithSplitPipeline(provider, cstrname_en, spname2, p12);

                    data.Add("status", mc.GetMessage("api_output_ok"));
                    data.Add("message", mc.GetMessage("delete_success"));
                }
                catch (Exception ex)
                {
                    data = new JObject();
                    data.Add("status", mc.GetMessage("api_output_not_ok"));
                    data.Add("message", ex.Message);
                }
            }
            else
            {
                data.Add("status", mc.GetMessage("api_output_not_ok"));
                data.Add("message", "the data is already used");
            }

            return data;
        }

        private List<dynamic> InsertLog(JObject data, String action)
        {

            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");

            string spname = "log.get_coll_insert_log";
            string p1 = "p_id," + data.GetValue("id").ToString() + ",bg";
            string p2 = "p_code," + data.GetValue("lsc_code").ToString() + ",s";
            string p3 = "p_name," + data.GetValue("lsc_name").ToString() + ",s";
            string p4 = "p_content," + data.GetValue("lsc_content") + ",s";
            string p5 = "p_day," + data.GetValue("lsc_day") + ",s";
            string p6 = "p_isactive," + data.GetValue("lsc_is_active") + ",s";
            string p7 = "p_createdby," + data.GetValue("created_by") + ",s";
            string p8 = "p_updateby," + data.GetValue("modified_by") + ",s";
            string p9 = "p_approval_status," + data.GetValue("approved_status") + ",s";
            string p10 = "p_user," + bc.CheckValueData(data, "user") + ",s";
            string p11 = "p_logid," + bc.CheckValueData(data, "p_logid") + ",s";
            string p12 = "p_action," + action + ",s";
            string p13 = "p_version," + data.GetValue("p_version") + ",s";
            return bc.getDataToObject(provider, cstrname, spname, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13);

        }

        #region CallScript

        [HttpGet("list/callscript")]
        public JObject GetListCallScript()
        {


            var retObject = new List<dynamic>();
            var data = new JObject();
            try
            {
                data = new JObject();
                var provider = dbconn.sqlprovider();
                var cstrname = dbconn.constringName("skycoll");
                string spname = "version_crud.get_callscript_list";

                retObject = bc.getDataToObject(provider, cstrname, spname);
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

        [HttpGet("listactive/callscript")]
        public JObject GetListActiveCallScript()
        {


            var retObject = new List<dynamic>();
            var data = new JObject();
            try
            {
                data = new JObject();
                var provider = dbconn.sqlprovider();
                var cstrname = dbconn.constringName("skycoll");
                string spname = "version_crud.get_callscript_list_active";

                retObject = bc.getDataToObject(provider, cstrname, spname);
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

        [HttpPost("detail/callscript")]
        public JObject GetDetailCallScript([FromBody] JObject json)
        {


            var retObject = new List<dynamic>();
            var data = new JObject();
            try
            {
                data = new JObject();
                var ids = json.GetValue("id").ToString();
                retObject = ldl.GetCallScriptDetail(ids);
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

        [HttpPost("insert/callscript")]
        public JObject InsertCallScriptData([FromBody] JObject json)
        {

            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            var cstrname_en = dbconn.constringName("skyen");
            // check version 

            var current_date = DateTime.Now.ToString("yy");
            var version = current_date + "." + "0001";


            string spname = "version_crud.insert_callscript";
            string p1 = "p_code|" + json.GetValue("code") + "|s";
            string p2 = "p_desc|" + json.GetValue("desc") + "|s";
            string p3 = "p_accd_min|" + json.GetValue("csc_accd_min") + "|i";
            string p4 = "p_accd_max|" + json.GetValue("csc_accd_max") + "|i";
            string p5 = "p_script|" + json.GetValue("script") + "|s";
            string p6 = "p_user|" + json.GetValue("user") + "|s";
            string p7 = "p_version|" + version + "|s";
            string p8 = "p_idori|" + "0" + "|bg";
            var data = new JObject();
            try
            {
                var retObject = new List<dynamic>();
                var jaData = new JArray();
                //collect detail info score card by code
                retObject = ldl.GetCallScriptDetailByCode(json.GetValue("code").ToString());
                jaData = lc.convertDynamicToJArray(retObject);

                data = new JObject();

                if (jaData.Count > 0)
                {
                    data.Add("status", mc.GetMessage("api_output_not_ok"));
                    data.Add("message", "code " + json.GetValue("code").ToString() + " Already Exist.");
                }
                else
                {
                    bc.execSqlWithSplitPipeline(provider, cstrname, spname, p1, p2, p3, p4, p5, p6, p7,p8);

                    //collect detail info score card by code
                    retObject = ldl.GetCallScriptDetailByCode(json.GetValue("code").ToString());
                    jaData = lc.convertDynamicToJArray(retObject);

                    for (int i = 0; i < jaData.Count; i++)
                    {
                        // insert master data log

                        var jsonData = new JObject();
                        jsonData.Add("id", jaData[i]["csc_id"].ToString());
                        jsonData.Add("csc_code", jaData[i]["csc_code"].ToString());
                        jsonData.Add("csc_desc", jaData[i]["csc_desc"].ToString());
                        jsonData.Add("csc_accd_min", jaData[i]["csc_accd_min"].ToString());
                        jsonData.Add("csc_accd_max", jaData[i]["csc_accd_max"].ToString());
                        jsonData.Add("script", jaData[i]["csc_cs_script"].ToString());
                        jsonData.Add("is_active", jaData[i]["csc_is_active"].ToString());
                        jsonData.Add("created_by", jaData[i]["csc_created_by"].ToString());
                        jsonData.Add("modified_by", jaData[i]["csc_modified_by"].ToString());
                        jsonData.Add("approved_status", jaData[i]["csc_approved_status"].ToString());
                        jsonData.Add("user", json.GetValue("p_user"));
                        jsonData.Add("p_logid", json.GetValue("p_logid"));
                        jsonData.Add("p_version", version);

                        retObject = this.InsertLogCallScript(jsonData, "INSERT");
                    }

                    string spname1 = "masters.insert_parameter_versions_callscript";
                    string p11 = "p_module|" + "callscript" + "|s";
                    string p12 = "p_version|" + version + "|s";
                    bc.execSqlWithSplitPipeline(provider, cstrname_en, spname1, p11, p12);

                    data.Add("status", mc.GetMessage("api_output_ok"));
                    data.Add("message", mc.GetMessage("save_success"));
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

        [HttpPost("update/callscript")]
        public JObject UpdateDataCallScript([FromBody] JObject json)
        {
            var retObject = new List<dynamic>();
            var jaData = new JArray();
            var data = new JObject();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            var cstrname_en = dbconn.constringName("skyen");

            //check id param_version
            var jaRetrune1 = new JArray();
            var jaObject1 = new List<dynamic>();
            jaObject1 = ldl.Checkversion(json.GetValue("id").ToString(), "callscript");
            jaRetrune1 = lc.convertDynamicToJArray(jaObject1);
            var version = jaRetrune1[0]["versions"].ToString();

            // check id param_version
            var jaRetrune = new JArray();
            var jaObject = new List<dynamic>();

            jaObject = ldl.Checkdataparamversion(json.GetValue("id").ToString(), "callscript");
            jaData = lc.convertDynamicToJArray(jaObject);
            var checkdata = jaData[0]["retrundata"].ToString();

            if (checkdata == "0")
            {
                string spname = "version_crud.insert_callscript_update";
                string p1 = "p_code|" + json.GetValue("code") + "|s";
                string p2 = "p_desc|" + json.GetValue("desc") + "|s";
                string p3 = "p_accd_min|" + json.GetValue("csc_accd_min") + "|i";
                string p4 = "p_accd_max|" + json.GetValue("csc_accd_max") + "|i";
                string p5 = "p_script|" + json.GetValue("script") + "|s";
                string p6 = "p_user|" + json.GetValue("user") + "|s";
                string p7 = "p_version|" + version + "|s";
                string p8 = "p_idori|" + json.GetValue("id").ToString() + "|bg";

                try
                {
                    //collect detail info score card by code
                    retObject = ldl.GetCallScriptDetailByCode(json.GetValue("code").ToString());
                    jaData = lc.convertDynamicToJArray(retObject);

                    data = new JObject();
                    bc.execSqlWithSplitPipeline(provider, cstrname, spname, p1, p2, p3, p4, p5, p6, p7,p8);

                    for (int i = 0; i < jaData.Count; i++)
                    {
                        // insert master data log

                        var jsonData = new JObject();
                        jsonData.Add("id", jaData[i]["csc_id"].ToString());
                        jsonData.Add("csc_code", jaData[i]["csc_code"].ToString());
                        jsonData.Add("csc_desc", jaData[i]["csc_desc"].ToString());
                        jsonData.Add("csc_accd_min", jaData[i]["csc_accd_min"].ToString());
                        jsonData.Add("csc_accd_max", jaData[i]["csc_accd_max"].ToString());
                        jsonData.Add("script", jaData[i]["csc_cs_script"].ToString());
                        jsonData.Add("is_active", jaData[i]["csc_is_active"].ToString());
                        jsonData.Add("created_by", jaData[i]["csc_created_by"].ToString());
                        jsonData.Add("modified_by", jaData[i]["csc_modified_by"].ToString());
                        jsonData.Add("approved_status", jaData[i]["csc_approved_status"].ToString());
                        jsonData.Add("user", json.GetValue("user"));
                        jsonData.Add("p_logid", json.GetValue("logid"));
                        jsonData.Add("p_version", version);

                        retObject = this.InsertLogCallScript(jsonData, "INSERT");
                    }


                    string spname2 = "masters.insert_update_parameter_versions";
                    string p14 = "p_id|" + json.GetValue("id").ToString() + "|s";
                    string p15 = "p_module|" + "callscript" + "|s";
                    string p16 = "p_version|" + version + "|s";
                    bc.execSqlWithSplitPipeline(provider, cstrname_en, spname2, p14, p15, p16);


                    data.Add("status", mc.GetMessage("api_output_ok"));
                    data.Add("message", mc.GetMessage("save_success"));
                    //}
                }
                catch (Exception ex)
                {
                    data = new JObject();
                    data.Add("status", mc.GetMessage("api_output_not_ok"));
                    data.Add("message", ex.Message);
                }
            }

            else
            {
                //collect detail info score card by code
                retObject = ldl.GetCallScriptDetailByCode(json.GetValue("code").ToString());
                jaData = lc.convertDynamicToJArray(retObject);

                if (jaData.Count > 0)
                {
                    for (int i = 0; i < jaData.Count; i++)
                    {
                        // insert master data log

                        var jsonData = new JObject();
                        jsonData.Add("id", jaData[i]["csc_id"].ToString());
                        jsonData.Add("csc_code", jaData[i]["csc_code"].ToString());
                        jsonData.Add("csc_desc", jaData[i]["csc_desc"].ToString());
                        jsonData.Add("csc_accd_min", jaData[i]["csc_accd_min"].ToString());
                        jsonData.Add("csc_accd_max", jaData[i]["csc_accd_max"].ToString());
                        jsonData.Add("script", jaData[i]["csc_cs_script"].ToString());
                        jsonData.Add("is_active", jaData[i]["csc_is_active"].ToString());
                        jsonData.Add("created_by", jaData[i]["csc_created_by"].ToString());
                        jsonData.Add("modified_by", jaData[i]["csc_modified_by"].ToString());
                        jsonData.Add("approved_status", jaData[i]["csc_approved_status"].ToString());
                        jsonData.Add("user", json.GetValue("user"));
                        jsonData.Add("p_logid", json.GetValue("p_logid"));
                        jsonData.Add("p_version", json.GetValue("version"));

                        retObject = this.InsertLogCallScript(jsonData, "BEFORE UPDATE");
                    }
                }

                string spname = "version_crud.update_callscript";
                string p1 = "p_id|" + Int32.Parse(json.GetValue("id").ToString()) + "|bg";
                string p2 = "p_code|" + json.GetValue("code") + "|s";
                string p3 = "p_desc|" + json.GetValue("desc") + "|s";
                string p4 = "p_accd_min|" + json.GetValue("csc_accd_min") + "|i";
                string p5 = "p_accd_max|" + json.GetValue("csc_accd_max") + "|i";
                string p6 = "p_script|" + json.GetValue("script") + "|s";
                string p7 = "p_user|" + json.GetValue("user") + "|s";
                string p8 = "p_version|" + json.GetValue("version") + "|s";

                try
                {
                    data = new JObject();
                    bc.execSqlWithSplitPipeline(provider, cstrname, spname, p1, p2, p3, p4, p5, p6, p7,p8);

                    //collect detail info score card by code
                    retObject = ldl.GetCallScriptDetailByCode(json.GetValue("code").ToString());
                    jaData = lc.convertDynamicToJArray(retObject);

                    if (jaData.Count > 0)
                    {
                        for (int i = 0; i < jaData.Count; i++)
                        {
                            // insert master data log

                            var jsonData = new JObject();
                            jsonData.Add("id", jaData[i]["csc_id"].ToString());
                            jsonData.Add("csc_code", jaData[i]["csc_code"].ToString());
                            jsonData.Add("csc_desc", jaData[i]["csc_desc"].ToString());
                            jsonData.Add("csc_accd_min", jaData[i]["csc_accd_min"].ToString());
                            jsonData.Add("csc_accd_max", jaData[i]["csc_accd_max"].ToString());
                            jsonData.Add("script", jaData[i]["csc_cs_script"].ToString());
                            jsonData.Add("is_active", jaData[i]["csc_is_active"].ToString());
                            jsonData.Add("created_by", jaData[i]["csc_created_by"].ToString());
                            jsonData.Add("modified_by", jaData[i]["csc_modified_by"].ToString());
                            jsonData.Add("approved_status", jaData[i]["csc_approved_status"].ToString());
                            jsonData.Add("user", json.GetValue("user"));
                            jsonData.Add("p_logid", json.GetValue("p_logid"));
                            jsonData.Add("p_version", json.GetValue("version"));

                            retObject = InsertLogCallScript(jsonData, "AFTER UPDATE");
                        }
                    }

                    string spname2 = "masters.update_parameter_versions";
                    string p12 = "p_id|" + json.GetValue("id").ToString() + "|s";
                    string p13 = "p_module|" + "callscript" + "|s";
                    string p14 = "p_status|" + "updated" + "|s";
                    bc.execSqlWithSplitPipeline(provider, cstrname_en, spname2, p12, p13, p14);

                    data.Add("status", mc.GetMessage("api_output_ok"));
                    data.Add("message", mc.GetMessage("update_success"));

                }
                catch (Exception ex)
                {
                    data = new JObject();
                    data.Add("status", mc.GetMessage("api_output_not_ok"));
                    data.Add("message", ex.Message);
                }

            }

            return data;
        }

        [HttpPost("callscript/approval")]
        public JObject UpdateApprovalCallScript([FromBody] JObject json)
        {
            var data = new JObject();

            try
            {

                string strout = "";

                strout = bcx.UpdateCallScriptApprovalStatus(json);
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

        [HttpPost("delete/callscript")]
        public JObject DeleteRowsCallScript([FromBody] JObject json)
        {
            var retObject = new List<dynamic>();
            var data = new JObject();
            // check id param_version
            var jaRetrune = new JArray();
            var jaObject = new List<dynamic>();
            jaObject = ldl.Checkdataparamversion(json.GetValue("id").ToString(), "callscript");
            jaRetrune = lc.convertDynamicToJArray(jaObject);
            var checkdata = jaRetrune[0]["retrundata"].ToString();
            if (checkdata != "0")
            {
                retObject = ldl.GetCallScriptDetailById(json.GetValue("id").ToString());
                string spname = "";
                if (retObject[0].csc_is_used == true)
                {
                    spname = "version_crud.delete_soft_callscript";
                }
                else
                {
                    spname = "version_crud.delete_hard_callscript";
                }
                string p1 = "p_id," + json.GetValue("id").ToString() + ",bg";

                try
                {

                    var jaData = new JArray();
                    //collect detail info master data by code
                    retObject = ldl.GetCallScriptDetailById(json.GetValue("id").ToString());
                    jaData = lc.convertDynamicToJArray(retObject);

                    if (jaData.Count > 0)
                    {
                        for (int i = 0; i < jaData.Count; i++)
                        {
                            // insert master data log
                            var jsonData = new JObject();
                            jsonData.Add("id", jaData[i]["csc_id"].ToString());
                            jsonData.Add("csc_code", jaData[i]["csc_code"].ToString());
                            jsonData.Add("csc_desc", jaData[i]["csc_desc"].ToString());
                            jsonData.Add("csc_accd_min", jaData[i]["csc_accd_min"].ToString());
                            jsonData.Add("csc_accd_max", jaData[i]["csc_accd_max"].ToString());
                            jsonData.Add("script", jaData[i]["csc_cs_script"].ToString());
                            jsonData.Add("is_active", jaData[i]["csc_is_active"].ToString());
                            jsonData.Add("created_by", jaData[i]["csc_created_by"].ToString());
                            jsonData.Add("modified_by", jaData[i]["csc_modified_by"].ToString());
                            jsonData.Add("approved_status", jaData[i]["csc_approved_status"].ToString());
                            jsonData.Add("user", json.GetValue("user"));
                            jsonData.Add("p_logid", json.GetValue("p_logid"));
                            jsonData.Add("p_version", json.GetValue("version"));


                            retObject = InsertLogCallScript(jsonData, "BEFORE DELETE");
                        }
                    }

                    data = new JObject();
                    var provider = dbconn.sqlprovider();
                    var cstrname = dbconn.constringName("skycoll");
                    var cstrname_en = dbconn.constringName("skyen");

                    bc.getDataToObject(provider, cstrname, spname, p1);

                    string spname2 = "masters.delete_version_callscript";
                    string p12 = "p_id|" + json.GetValue("id").ToString() + "|s";

                    bc.execSqlWithSplitPipeline(provider, cstrname_en, spname2, p12);

                    data.Add("status", mc.GetMessage("api_output_ok"));
                    data.Add("message", mc.GetMessage("delete_success"));
                }
                catch (Exception ex)
                {
                    data = new JObject();
                    data.Add("status", mc.GetMessage("api_output_not_ok"));
                    data.Add("message", ex.Message);
                }
            }
            else
            {
                data.Add("status", mc.GetMessage("api_output_not_ok"));
                data.Add("message", "the data is already used");
            }

            return data;
        }

        private List<dynamic> InsertLogCallScript(JObject data, String action)
        {

            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");

            string spname = "log.get_callscript_insert_log";
            string p1 = "p_id," + data.GetValue("id").ToString() + ",bg";
            string p2 = "p_code," + data.GetValue("csc_code").ToString() + ",s";
            string p3 = "p_desc," + data.GetValue("csc_desc").ToString() + ",s";
            string p4 = "p_accd_min," + data.GetValue("csc_accd_min") + ",i";
            string p5 = "p_accd_max," + data.GetValue("csc_accd_max") + ",i";
            string p6 = "p_script," + data.GetValue("csc_cs_script") + ",s";
            string p7 = "p_isactive," + data.GetValue("csc_is_active") + ",s";
            string p8 = "p_createdby," + data.GetValue("created_by") + ",s";
            string p9 = "p_updateby," + data.GetValue("modified_by") + ",s";
            string p10 = "p_approval_status," + data.GetValue("approved_status") + ",s";
            string p11 = "p_user," + bc.CheckValueData(data, "user") + ",s";
            string p12 = "p_logid," + bc.CheckValueData(data, "p_logid") + ",s";
            string p13 = "p_action," + action + ",s";
            string p14 = "p_version," + data.GetValue("p_version") + ",s";
            return bc.getDataToObject(provider, cstrname, spname, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14);

        }

        #endregion
        // end Callscript Region

        #region reason
        // Reason region

        [HttpGet("list/reason")]
        public JObject GetListReason()
        {


            var retObject = new List<dynamic>();
            var data = new JObject();
            try
            {
                data = new JObject();
                var provider = dbconn.sqlprovider();
                var cstrname = dbconn.constringName("skycoll");
                string spname = "version_crud.get_reason_list";

                retObject = bc.getDataToObject(provider, cstrname, spname);
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

        [HttpGet("listactive/reason")]
        public JObject GetListActiveReason()
        {


            var retObject = new List<dynamic>();
            var data = new JObject();
            try
            {
                data = new JObject();
                var provider = dbconn.sqlprovider();
                var cstrname = dbconn.constringName("skycoll");
                string spname = "version_crud.get_reason_list_active";

                retObject = bc.getDataToObject(provider, cstrname, spname);
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

        [HttpPost("detail/reason")]
        public JObject GetDetailReason([FromBody] JObject json)
        {


            var retObject = new List<dynamic>();
            var data = new JObject();
            try
            {
                data = new JObject();
                var ids = json.GetValue("id").ToString();
                retObject = ldl.GetReasonDetail(ids);
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

        [HttpPost("insert/reason")]
        public JObject InsertReasonData([FromBody] JObject json)
        {

            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            var cstrname_en = dbconn.constringName("skyen");
            // check version 

            var current_date = DateTime.Now.ToString("yy");
            var version = current_date + "." + "0001";


            string spname = "version_crud.insert_reason";
            string p1 = "p_code|" + json.GetValue("code") + "|s";
            string p2 = "p_name|" + json.GetValue("name") + "|s";
            string p3 = "p_is_dc|" + json.GetValue("rsn_is_dc") + "|i";
            string p4 = "p_is_fc|" + json.GetValue("rsn_is_fc") + "|i";
            string p5 = "p_user|" + json.GetValue("user") + "|s";
            string p6 = "p_version|" + version + "|s";
            string p7 = "p_idori|" + "0" + "|bg";
            var data = new JObject();
            try
            {
                var retObject = new List<dynamic>();
                var jaData = new JArray();
                //collect detail info score card by code
                retObject = ldl.GetReasonDetailByCode(json.GetValue("code").ToString());
                jaData = lc.convertDynamicToJArray(retObject);

                data = new JObject();

                if (jaData.Count > 0)
                {
                    data.Add("status", mc.GetMessage("api_output_not_ok"));
                    data.Add("message", "id" + json.GetValue("name").ToString() + " Already Exist.");
                }
                else
                {
                    bc.execSqlWithSplitPipeline(provider, cstrname, spname, p1, p2, p3, p4, p5, p6, p7);

                    //collect detail info reason by code
                    retObject = ldl.GetReasonDetailByCode(json.GetValue("code").ToString());
                    jaData = lc.convertDynamicToJArray(retObject);

                    for (int i = 0; i < jaData.Count; i++)
                    {
                        // insert master data log

                        var jsonData = new JObject();
                        jsonData.Add("id", jaData[i]["rsn_id"].ToString());
                        jsonData.Add("rsn_code", jaData[i]["rsn_code"].ToString());
                        jsonData.Add("rsn_name", jaData[i]["rsn_name"].ToString());
                        jsonData.Add("rsn_is_dc", jaData[i]["rsn_is_dc"].ToString());
                        jsonData.Add("rsn_is_fc", jaData[i]["rsn_is_fc"].ToString());
                        jsonData.Add("is_active", jaData[i]["rsn_is_active"].ToString());
                        jsonData.Add("created_by", jaData[i]["rsn_created_by"].ToString());
                        jsonData.Add("modified_by", jaData[i]["rsn_modified_by"].ToString());
                        jsonData.Add("approved_status", jaData[i]["rsn_approved_status"].ToString());
                        jsonData.Add("user", json.GetValue("p_user"));
                        jsonData.Add("p_logid", json.GetValue("p_logid"));
                        jsonData.Add("p_version", version);

                        retObject = this.InsertLogReason(jsonData, "INSERT");
                    }

                    string spname1 = "masters.insert_parameter_versions";
                    string p11 = "p_module|" + "reason" + "|s";
                    string p12 = "p_version|" + version + "|s";
                    bc.execSqlWithSplitPipeline(provider, cstrname_en, spname1, p11, p12);

                    data.Add("status", mc.GetMessage("api_output_ok"));
                    data.Add("message", mc.GetMessage("save_success"));
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

        [HttpPost("update/reason")]
        public JObject UpdateDataReason([FromBody] JObject json)
        {
            var retObject = new List<dynamic>();
            var jaData = new JArray();
            var data = new JObject();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            var cstrname_en = dbconn.constringName("skyen");

            //check id param_version
            var jaRetrune1 = new JArray();
            var jaObject1 = new List<dynamic>();
            jaObject1 = ldl.Checkversion(json.GetValue("id").ToString(), "reason");
            jaRetrune1 = lc.convertDynamicToJArray(jaObject1);
            var version = jaRetrune1[0]["versions"].ToString();

            // check id param_version
            var jaRetrune = new JArray();
            var jaObject = new List<dynamic>();

            jaObject = ldl.Checkdataparamversion(json.GetValue("id").ToString(), "reason");
            jaData = lc.convertDynamicToJArray(jaObject);
            var checkdata = jaData[0]["retrundata"].ToString();

            if (checkdata == "0")
            {
                string spname = "version_crud.insert_reason_update";
                string p1 = "p_code|" + json.GetValue("rsn_code") + "|s";
                string p2 = "p_name|" + json.GetValue("rsn_name") + "|s";
                string p3 = "p_is_dc|" + json.GetValue("rsn_is_dc") + "|i";
                string p4 = "p_is_fc|" + json.GetValue("rsn_is_fc") + "|i";
                string p5 = "p_user|" + json.GetValue("user") + "|s";
                string p6 = "p_version|" + version + "|s";
                string p7 = "p_idori|" + json.GetValue("id").ToString() + "|bg";

                try
                {
                    //collect detail info reason by code
                    retObject = ldl.GetReasonDetailByCode(json.GetValue("rsn_code").ToString());
                    jaData = lc.convertDynamicToJArray(retObject);

                    data = new JObject();
                    bc.execSqlWithSplitPipeline(provider, cstrname, spname, p1, p2, p3, p4, p5, p6, p7);

                    for (int i = 0; i < jaData.Count; i++)
                    {
                        // insert reason log

                        var jsonData = new JObject();
                        jsonData.Add("id", jaData[i]["rsn_id"].ToString());
                        jsonData.Add("rsn_code", jaData[i]["rsn_code"].ToString());
                        jsonData.Add("rsn_name", jaData[i]["rsn_name"].ToString());
                        jsonData.Add("rsn_is_dc", jaData[i]["rsn_is_dc"].ToString());
                        jsonData.Add("rsn_is_fc", jaData[i]["rsn_is_fc"].ToString());
                        jsonData.Add("is_active", jaData[i]["rsn_is_active"].ToString());
                        jsonData.Add("created_by", jaData[i]["rsn_created_by"].ToString());
                        jsonData.Add("modified_by", jaData[i]["rsn_modified_by"].ToString());
                        jsonData.Add("approved_status", jaData[i]["rsn_approved_status"].ToString());
                        jsonData.Add("user", json.GetValue("p_user"));
                        jsonData.Add("p_logid", json.GetValue("p_logid"));
                        jsonData.Add("p_version", version);

                        retObject = this.InsertLogReason(jsonData, "INSERT");
                    }


                    string spname2 = "masters.insert_update_parameter_versions";
                    string p14 = "p_id|" + json.GetValue("id").ToString() + "|s";
                    string p15 = "p_module|" + "reason" + "|s";
                    string p16 = "p_version|" + version + "|s";
                    bc.execSqlWithSplitPipeline(provider, cstrname_en, spname2, p14, p15, p16);


                    data.Add("status", mc.GetMessage("api_output_ok"));
                    data.Add("message", mc.GetMessage("save_success"));
                    //}
                }
                catch (Exception ex)
                {
                    data = new JObject();
                    data.Add("status", mc.GetMessage("api_output_not_ok"));
                    data.Add("message", ex.Message);
                }
            }

            else
            {
                //collect detail info reason by code
                retObject = ldl.GetReasonDetailByCode(json.GetValue("rsn_code").ToString());
                jaData = lc.convertDynamicToJArray(retObject);

                if (jaData.Count > 0)
                {
                    for (int i = 0; i < jaData.Count; i++)
                    {
                        // insert reason log

                        var jsonData = new JObject();
                        jsonData.Add("id", jaData[i]["rsn_id"].ToString());
                        jsonData.Add("rsn_code", jaData[i]["rsn_code"].ToString());
                        jsonData.Add("rsn_name", jaData[i]["rsn_name"].ToString());
                        jsonData.Add("rsn_is_dc", jaData[i]["rsn_is_dc"].ToString());
                        jsonData.Add("rsn_is_fc", jaData[i]["rsn_is_fc"].ToString());
                        jsonData.Add("is_active", jaData[i]["rsn_is_active"].ToString());
                        jsonData.Add("created_by", jaData[i]["rsn_created_by"].ToString());
                        jsonData.Add("modified_by", jaData[i]["rsn_modified_by"].ToString());
                        jsonData.Add("approved_status", jaData[i]["rsn_approved_status"].ToString());
                        jsonData.Add("user", json.GetValue("p_user"));
                        jsonData.Add("p_logid", json.GetValue("p_logid"));
                        jsonData.Add("p_version", version);

                        retObject = this.InsertLogReason(jsonData, "BEFORE UPDATE");
                    }
                }

                string spname = "version_crud.update_reason";
                string p1 = "p_id|" + Int32.Parse(json.GetValue("id").ToString()) + "|bg";
                string p2 = "p_code|" + json.GetValue("rsn_code") + "|s";
                string p3 = "p_name|" + json.GetValue("rsn_name") + "|s";
                string p4 = "p_is_dc|" + json.GetValue("rsn_is_dc") + "|i";
                string p5 = "p_is_fc|" + json.GetValue("rsn_is_fc") + "|i";
                string p6 = "p_user|" + json.GetValue("user") + "|s";
                string p7 = "p_version|" + json.GetValue("version") + "|s";

                try
                {
                    data = new JObject();
                    bc.execSqlWithSplitPipeline(provider, cstrname, spname, p1, p2, p3, p4, p5, p6, p7);

                    //collect detail info reason by code
                    retObject = ldl.GetReasonDetailByCode(json.GetValue("rsn_code").ToString());
                    jaData = lc.convertDynamicToJArray(retObject);

                    if (jaData.Count > 0)
                    {
                        for (int i = 0; i < jaData.Count; i++)
                        {
                            // insert reason log

                            var jsonData = new JObject();
                            jsonData.Add("id", jaData[i]["rsn_id"].ToString());
                            jsonData.Add("rsn_code", jaData[i]["rsn_code"].ToString());
                            jsonData.Add("rsn_name", jaData[i]["rsn_name"].ToString());
                            jsonData.Add("rsn_is_dc", jaData[i]["rsn_is_dc"].ToString());
                            jsonData.Add("rsn_is_fc", jaData[i]["rsn_is_fc"].ToString());
                            jsonData.Add("is_active", jaData[i]["rsn_is_active"].ToString());
                            jsonData.Add("created_by", jaData[i]["rsn_created_by"].ToString());
                            jsonData.Add("modified_by", jaData[i]["rsn_modified_by"].ToString());
                            jsonData.Add("approved_status", jaData[i]["rsn_approved_status"].ToString());
                            jsonData.Add("user", json.GetValue("p_user"));
                            jsonData.Add("p_logid", json.GetValue("p_logid"));
                            jsonData.Add("p_version", version);

                            retObject = InsertLogReason(jsonData, "AFTER UPDATE");
                        }
                    }

                    string spname2 = "masters.update_parameter_versions";
                    string p12 = "p_id|" + json.GetValue("id").ToString() + "|s";
                    string p13 = "p_module|" + "reason" + "|s";
                    string p14 = "p_status|" + "updated" + "|s";
                    bc.execSqlWithSplitPipeline(provider, cstrname_en, spname2, p12, p13, p14);

                    data.Add("status", mc.GetMessage("api_output_ok"));
                    data.Add("message", mc.GetMessage("update_success"));

                }
                catch (Exception ex)
                {
                    data = new JObject();
                    data.Add("status", mc.GetMessage("api_output_not_ok"));
                    data.Add("message", ex.Message);
                }

            }

            return data;
        }

        [HttpPost("approval/reason")]
        public JObject UpdateApprovalReason([FromBody] JObject json)
        {
            var data = new JObject();

            try
            {

                string strout = "";

                strout = bcx.UpdateReasonApprovalStatus(json);
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

        [HttpPost("delete/reason")]
        public JObject DeleteRowsReason([FromBody] JObject json)
        {
            var retObject = new List<dynamic>();
            var data = new JObject();
            // check id param_version
            var jaRetrune = new JArray();
            var jaObject = new List<dynamic>();
            jaObject = ldl.Checkdataparamversion(json.GetValue("id").ToString(), "reason");
            jaRetrune = lc.convertDynamicToJArray(jaObject);
            var checkdata = jaRetrune[0]["retrundata"].ToString();
            if (checkdata != "0")
            {
                retObject = ldl.GetReasonDetail(json.GetValue("id").ToString());
                string spname = "";
                if (retObject[0].rsn_is_used == true)
                {
                    spname = "version_crud.delete_soft_reason";
                }
                else
                {
                    spname = "version_crud.delete_hard_reason";
                }
                string p1 = "p_id," + json.GetValue("id").ToString() + ",bg";

                try
                {

                    var jaData = new JArray();
                    //collect detail info reason by code
                    retObject = ldl.GetReasonDetail(json.GetValue("id").ToString());
                    jaData = lc.convertDynamicToJArray(retObject);

                    if (jaData.Count > 0)
                    {
                        for (int i = 0; i < jaData.Count; i++)
                        {
                            // insert reason log
                            var jsonData = new JObject();
                            jsonData.Add("id", jaData[i]["rsn_id"].ToString());
                            jsonData.Add("rsn_code", jaData[i]["rsn_code"].ToString());
                            jsonData.Add("rsn_name", jaData[i]["rsn_name"].ToString());
                            jsonData.Add("rsn_is_dc", jaData[i]["rsn_is_dc"].ToString());
                            jsonData.Add("rsn_is_fc", jaData[i]["rsn_is_fc"].ToString());
                            jsonData.Add("is_active", jaData[i]["rsn_is_active"].ToString());
                            jsonData.Add("created_by", jaData[i]["rsn_created_by"].ToString());
                            jsonData.Add("modified_by", jaData[i]["rsn_modified_by"].ToString());
                            jsonData.Add("approved_status", jaData[i]["rsn_approved_status"].ToString());
                            jsonData.Add("user", json.GetValue("p_user"));
                            jsonData.Add("p_logid", json.GetValue("p_logid"));
                            jsonData.Add("p_version", json.GetValue("version"));


                            retObject = InsertLogReason(jsonData, "BEFORE DELETE");
                        }
                    }

                    data = new JObject();
                    var provider = dbconn.sqlprovider();
                    var cstrname = dbconn.constringName("skycoll");
                    var cstrname_en = dbconn.constringName("skyen");

                    bc.getDataToObject(provider, cstrname, spname, p1);

                    string spname2 = "masters.delete_version_reason";
                    string p12 = "p_id|" + json.GetValue("id").ToString() + "|s";

                    bc.execSqlWithSplitPipeline(provider, cstrname_en, spname2, p12);

                    data.Add("status", mc.GetMessage("api_output_ok"));
                    data.Add("message", mc.GetMessage("delete_success"));
                }
                catch (Exception ex)
                {
                    data = new JObject();
                    data.Add("status", mc.GetMessage("api_output_not_ok"));
                    data.Add("message", ex.Message);
                }
            }
            else
            {
                data.Add("status", mc.GetMessage("api_output_not_ok"));
                data.Add("message", "the data is already used");
            }

            return data;
        }


        private List<dynamic> InsertLogReason(JObject data, String action)
        {

            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");

            string spname = "log.get_reason_insert_log";
            string p1 = "p_id," + data.GetValue("id").ToString() + ",bg";
            string p2 = "p_code," + data.GetValue("rsn_code").ToString() + ",s";
            string p3 = "p_name," + data.GetValue("rsn_name").ToString() + ",s";
            string p4 = "p_is_dc," + data.GetValue("rsn_is_dc") + ",i";
            string p5 = "p_is_fc," + data.GetValue("rsn_is_fc") + ",i";
            string p6 = "p_isactive," + data.GetValue("csc_is_active") + ",s";
            string p7 = "p_createdby," + data.GetValue("created_by") + ",s";
            string p8 = "p_updateby," + data.GetValue("modified_by") + ",s";
            string p9 = "p_approval_status," + data.GetValue("approved_status") + ",s";
            string p10 = "p_user," + bc.CheckValueData(data, "user") + ",s";
            string p11 = "p_logid," + bc.CheckValueData(data, "p_logid") + ",s";
            string p12 = "p_action," + action + ",s";
            string p13 = "p_version," + data.GetValue("p_version") + ",s";
            return bc.getDataToObject(provider, cstrname, spname, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13);

        }



        #endregion
    }
}
