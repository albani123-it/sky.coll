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

    public class tasklistController : Controller
    {
        private BaseController bc = new BaseController();
        private BasetrxController bcx = new BasetrxController();
        private lDbConn dbconn = new lDbConn();
        private MessageController mc = new MessageController();
        private TokenController tc = new TokenController();
        private lConvert lc = new lConvert();
        private lDataLayer ldl = new lDataLayer();

        [HttpPost("list/dc")]
        public JObject GetListtasklistdc([FromBody] JObject json)
        {
            var retObject = new List<dynamic>();
            var data = new JObject();

            var jaFormDet = new JArray();
            try
            {
                data = new JObject();
                JObject jFormNT = new JObject();
                JObject jFormCont = new JObject();
                JArray jaDrp = new JArray();
                var usr = json.GetValue("userid").ToString();
                var dtretrunNT = ldl.Getlisttasklistdc("NT", usr);
                var dtretrunFUIM = ldl.Getlisttasklistdc("FUIM", usr);
                var dtretrunLL = ldl.Getlisttasklistdc("LL", usr);
                var dtretrunMESS = ldl.Getlisttasklistdc("MESS", usr);
                var dtretrunNOAS = ldl.Getlisttasklistdc("NOAS", usr);
                var dtretrunNOAS1 = ldl.Getlisttasklistdc("NOAS1", usr);
                var dtretrunNOAS2 = ldl.Getlisttasklistdc("NOAS2", usr);
                var dtretrunNOAS3 = ldl.Getlisttasklistdc("NOAS3", usr);
                var dtretrunPAY = ldl.Getlisttasklistdc("PAY", usr);
                var dtretrunPTP = ldl.Getlisttasklistdc("PTP", usr);


                jFormNT.Add("countnewtask", dtretrunNT.Count());
                jFormNT.Add("NewTask", dtretrunNT);
                jFormNT.Add("countcallback", dtretrunFUIM.Count());
                jFormNT.Add("Callback", dtretrunFUIM);
                jFormNT.Add("countlainnya", dtretrunLL.Count());
                jFormNT.Add("Lainnya", dtretrunLL);
                jFormNT.Add("countmsg", dtretrunMESS.Count());
                jFormNT.Add("Message", dtretrunMESS);
                jFormNT.Add("countnoas", dtretrunNOAS.Count());
                jFormNT.Add("NoAnswer", dtretrunNOAS);
                jFormNT.Add("countnoas1", dtretrunNOAS1.Count());
                jFormNT.Add("TidakDiangkat", dtretrunNOAS1);
                jFormNT.Add("countnoas2", dtretrunNOAS2.Count());
                jFormNT.Add("NoTidakAktif", dtretrunNOAS2);
                jFormNT.Add("countnoas3", dtretrunNOAS3.Count());
                jFormNT.Add("WrongNumber", dtretrunNOAS3);
                jFormNT.Add("countpay", dtretrunPAY.Count());
                jFormNT.Add("Payment", dtretrunPAY);
                jFormNT.Add("countptp", dtretrunPTP.Count());
                jFormNT.Add("PromiseToPay", dtretrunPTP);

                data.Add("status", mc.GetMessage("api_output_ok"));
                data.Add("message", mc.GetMessage("process_success"));
                data.Add("data", jFormNT);
            }
            catch (Exception ex)
            {
                data = new JObject();
                data.Add("status", mc.GetMessage("api_output_not_ok"));
                data.Add("message", ex.Message);
            }

            return data;
        }

        [HttpGet("ddljanjibayar/dc")]
        public JObject ddljanjibayardc()
        {
            var data = new JObject();
            try
            {

                var dtReturn = ldl.ddljanjibayardc();
                data.Add("status", mc.GetMessage("api_output_ok"));
                data.Add("message", mc.GetMessage("process_success"));
                data.Add("data", dtReturn);
            }
            catch (Exception ex)
            {
                data.Add("status", mc.GetMessage("api_output_not_ok"));
                data.Add("message", ex.Message);
            }
            return data;
        }

        [HttpGet("ddlreason/dc")]
        public JObject ddlreason()
        {
            var data = new JObject();
            try
            {

                var dtReturn = ldl.ddlreason();
                data.Add("status", mc.GetMessage("api_output_ok"));
                data.Add("message", mc.GetMessage("process_success"));
                data.Add("data", dtReturn);
            }
            catch (Exception ex)
            {
                data.Add("status", mc.GetMessage("api_output_not_ok"));
                data.Add("message", ex.Message);
            }
            return data;
        }


        [HttpPost("detail")]
        public JObject Getdetaildc([FromBody] JObject json)
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
                if (retgnrlinfo.Count > 0)
                {
                    var dtretrun1 = ldl.GetListCallRecord(json);
                    var dtretrun2 = ldl.GetListActionHistory(json);



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
                        jFormDetData.Add("city", retgnrlinfo[0].city);
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

                    var cif = retgnrlinfo[0].no_cif;
                    var dpd = Convert.ToString(retgnrlinfo[0].dpd);
                    var dtretrun3 = ldl.GetListaddcontactbycif(cif);
                    var dtretrun4 = ldl.GetListcallscriptbydpd(dpd);

                    jFormCont.Add("id", json.GetValue("id"));
                    jFormCont.Add("name", retgnrlinfo[0].name);
                    jFormCont.Add("no_cif", retgnrlinfo[0].no_cif);
                    jFormCont.Add("norek", retgnrlinfo[0].norek);
                   
                    jFormCont.Add("genearlinfo", jaFormDet);
                    jFormCont.Add("addcontact", dtretrun3);
                    jFormCont.Add("callscript", dtretrun4);
                    jFormCont.Add("fasilitas", new JArray());
                    jFormCont.Add("agunan", new JArray());
                    jFormCont.Add("callrecord", dtretrun1);
                    jFormCont.Add("actionhistory", dtretrun2);

                    data.Add("status", mc.GetMessage("api_output_ok"));
                    data.Add("message", mc.GetMessage("process_success"));
                    data.Add("data", new JArray(jFormCont));
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

        [HttpPost("deskcall/contact/insert")]
        public JObject deskcallcontactinsert([FromBody] JObject json)
        {
            var data = new JObject();

            try
            {

                string strout = "";
                var loan_id = json.GetValue("id").ToString();
                var userid = json.GetValue("userid").ToString();
                var dtReturn1 = ldl.checkloanmasterbyid(loan_id);
                if (dtReturn1.Count > 0)
                {
                    var dtretrunusr = ldl.getDataUserdetail(userid);
                    json["cif"] = dtReturn1[0]["v_cu_cif"].ToString();
                    json["accno"] = dtReturn1[0]["v_acc_no"].ToString();
                    json["form"] = "DC WEB";
                    if (dtReturn1.Count > 0)
                    {
                        json["usrid"] = dtretrunusr[0]["usrid"].ToString();
                    }
                    else
                    {
                        json["usrid"] = "0";
                    }

                    strout = bcx.insertdeskcallcontact(json);
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
                else
                {
                    data = new JObject();
                    data.Add("status", mc.GetMessage("api_output_not_ok"));
                    data.Add("message", "loan not found");
                }
                  


            }
            catch (Exception ex)
            {
                data.Add("status", mc.GetMessage("api_output_not_ok"));
                data.Add("message", ex.Message);
            }
            return data;
        }

        [HttpPost("deskcall/insert")]
        public JObject insertdeskcalldc([FromBody] JObject json)
        {
            var data = new JObject();

            try
            {

                string strout = "";
                var loan_id = json.GetValue("id").ToString();
                var userid = json.GetValue("userid").ToString();
                var dtReturn1 = ldl.getdateloanmasterbyid(loan_id);
                if (dtReturn1.Count > 0)
                {
                    var dtretrunusr = ldl.getDataUserdetail(userid);
                    json["callid"] = dtReturn1[0]["v_callid"].ToString();
                    json["brncid"] = dtReturn1[0]["v_branchid"].ToString();
                    json["cif"] = dtReturn1[0]["v_cu_cif"].ToString();
                    json["accno"] = dtReturn1[0]["v_acc_no"].ToString();
                    json["dpd"] = dtReturn1[0]["v_dpd"].ToString();
                    json["kolek"] = dtReturn1[0]["v_kolek"].ToString();
                    json["usrid"] = dtretrunusr[0]["usrid"].ToString();
                    json["callby"] = dtReturn1[0]["v_callby"].ToString();

                    //if (json.GetValue("p_addid").ToString() == "")
                    //{
                    //    json["addid"] = dtReturn1[0]["v_add_id"].ToString();
                    //}
                    //else
                    //{
                    //    json["addid"] = json.GetValue("p_addid").ToString();
                      
                    //}
                  

                    strout = bcx.insertdeskcalldc(json);
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
                else
                {
                    data = new JObject();
                    data.Add("status", mc.GetMessage("api_output_not_ok"));
                    data.Add("message", "loan not found");
                }



            }
            catch (Exception ex)
            {
                data.Add("status", mc.GetMessage("api_output_not_ok"));
                data.Add("message", ex.Message);
            }
            return data;
        }

        [HttpPost("deskcall/call")]
        public JObject integrasibrikerbox([FromBody] JObject json)
        {
            var data = new JObject();
            var jFormCont = new JObject();
            var jreqlogin = new JObject();
            string strout = "";
            try
            {
                var callid = json.GetValue("id").ToString();
                var userid = json.GetValue("userid").ToString();

                var dtretrunusr = ldl.getDataUserdetail(userid);
                if (dtretrunusr.Count > 0)
                {
               
                    var dtReturn1 = ldl.getDataAddContact(callid);
                    jFormCont.Add("callid", dtReturn1[0]["v_id"].ToString());
                    jFormCont.Add("phoneno", dtReturn1[0]["v_add_phone"].ToString());
                    jFormCont.Add("statusid", "8");
                    jFormCont.Add("usrid", dtretrunusr[0]["usrid"].ToString());

                    strout = bcx.insertrequestcall(jFormCont);
                    var pars = strout.Split('|');
                    if (pars[0] == "success")
                    {
                        var dtReturn3 = ldl.getDetailGlobalConfig("BRKIP");
                        if (dtReturn3.Count > 0)
                        {

                            var agent = dtretrunusr[0]["usr_tel_device"].ToString();
                            var code = dtretrunusr[0]["usr_tel_code"].ToString();

                            var url = "https://" + dtReturn3[0]["glc_value"].ToString() + ":10021";
                            jreqlogin.Add("cmd", "admlogin");

                            var tiket = bc.ExecPostAPILoginBrikerbox(url, jreqlogin);
                            if (tiket.Length > 0)
                            {
                                Random rnd = new Random();
                                var ran = rnd.Next();
                                var respagent = bc.LoginAgent(tiket, agent, code, url);
                                if (respagent == true)
                                {
                                    bc.DoCall(tiket, dtReturn1[0]["v_add_phone"].ToString(), agent, pars[1].ToString(), url);
                                    bc.LogoutAgent(tiket, agent, code, url);
                                    data.Add("status", mc.GetMessage("api_output_ok"));
                                    data.Add("message", mc.GetMessage("execdb_success"));
                                }
                                else
                                {
                                    bcx.updaterequestcallbyid(pars[1].ToString());
                                    data.Add("status", mc.GetMessage("api_output_not_ok"));
                                    data.Add("message", mc.GetMessage("process_not_success"));
                                }
                            }
                            else
                            {
                                bcx.updaterequestcallbyid(pars[1].ToString());
                                data.Add("status", mc.GetMessage("api_output_not_ok"));
                                data.Add("message", mc.GetMessage("process_not_success"));

                            }

                        }

                    }
                    else
                    {
                        data.Add("status", mc.GetMessage("api_output_not_ok"));
                        data.Add("message", strout);
                    }
                }
                  


            }
            catch (Exception ex)
            {
                data.Add("status", mc.GetMessage("api_output_not_ok"));
                data.Add("message", ex.Message);
            }
            return data;
        }

        #region FC
        [HttpPost("list/fc")]
        public JObject GetListtasklistfc([FromBody] JObject json)
        {
            var retObject = new List<dynamic>();
            var data = new JObject();

            var jaFormDet = new JArray();
            try
            {
                var userid = json.GetValue("userid").ToString();
                var dtretrunusr = ldl.getDataUserdetail(userid);

                if (dtretrunusr.Count > 0)
                {
                    var id = dtretrunusr[0]["usrid"].ToString();
                    var dtretrun = ldl.Getlisttasklistfc(id);

                    data.Add("status", mc.GetMessage("api_output_ok"));
                    data.Add("message", mc.GetMessage("process_success"));
                    data.Add("data", dtretrun);
                }
                else
                {
                    data.Add("status", mc.GetMessage("api_output_not_ok"));
                    data.Add("message", mc.GetMessage("process_not_success"));
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


        [HttpPost("detail/fc")]
        public JObject Getdetailfc([FromBody] JObject json)
        {
            var retgnrlinfo = new List<dynamic>();
            var retcontact = new List<dynamic>();
            var retcollateral = new List<dynamic>();
            var retlog = new List<dynamic>();

            var data = new JObject();
            try
            {
                data = new JObject();
                var jaFormDet = new JArray();
                var jaFormDet1 = new JArray();
                var jaFormDet2 = new JArray();
                var jFormCont = new JObject();
                retgnrlinfo = ldl.GetDetailGnrlInfofc(json);
               
                if (retgnrlinfo.Count > 0)
                {
                    var jFormDetData = new JObject();
                    var jDatainfokrdit = new JObject();


                    // informasi kontak
                    jFormDetData.Add("no_ktp", retgnrlinfo[0].ktp);
                    jFormDetData.Add("tgl_lahir", retgnrlinfo[0].tgl_lahir);
                    jFormDetData.Add("alamat", retgnrlinfo[0].alamat);
                    jFormDetData.Add("kelurahan", retgnrlinfo[0].kelurahan);
                    jFormDetData.Add("kecamatan", retgnrlinfo[0].kecamatan);
                    jFormDetData.Add("city", retgnrlinfo[0].city);

                    var addcontact = ldl.GetDataContactFC(json);
                    if (addcontact.Count > 0)
                    {
                        for (int i = 0; i < addcontact.Count(); i++)
                        {
                            var jFormDetData1 = new JObject();
                            jFormDetData1.Add("form", addcontact[i]["v_add_form"].ToString());
                            jFormDetData1.Add("no_hp", addcontact[i]["v_add_phone"].ToString());

                            jaFormDet2.Add(jFormDetData1);
                        }
                    }
                    else
                    {
                        retcontact = ldl.GetDataContactFCbyMasterCust(json);

                        var nohp = retcontact[0].no_hp;
                        string[] nohpsplt = nohp.Split("]");
                        for (int i = 0; i < nohpsplt.Length; i++)
                        {
                            var jFormDetData1 = new JObject();
                            jFormDetData1.Add("form", "CORE");
                            jFormDetData1.Add("no_hp", nohpsplt[i]);
                            jaFormDet2.Add(jFormDetData1);
                        }
                    }



                    jFormDetData.Add("tlpn", jaFormDet2);

                    jaFormDet.Add(jFormDetData);

                    // informasi kredit
                    jDatainfokrdit.Add("no_kontak", retgnrlinfo[0].no_kontak);
                    jDatainfokrdit.Add("loan_number", retgnrlinfo[0].loan_number);
                    jDatainfokrdit.Add("startdate", retgnrlinfo[0].startdate);
                    jDatainfokrdit.Add("segement", retgnrlinfo[0].segement);
                    jDatainfokrdit.Add("product", retgnrlinfo[0].product);
                    jDatainfokrdit.Add("jumlah_angsuaran", retgnrlinfo[0].jumlah_angsuaran);
                    jDatainfokrdit.Add("tgl_mulai", retgnrlinfo[0].tgl_mulai);
                    jDatainfokrdit.Add("tgl_jatuh_tempo", retgnrlinfo[0].tgl_jatuh_tempo);
                    jDatainfokrdit.Add("tenor", retgnrlinfo[0].tenor);
                    jDatainfokrdit.Add("plafond", retgnrlinfo[0].plafond);
                    jDatainfokrdit.Add("outstanding", retgnrlinfo[0].outstanding);
                    jDatainfokrdit.Add("kolektabilitas", retgnrlinfo[0].kolektabilitas);
                    jDatainfokrdit.Add("dpd", retgnrlinfo[0].dpd);
                    jDatainfokrdit.Add("tglbayarterakhir", retgnrlinfo[0].tglbayarterakhir);
                    jDatainfokrdit.Add("tunggakanpokok", retgnrlinfo[0].tunggakanpokok);
                    jDatainfokrdit.Add("tunggakanbunga", retgnrlinfo[0].tunggakanbunga);
                    jDatainfokrdit.Add("tunggakandenda", retgnrlinfo[0].tunggakandenda);
                    jDatainfokrdit.Add("tunggakantotal", retgnrlinfo[0].tunggakantotal);
                    jDatainfokrdit.Add("kewajibantotal", retgnrlinfo[0].kewajibantotal);
                    jaFormDet1.Add(jDatainfokrdit);



                    jFormCont.Add("id", json.GetValue("id"));
                    jFormCont.Add("name", retgnrlinfo[0].name);
                    jFormCont.Add("no_cif", retgnrlinfo[0].no_cif);


                    jFormCont.Add("infokontak", jaFormDet);
                    jFormCont.Add("infokredit", jaFormDet1);


                    data.Add("status", mc.GetMessage("api_output_ok"));
                    data.Add("message", mc.GetMessage("process_success"));
                    data.Add("data", new JArray(jFormCont));
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

        [HttpPost("list/janjibayar/fc")]
        public JObject GetListjanjibayarfc([FromBody] JObject json)
        {
            var retObject = new List<dynamic>();
            var data = new JObject();

            var jaFormDet = new JArray();
            try
            {
                var userid = json.GetValue("userid").ToString();
                var dtretrunusr = ldl.getDataUserdetail(userid);

                if (dtretrunusr.Count > 0)
                {
                    var id = dtretrunusr[0]["usrid"].ToString();
                    var dtretrun = ldl.Getlistjanjibayarfc(id);

                    data.Add("status", mc.GetMessage("api_output_ok"));
                    data.Add("message", mc.GetMessage("process_success"));
                    data.Add("data", dtretrun);
                }
                else
                {
                    data.Add("status", mc.GetMessage("api_output_not_ok"));
                    data.Add("message", mc.GetMessage("process_not_success"));
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

        [HttpPost("list/riwayat/fc")]
        public JObject GetListriwayatfc([FromBody] JObject json)
        {
            var retObject = new List<dynamic>();
            var data = new JObject();

            var jaFormDet = new JArray();
            try
            {
                var userid = json.GetValue("userid").ToString();
                var dtretrunusr = ldl.getDataUserdetail(userid);

                if (dtretrunusr.Count > 0)
                {
                    var id = dtretrunusr[0]["usrid"].ToString();
                    var dtretrun = ldl.Getlistriwayatfc(id);

                    data.Add("status", mc.GetMessage("api_output_ok"));
                    data.Add("message", mc.GetMessage("process_success"));
                    data.Add("data", dtretrun);
                }
                else
                {
                    data.Add("status", mc.GetMessage("api_output_not_ok"));
                    data.Add("message", mc.GetMessage("process_not_success"));
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

        [HttpPost("riwayat/fc")]
        public JObject GetDataRiwayatfc([FromBody] JObject json)
        {
            var retObject = new List<dynamic>();
            var data = new JObject();

            var jaFormDet = new JArray();
            try
            {

                var loan_id = json.GetValue("id").ToString(); ;
                var dtretrun = ldl.Getdetailriwayatfc(loan_id);

                data.Add("status", mc.GetMessage("api_output_ok"));
                data.Add("message", mc.GetMessage("process_success"));
                data.Add("data", dtretrun);


            }
            catch (Exception ex)
            {
                data = new JObject();
                data.Add("status", mc.GetMessage("api_output_not_ok"));
                data.Add("message", ex.Message);
            }

            return data;
        }

        [HttpGet("ddlreason/fc")]
        public JObject ddlreasonfc()
        {
            var data = new JObject();
            try
            {

                var dtReturn = ldl.ddlreasonfc();
                data.Add("status", mc.GetMessage("api_output_ok"));
                data.Add("message", mc.GetMessage("process_success"));
                data.Add("data", dtReturn);
            }
            catch (Exception ex)
            {
                data.Add("status", mc.GetMessage("api_output_not_ok"));
                data.Add("message", ex.Message);
            }
            return data;
        }

        [HttpPost("fieldcoll/uploadphoto")]
        public JObject insert_fieldcoll_uploadphoto([FromBody] JObject json)
        {
            var data = new JObject();

            try
            {

                string strout = "";
                var userid = json.GetValue("userid").ToString();
                var dtretrunusr = ldl.getDataUserdetail(userid);
                json["usrid"] = dtretrunusr[0]["usrid"].ToString();

                strout = bcx.insertuploadphoto(json);
                var pars = strout.Split('|');
                if (pars[0] == "success")
                {
                    data.Add("status", mc.GetMessage("api_output_ok"));
                    data.Add("message", mc.GetMessage("save_success"));
                    data.Add("photoid", pars[1].ToString());

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


        [HttpPost("fieldcoll/insert/hasilvisit")]
        public JObject insert_fieldcoll_hasil_visit([FromBody] JObject json)
        {
            var data = new JObject();

            try
            {

                string strout = "";
                var loan_id = json.GetValue("id").ToString();
                var userid = json.GetValue("userid").ToString();
                var dtReturn1 = ldl.getdateloanmasterbyid(loan_id);
                if (dtReturn1.Count > 0)
                {
                    var dtretrunusr = ldl.getDataUserdetail(userid);
                    json["callid"] = dtReturn1[0]["v_callid"].ToString();
                    json["brncid"] = dtReturn1[0]["v_branchid"].ToString();
                    json["accno"] = dtReturn1[0]["v_acc_no"].ToString();
                    json["usrid"] = dtretrunusr[0]["usrid"].ToString();
                    json["kolek"] = dtReturn1[0]["v_kolek"].ToString();
                    json["dpd"] = dtReturn1[0]["v_dpd"].ToString();
                    json["resultid"] = dtReturn1[0]["v_result"].ToString();
                    json["callby"] = dtReturn1[0]["v_callby"].ToString();

                    strout = bcx.insert_fieldcoll_hasil_visit(json);
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
                else
                {
                    data = new JObject();
                    data.Add("status", mc.GetMessage("api_output_not_ok"));
                    data.Add("message", "loan not found");
                }



            }
            catch (Exception ex)
            {
                data.Add("status", mc.GetMessage("api_output_not_ok"));
                data.Add("message", ex.Message);
            }
            return data;
        }

        [HttpPost("fieldcoll/insert/janjibayar")]
        public JObject insert_fieldcoll_janji_bayar([FromBody] JObject json)
        {
            var data = new JObject();

            try
            {

                string strout = "";
                var loan_id = json.GetValue("id").ToString();
                var userid = json.GetValue("userid").ToString();
                var dtReturn1 = ldl.getdateloanmasterbyid(loan_id);
                if (dtReturn1.Count > 0)
                {
                    var dtretrunusr = ldl.getDataUserdetail(userid);
                    json["callid"] = dtReturn1[0]["v_callid"].ToString();
                    json["brncid"] = dtReturn1[0]["v_branchid"].ToString();
                    json["accno"] = dtReturn1[0]["v_acc_no"].ToString();
                    json["usrid"] = dtretrunusr[0]["usrid"].ToString();
                    json["kolek"] = dtReturn1[0]["v_kolek"].ToString();
                    json["dpd"] = dtReturn1[0]["v_dpd"].ToString();
                    json["reasonid"] = dtReturn1[0]["v_reason"].ToString();
                    json["callby"] = dtReturn1[0]["v_callby"].ToString();

                    strout = bcx.insert_fieldcoll_janjibayar(json);
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
                else
                {
                    data = new JObject();
                    data.Add("status", mc.GetMessage("api_output_not_ok"));
                    data.Add("message", "loan not found");
                }



            }
            catch (Exception ex)
            {
                data.Add("status", mc.GetMessage("api_output_not_ok"));
                data.Add("message", ex.Message);
            }
            return data;
        }

        [HttpPost("fieldcoll/contact/insert")]
        public JObject fieldcollcontactinsert([FromBody] JObject json)
        {
            var data = new JObject();

            try
            {

                string strout = "";
                var loan_id = json.GetValue("id").ToString();
                var userid = json.GetValue("userid").ToString();
                var dtReturn1 = ldl.checkloanmasterbyid(loan_id);
                if (dtReturn1.Count > 0)
                {
                    var dtretrunusr = ldl.getDataUserdetail(userid);
                    json["cif"] = dtReturn1[0]["v_cu_cif"].ToString();
                    json["accno"] = dtReturn1[0]["v_acc_no"].ToString();
                    json["form"] = "FC APPS";
                    if (dtReturn1.Count > 0)
                    {
                        json["usrid"] = dtretrunusr[0]["usrid"].ToString();
                    }
                    else
                    {
                        json["usrid"] = "0";
                    }

                    strout = bcx.insertfieldcollcontact(json);
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
                else
                {
                    data = new JObject();
                    data.Add("status", mc.GetMessage("api_output_not_ok"));
                    data.Add("message", "loan not found");
                }



            }
            catch (Exception ex)
            {
                data.Add("status", mc.GetMessage("api_output_not_ok"));
                data.Add("message", ex.Message);
            }
            return data;
        }

        [HttpPost("fieldcoll/contact/uploadphoto")]
        public JObject insert_fieldcoll_contact_uploadphoto([FromBody] JObject json)
        {
            var data = new JObject();

            try
            {

                string strout = "";
                var userid = json.GetValue("userid").ToString();
                var dtretrunusr = ldl.getDataUserdetail(userid);
                json["usrid"] = dtretrunusr[0]["usrid"].ToString();

                strout = bcx.insertuploadphotocontact(json);
                var pars = strout.Split('|');
                if (pars[0] == "success")
                {
                    data.Add("status", mc.GetMessage("api_output_ok"));
                    data.Add("message", mc.GetMessage("save_success"));
                    data.Add("photoid", pars[1].ToString());

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


        #endregion


    }
}
