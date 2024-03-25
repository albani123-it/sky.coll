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
    public class ReassignController : Controller
    {
        private BaseController bc = new BaseController();
        private BasetrxController bcx = new BasetrxController();
        private lDbConn dbconn = new lDbConn();
        private MessageController mc = new MessageController();
        private TokenController tc = new TokenController();
        private lConvert lc = new lConvert();
        private lDataLayer ldl = new lDataLayer();


        #region reassign dc
        [HttpPost("reassigdc/list")]
        public JObject Getreassigndc([FromBody] JObject json)
        {
            var retObject = new List<dynamic>();
            var data = new JObject();
            try
            {
                data = new JObject();
                retObject = ldl.Getlistreassigndc(json);

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

        [HttpGet("reassigdc/ddown")]
        public JObject Getreassigndcddown()
        {
            var retObject = new List<dynamic>();
            var data = new JObject();
            try
            {
                data = new JObject();
                retObject = ldl.Getlistreassigndcddwon();

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

        [HttpPost("reassigndc/detail")]
        public JObject Getreassigndcdetail([FromBody] JObject json)
        {
            var retgnrlinfo = new List<dynamic>();
            var retcollateral = new List<dynamic>();
            var retlog = new List<dynamic>();
            var data = new JObject();
            try
            {
                data = new JObject();
                var jaFormDet = new JArray();
                var jFormCont = new JObject();
                retgnrlinfo = ldl.GetDetailGnrlInfo(json);
                var accno = retgnrlinfo[0].acc_no;
                retlog = ldl.GetDetailActivityLog(accno);
                for (int i = 0; i < retgnrlinfo.Count; i++)
                {
                    var jFormDetData = new JObject();
                    jFormDetData.Add("id", retgnrlinfo[0].id);
                    jFormDetData.Add("name", retgnrlinfo[0].name);
                    //jFormDetData.Add("no_cif", retgnrlinfo[0].no_cif);
                    //jFormDetData.Add("norek", retgnrlinfo[0].norek);
                    jFormDetData.Add("no_ktp", retgnrlinfo[0].ktp);
                    jFormDetData.Add("tgl_lahir", retgnrlinfo[0].tgl_lahir);
                    jFormDetData.Add("alamat", retgnrlinfo[0].alamat);
                    jFormDetData.Add("no_tlpn", retgnrlinfo[0].no_tlpn);
                    jFormDetData.Add("no_hp", retgnrlinfo[0].no_hp);
                    jFormDetData.Add("pekerjaan", retgnrlinfo[0].pekerjaan);
                    jFormDetData.Add("startdate", retgnrlinfo[0].startdate);
                    jFormDetData.Add("segement", retgnrlinfo[0].segement);
                    jFormDetData.Add("product", retgnrlinfo[0].product);
                    jFormDetData.Add("jumlah_angsuaran", retgnrlinfo[0].jumlah_angsuaran);
                    jFormDetData.Add("tgl_mulai", retgnrlinfo[0].tgl_mulai);
                    jFormDetData.Add("tgl_jatuh_tempo", retgnrlinfo[0].tgl_jatuh_tempo);
                    jFormDetData.Add("tenor", retgnrlinfo[0].tenor);
                    jFormDetData.Add("plafond", retgnrlinfo[0].plafond);
                    jFormDetData.Add("outstanding", retgnrlinfo[0].outstanding);
                    jFormDetData.Add("kolektabilitas", retgnrlinfo[0].kolektabilitas);
                    jFormDetData.Add("dpd", retgnrlinfo[0].dpd);
                    jFormDetData.Add("tglbayarterakhir", retgnrlinfo[0].tglbayarterakhir);
                    jFormDetData.Add("tunggakanpokok", retgnrlinfo[0].tunggakanpokok);
                    jFormDetData.Add("tunggakanbunga", retgnrlinfo[0].tunggakanbunga);
                    jFormDetData.Add("tunggakandenda", retgnrlinfo[0].tunggakandenda);
                    jFormDetData.Add("tunggakantotal", retgnrlinfo[0].tunggakantotal);
                    jFormDetData.Add("kewajibantotal", retgnrlinfo[0].kewajibantotal);
                    jaFormDet.Add(jFormDetData);

                }

                jFormCont.Add("id", json.GetValue("id"));
                jFormCont.Add("name", retgnrlinfo[0].name);
                jFormCont.Add("no_cif", retgnrlinfo[0].no_cif);
                jFormCont.Add("norek", retgnrlinfo[0].norek);
                jFormCont.Add("genearlinfo", jaFormDet);
                jFormCont.Add("collateral", new JArray());
                jFormCont.Add("activitylog", lc.convertDynamicToJArray(retlog));

                data.Add("status", mc.GetMessage("api_output_ok"));
                data.Add("message", mc.GetMessage("process_success"));
                data.Add("data", new JArray(jFormCont));
            }
            catch (Exception ex)
            {
                data = new JObject();
                data.Add("status", mc.GetMessage("api_output_not_ok"));
                data.Add("message", ex.Message);
            }

            return data;
        }

        [HttpPost("reassigndc/activityhistory/detail")]
        public JObject GetDetailactivityhistory([FromBody] JObject json)
        {


            var retObject = new List<dynamic>();
            var data = new JObject();
            try
            {
                data = new JObject();
                retObject = ldl.GetDetailactivityhistory(json);
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

        [HttpPost("checkpayment/detail")]
        public JObject Getcheckpayment([FromBody] JObject json)
        {
            var retgnrlinfo = new List<dynamic>();
            var retcollateral = new List<dynamic>();
            var retlog = new List<dynamic>();
            var data = new JObject();
            try
            {
                data = new JObject();
                var jaFormDet = new JArray();
                var jaFormDet1 = new JArray();
                var jFormCont = new JObject();
                for (int i = 0; i < 3; i++)
                {
                    if (i == 0)
                    {
                        var jFormDetData = new JObject();
                        jFormDetData.Add("date", "25 JAN 2021");
                        jFormDetData.Add("status", "C");
                        jFormDetData.Add("amount", "1000000");
                        jFormDetData.Add("outstanding", null);
                        jFormDetData.Add("amountDue", null);
                        jFormDetData.Add("endBalance", null);
                        jFormDetData.Add("sorter", null);
                        jaFormDet.Add(jFormDetData);
                    }
                    else if (i == 1)
                    {
                        var jFormDetData = new JObject();
                        jFormDetData.Add("date", "25 FEB 2021");
                        jFormDetData.Add("status", "C");
                        jFormDetData.Add("amount", "2000000");
                        jFormDetData.Add("outstanding", null);
                        jFormDetData.Add("amountDue", null);
                        jFormDetData.Add("endBalance", null);
                        jFormDetData.Add("sorter", null);
                        jaFormDet.Add(jFormDetData);
                    }
                    else
                    {
                        var jFormDetData = new JObject();
                        jFormDetData.Add("date", "25 MAR 2021");
                        jFormDetData.Add("status", "C");
                        jFormDetData.Add("amount", "1000000");
                        jFormDetData.Add("outstanding", null);
                        jFormDetData.Add("amountDue", null);
                        jFormDetData.Add("endBalance", null);
                        jFormDetData.Add("sorter", null);
                        jaFormDet.Add(jFormDetData);
                    }
                }

                for (int i = 0; i < 3; i++)
                {
                    if (i == 0)
                    {
                        var jFormDetData = new JObject();
                        jFormDetData.Add("date", "25 APR 2021");
                        jFormDetData.Add("status", null);
                        jFormDetData.Add("amount", "2000000");
                        jFormDetData.Add("outstanding", null);
                        jFormDetData.Add("amountDue", null);
                        jFormDetData.Add("endBalance", null);
                        jFormDetData.Add("sorter", null);
                        jaFormDet1.Add(jFormDetData);
                    }
                    else if (i == 1)
                    {
                        var jFormDetData = new JObject();
                        jFormDetData.Add("date", "25 MEI 2021");
                        jFormDetData.Add("status", null);
                        jFormDetData.Add("amount", "2000000");
                        jFormDetData.Add("outstanding", null);
                        jFormDetData.Add("amountDue", null);
                        jFormDetData.Add("endBalance", null);
                        jFormDetData.Add("sorter", null);
                        jaFormDet1.Add(jFormDetData);
                    }
                    else 
                    {
                        var jFormDetData = new JObject();
                        jFormDetData.Add("date", "25 JUN 2021");
                        jFormDetData.Add("status", null);
                        jFormDetData.Add("amount", "2000000");
                        jFormDetData.Add("outstanding", null);
                        jFormDetData.Add("amountDue", null);
                        jFormDetData.Add("endBalance", null);
                        jFormDetData.Add("sorter", null);
                        jaFormDet1.Add(jFormDetData);
                    }
                }
                jFormCont.Add("status", true);
                jFormCont.Add("account", jaFormDet);
                jFormCont.Add("payment", jaFormDet1);

                data.Add("status", mc.GetMessage("api_output_ok"));
                data.Add("message", mc.GetMessage("process_success"));
                data.Add("data", new JArray(jFormCont));
            }
            catch (Exception ex)
            {
                data = new JObject();
                data.Add("status", mc.GetMessage("api_output_not_ok"));
                data.Add("message", ex.Message);
            }

            return data;
        }


        [HttpPost("reassigndc/tunggakan/detail")]
        public JObject GetDetailtunggakanbyid([FromBody] JObject json)
        {


            var retObject = new List<dynamic>();
            var data = new JObject();
            try
            {
                data = new JObject();
                retObject = ldl.GetDetailtunggakanbyid(json);
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

        [HttpPost("reassigndc/asign")]
        public JObject GetReassigndc([FromBody] JObject json)
        {
            JObject jo = new JObject();
            var data = new JObject();
            try
            {
                var usrid = json.GetValue("usrid").ToString();

                data = new JObject();
               var dtReturn1 = ldl.checkusercollbyid(usrid,"DC");
                if (dtReturn1.Count > 0)
                {
                    for (int i = 0; i < json.GetValue("detail").Count(); i++)
                    {
                        jo = new JObject();
                        jo = JObject.Parse(json.GetValue("detail")[i].ToString());
                        var loanid = jo.GetValue("loanid").ToString();
                        ldl.reassigndc(usrid, loanid);

                    }
                    data.Add("status", mc.GetMessage("api_output_ok"));
                    data.Add("message", mc.GetMessage("process_success"));
                }
                else
                {
                    data = new JObject();
                    data.Add("status", mc.GetMessage("api_output_not_ok"));
                    data.Add("message", "user not active");
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

        #endregion

        #region reassign fc

        [HttpPost("reassigfc/list")]
        public JObject Getreassignfc([FromBody] JObject json)
        {
            var retObject = new List<dynamic>();
            var data = new JObject();
            try
            {
                data = new JObject();
                retObject = ldl.Getlistreassignfc(json);

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

        [HttpGet("reassigfc/ddown")]
        public JObject Getreassignfcddown()
        {
            var retObject = new List<dynamic>();
            var data = new JObject();
            try
            {
                data = new JObject();
                retObject = ldl.Getlistreassignfcddwon();

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

        [HttpPost("reassignfc/asign")]
        public JObject GetReassignfc([FromBody] JObject json)
        {
            JObject jo = new JObject();
            var data = new JObject();
            try
            {
                var usrid = json.GetValue("usrid").ToString();

                data = new JObject();
                var dtReturn1 = ldl.checkusercollbyid(usrid, "FC");
                if (dtReturn1.Count > 0)
                {
                    for (int i = 0; i < json.GetValue("detail").Count(); i++)
                    {
                        jo = new JObject();
                        jo = JObject.Parse(json.GetValue("detail")[i].ToString());
                        var loanid = jo.GetValue("loanid").ToString();
                        ldl.reassigndc(usrid, loanid);

                    }
                    data.Add("status", mc.GetMessage("api_output_ok"));
                    data.Add("message", mc.GetMessage("process_success"));
                }
                else
                {
                    data = new JObject();
                    data.Add("status", mc.GetMessage("api_output_not_ok"));
                    data.Add("message", "user not active");
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


        #endregion

    }
}
