using sky.coll.Controllers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;


namespace sky.coll.Libs
{
    public class lDataLayer
    {
        private BaseController bc = new BaseController();
        private lDbConn dbconn = new lDbConn();
        private lConvert lc = new lConvert();
        private MessageController mc = new MessageController();
        private lData ldt = new lData();
        private lPgsql lp = new lPgsql();

        public List<dynamic> lObjectChar = new List<dynamic>();
        public List<dynamic> lObject = new List<dynamic>();
 

       

        #region master

        public List<dynamic> GetDetailSmsContentByid(int lsc_id)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            string spname = "version_crud.get_sms_content_detail";
            string p1 = "p_id," + lsc_id + ",bg";
            var retObject = new List<dynamic>();
            retObject = bc.getDataToObject(provider, cstrname, spname,p1);
            return retObject;
        }
        public List<dynamic> CheckSmsContentByCode(string code)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            string spname = "version_crud.check_sms_content_detail";
            var retObject = new List<dynamic>();
            string p1 = "p_code," + code + ",s";
            retObject = bc.getDataToObject(provider, cstrname, spname, p1);
            return retObject;
        }

        public List<dynamic> GetSmsContentById(string id)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            string spname = "version_crud.get_sms_content_detail";
            var retObject = new List<dynamic>();
            string p1 = "p_id," + id + ",bg";
            retObject = bc.getDataToObject(provider, cstrname, spname, p1);
            return retObject;
        }

        //For CallScript Region

        public List<dynamic> GetCallScriptDetail(string csc_id)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            string spname = "version_crud.get_callscript_detail";
            var retObject = new List<dynamic>();
            string p1 = "p_id," + csc_id + ",bg";
            retObject = bc.getDataToObject(provider, cstrname, spname, p1);
            return retObject;
        }


        public List<dynamic> GetCallScriptDetailById(string id)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            string spname = "version_crud.get_callscript_detail_byid";
            var retObject = new List<dynamic>();
            string p1 = "p_id," + id + ",bg";
            retObject = bc.getDataToObject(provider, cstrname, spname, p1);
            return retObject;
        }

        public List<dynamic> GetCallScriptDetailByCode(string code)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            string spname = "version_crud.get_callscript_detail_bycode";
            var retObject = new List<dynamic>();
            string p1 = "p_code," + code + ",s";
            retObject = bc.getDataToObject(provider, cstrname, spname, p1);
            return retObject;
        }




        #endregion


        #region reason

        public List<dynamic> GetReasonDetail(string rsn_id)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            string spname = "version_crud.get_reason_detail";
            var retObject = new List<dynamic>();
            string p1 = "p_id," + rsn_id + ",bg";
            retObject = bc.getDataToObject(provider, cstrname, spname, p1);
            return retObject;
        }

        public List<dynamic> GetReasonDetailByCode(string code)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            string spname = "version_crud.get_reason_detail_bycode";
            var retObject = new List<dynamic>();
            string p1 = "p_code," + code + ",s";
            retObject = bc.getDataToObject(provider, cstrname, spname, p1);
            return retObject;
        }

        #endregion


        #region check version 
        public List<dynamic> Checkversion(string id, string module)
        {

            var retObject = new List<dynamic>();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skyen");

            string spname = "masters.checkversion";
            string p1 = "p_id," + id + ",s";
            string p2 = "p_module," + module + ",s";
            retObject = bc.getDataToObject(provider, cstrname, spname, p1, p2);
            return retObject;
        }

        public List<dynamic> Checkdataparamversion(string id, string module)
        {

            var retObject = new List<dynamic>();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skyen");

            string spname = "masters.checkdataparamversion";
            string p1 = "p_id," + id + ",s";
            string p2 = "p_module," + module + ",s";
            retObject = bc.getDataToObject(provider, cstrname, spname, p1, p2);
            return retObject;
        }


        #endregion

        #region reassign dc
        public List<dynamic> Getlistreassigndc(JObject json)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            string spname = "public.reassign_dc_list";
            string p1 = "p_usr," + json.GetValue("usr").ToString() + ",s";
            var retObject = new List<dynamic>();
            retObject = bc.getDataToObject(provider, cstrname, spname,p1);
            return retObject;
        }

        public List<dynamic> Getlistreassigndcddwon()
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycore");
            string spname = "public.get_name_user_by_assigndc";

            var retObject = new List<dynamic>();
            retObject = bc.getDataToObject(provider, cstrname, spname);
            return retObject;
        }

        public List<dynamic> GetDetailGnrlInfo(JObject json)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            string spname = "public.get_detail_reassign_gnrl_info";
            string p1 = "p_id," + json.GetValue("id").ToString() + ",bg";
            var retObject = new List<dynamic>();
            retObject = bc.getDataToObject(provider, cstrname, spname, p1);
            return retObject;
        }
        public List<dynamic> GetDetailActivityLog(string  accno)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            string spname = "public.activity_history_list";
            string p1 = "p_accno," + accno + ",s";
            var retObject = new List<dynamic>();
            retObject = bc.getDataToObject(provider, cstrname, spname, p1);
            return retObject;
        }

        public List<dynamic> GetDetailactivityhistory(JObject json)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            string spname = "public.get_detail_activity_history";
            string p1 = "p_id," + json.GetValue("id").ToString() + ",bg";
            var retObject = new List<dynamic>();
            retObject = bc.getDataToObject(provider, cstrname, spname, p1);
            return retObject;
        }

        public List<dynamic> GetDetailtunggakanbyid(JObject json)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            string spname = "public.get_detail_tunggakan_byid";
            string p1 = "p_id," + json.GetValue("id").ToString() + ",i";
            var retObject = new List<dynamic>();
            retObject = bc.getDataToObject(provider, cstrname, spname, p1);
            return retObject;
        }
        #endregion

        #region dc
        public List<dynamic> Getlistreassignfcddwon()
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycore");
            string spname = "public.get_name_user_by_assignfc";

            var retObject = new List<dynamic>();
            retObject = bc.getDataToObject(provider, cstrname, spname);
            return retObject;
        }

        public List<dynamic> Getlistreassignfc(JObject json)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            string spname = "public.reassign_fc_list";
            string p1 = "p_usr," + json.GetValue("usr").ToString() + ",s";
            var retObject = new List<dynamic>();
            retObject = bc.getDataToObject(provider, cstrname, spname,p1);
            return retObject;
        }

        public List<dynamic> reassigndc(string usrid, string loanid)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            string spname = "public.reassign_tasklist";
            string p1 = "p_usrid," + usrid + ",i";
            string p2 = "p_loanid," + loanid + ",i";
            var retObject = new List<dynamic>();
            retObject = bc.getDataToObject(provider, cstrname, spname, p1, p2);
            return retObject;
        }


        
        #endregion

        #region generate letter 
        public List<dynamic> Getlistgenerateletter(string branchcode)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            string spname = "public.generate_letter_list";
            string p1 = "p_branchcode," + branchcode + ",s";
            var retObject = new List<dynamic>();
            retObject = bc.getDataToObject(provider, cstrname, spname, p1);
            return retObject;
        }
        public List<dynamic> Getlistgenerateletterhistory()
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            string spname = "public.generate_letter_list_history";

            var retObject = new List<dynamic>();
            retObject = bc.getDataToObject(provider, cstrname, spname);
            return retObject;
        }
        public List<dynamic> Getlistgenerateletterhistorybybranchname(string branchname)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            string spname = "public.generate_letter_list_historybybrn";
            string p1 = "p_branch_name," + branchname + ",s";
            var retObject = new List<dynamic>();
            retObject = bc.getDataToObject(provider, cstrname, spname, p1);
            return retObject;
        }
        public List<dynamic> Getlistgenerateletterhistorybybrnkyz()
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            string spname = "public.generate_letter_list_historybybrnkyz";
            var retObject = new List<dynamic>();
            retObject = bc.getDataToObject(provider, cstrname, spname);
            return retObject;
        }
        public JArray getDataUserdetail(string p_userid)
        {
            var jaReturn = new JArray();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycore");

            var split = "||";
            var schema = "public";

            string spname = "usr_getuser_detail";
            string p1 = "@userid" + split + p_userid + split + "s";
            var retObject = new List<dynamic>();
            retObject = bc.ExecSqlWithReturnCustomSplit(provider, cstrname, split, schema, spname, p1);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;
        }

        public JArray checkusercollbyid(string p_id, string rlname)
        {
            var jaReturn = new JArray();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycore");

            var split = "||";
            var schema = "public";

            string spname = "check_user_coll";
            string p1 = "@usrid" + split + p_id + split + "i";
            string p2 = "@rlname" + split + rlname + split + "s";
            var retObject = new List<dynamic>();
            retObject = bc.ExecSqlWithReturnCustomSplit(provider, cstrname, split, schema, spname, p1,p2);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;
        }
        public JArray checkbranchactivebyuserid(string p_userid)
        {
            var jaReturn = new JArray();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycore");

            var split = "||";
            var schema = "param";

            string spname = "check_branch_active_byusrid";
            string p1 = "@userid" + split + p_userid + split + "s";
            var retObject = new List<dynamic>();
            retObject = bc.ExecSqlWithReturnCustomSplit(provider, cstrname, split, schema, spname, p1);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;
        }
        public JArray checkloanmasterbyid(string loan_id)
        {
            var jaReturn = new JArray();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");

            var split = "||";
            var schema = "public";

            string spname = "check_loan_master_byid";
            string p1 = "@id" + split + loan_id + split + "bg";
            var retObject = new List<dynamic>();
            retObject = bc.ExecSqlWithReturnCustomSplit(provider, cstrname, split, schema, spname, p1);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;
        }

        public List<dynamic> Getdetailgenerateletter(string loanid, string dpd)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            string spname = "public.generate_letter_list_view";
            string p1 = "p_loanid," + loanid + ",i";
            string p2 = "p_dpd," + dpd + ",i";
            var retObject = new List<dynamic>();
            retObject = bc.getDataToObject(provider, cstrname, spname, p1,p2);
            return retObject;
        }
        public List<dynamic> Getdetailgenerateletterdownload(string loanid, string code)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            string spname = "public.generate_letter_download";
            string p1 = "p_loanid," + loanid + ",i";
            string p2 = "p_code," + code + ",s";
            var retObject = new List<dynamic>();
            retObject = bc.getDataToObject(provider, cstrname, spname, p1, p2);
            return retObject;
        }

        public List<dynamic> Getdetailgenerateletterinsert(string loanid, string code)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            string spname = "public.insert_generate_lette";
            string p1 = "p_loanid," + loanid + ",i";
            string p2 = "p_code," + code + ",i";
            var retObject = new List<dynamic>();
            retObject = bc.getDataToObject(provider, cstrname, spname, p1, p2);
            return retObject;
        }


        #endregion

        #region team fc

        public JArray Getteamfc(string spvid)
        {
            var jaReturn = new JArray();
            var dtretrun1 = this.Getlistteamfcbyspvid(spvid);
            for (int x = 0; x < dtretrun1.Count; x++)
            {
                var jFormDetData1 = new JObject();
                jFormDetData1.Add("fcid", dtretrun1[x]["v_id"].ToString());
                //jFormDetData1.Add("fcmemberid", dtretrun1[x]["v_memberid"].ToString());
                jFormDetData1.Add("fcuserid", dtretrun1[x]["v_usr_userid"].ToString());
                jFormDetData1.Add("fcusrname", dtretrun1[x]["v_usr_name"].ToString() + " (" + dtretrun1[x]["v_status"].ToString() + ")");
                jFormDetData1.Add("fcrolename", dtretrun1[x]["v_rol_name"].ToString());
                //jFormDetData1.Add("fcstatus", dtretrun1[x]["v_status"].ToString());
                jaReturn.Add(jFormDetData1);
            }

            return jaReturn;
        }

        public JArray Getlistteamspvfc()
        {
            var jaReturn = new JArray();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycore");

            var split = "||";
            var schema = "public";

            string spname = "get_team_spv_fc_list";
            var retObject = new List<dynamic>();
            retObject = bc.ExecSqlWithReturnCustomSplit(provider, cstrname, split, schema, spname);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;
        }

        public JArray Getlistteamfcbyspvid(string spvid)
        {
            var jaReturn = new JArray();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycore");

            var split = "||";
            var schema = "public";

            string spname = "get_team_fc_list";
            string p1 = "@spvusrid" + split + spvid + split + "s";
            var retObject = new List<dynamic>();
            retObject = bc.ExecSqlWithReturnCustomSplit(provider, cstrname, split, schema, spname,p1);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;
        }

        public List<dynamic> Getspvfcddown()
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycore");
            string spname = "public.get_team_spv_fc_list";

            var retObject = new List<dynamic>();
            retObject = bc.getDataToObject(provider, cstrname, spname);
            return retObject;
        }

        public JArray Getlistteambranch()
        {
            var jaReturn = new JArray();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycore");

            var split = "||";
            var schema = "param";

            string spname = "get_list_branch_coll";
            var retObject = new List<dynamic>();
            retObject = bc.ExecSqlWithReturnCustomSplit(provider, cstrname, split, schema, spname);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;
        }

        public JArray Getuserteambranchbycode(string brcode)
        {
            var jaReturn = new JArray();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycore");

            var split = "||";
            var schema = "public";

            string spname = "usr_getuser_branch_fc";
            string p1 = "@brc_code" + split + brcode + split + "s";
            var retObject = new List<dynamic>();
            retObject = bc.ExecSqlWithReturnCustomSplit(provider, cstrname, split, schema, spname, p1);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;
        }

        #endregion

        #region tasklist DC
        public JArray Getlisttasklistdc(string code, string usr)
        {
            var jaReturn = new JArray();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");

            var split = "||";
            var schema = "public";

            string spname = "getlist_dc_tasklist";
            string p1 = "@code" + split + code + split + "s";
            string p2 = "@usr" + split + usr + split + "s";
            var retObject = new List<dynamic>();
            retObject = bc.ExecSqlWithReturnCustomSplit(provider, cstrname, split, schema, spname, p1, p2);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;
        }

        public JArray ddljanjibayardc()
        {
            var jaReturn = new JArray();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");

            var split = "||";
            var schema = "public";

            string spname = "get_ddl_tasklist_janji";
            var retObject = new List<dynamic>();
            retObject = bc.ExecSqlWithReturnCustomSplit(provider, cstrname, split, schema, spname);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;
        }

        public JArray ddlreason()
        {
            var jaReturn = new JArray();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");

            var split = "||";
            var schema = "public";

            string spname = "get_ddl_reason";
            var retObject = new List<dynamic>();
            retObject = bc.ExecSqlWithReturnCustomSplit(provider, cstrname, split, schema, spname);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;
        }

        public JArray GetListCallRecord(JObject json)
        {
            var jaReturn = new JArray();

            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            var split = "||";
            var schema = "public";

            string spname = "getcallrecording";
            string p1 = "p_id" + split + json.GetValue("id").ToString() + split + "bg";
            var retObject = new List<dynamic>();
            retObject = bc.ExecSqlWithReturnCustomSplit(provider, cstrname, split, schema, spname, p1);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;
        }

        public JArray GetListaddcontactbycif(string cif)
        {
            var jaReturn = new JArray();

            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            var split = "||";
            var schema = "public";

            string spname = "getaddcontactbycif";
            string p1 = "p_cif" + split + cif + split + "s";
            var retObject = new List<dynamic>();
            retObject = bc.ExecSqlWithReturnCustomSplit(provider, cstrname, split, schema, spname, p1);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;
        }

        public JArray GetListcallscriptbydpd(string dpd)
        {
            var jaReturn = new JArray();

            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            var split = "||";
            var schema = "public";

            string spname = "getcallscriptbydpd";
            string p1 = "p_dpd" + split + dpd + split + "i";
            var retObject = new List<dynamic>();
            retObject = bc.ExecSqlWithReturnCustomSplit(provider, cstrname, split, schema, spname, p1);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;
        }

        public JArray GetListActionHistory(JObject json)
        {
            var jaReturn = new JArray();

            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            var split = "||";
            var schema = "public";

            string spname = "getlistactionhistory";
            string p1 = "p_id" + split + json.GetValue("id").ToString() + split + "bg";
            var retObject = new List<dynamic>();
            retObject = bc.ExecSqlWithReturnCustomSplit(provider, cstrname, split, schema, spname, p1);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;
        }

        public JArray getdateloanmasterbyid(string loan_id)
        {
            var jaReturn = new JArray();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");

            var split = "||";
            var schema = "public";

            string spname = "get_data_loan_master_byid";
            string p1 = "@id" + split + loan_id + split + "bg";
            var retObject = new List<dynamic>();
            retObject = bc.ExecSqlWithReturnCustomSplit(provider, cstrname, split, schema, spname, p1);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;
        }


        public JArray getDataAddContact(string loan_id)
        {
            var jaReturn = new JArray();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");

            var split = "||";
            var schema = "public";

            string spname = "getDataAddContact";
            string p1 = "@id" + split + loan_id + split + "i";
            var retObject = new List<dynamic>();
            retObject = bc.ExecSqlWithReturnCustomSplit(provider, cstrname, split, schema, spname, p1);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;
        }

        public JArray getDetailGlobalConfig(string code)
        {
            var jaReturn = new JArray();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycore");

            var split = "||";
            var schema = "param";

            string spname = "get_detail_global_config";
            string p1 = "@glc_code" + split + code + split + "s";
            var retObject = new List<dynamic>();
            retObject = bc.ExecSqlWithReturnCustomSplit(provider, cstrname, split, schema, spname, p1);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;
        }

        #endregion


        #region tasklist FC

        public JArray Getlisttasklistfc(string usrid)
        {
            var jaReturn = new JArray();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");

            var split = "||";
            var schema = "public";

            string spname = "getlist_fc_tasklist";
            string p1 = "@usrid" + split + usrid + split + "i";
            var retObject = new List<dynamic>();
            retObject = bc.ExecSqlWithReturnCustomSplit(provider, cstrname, split, schema, spname, p1);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;
        }

        public List<dynamic> GetDetailGnrlInfofc(JObject json)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            string spname = "public.get_detail_fc_gnrl_info";
            string p1 = "p_id," + json.GetValue("id").ToString() + ",bg";
            var retObject = new List<dynamic>();
            retObject = bc.getDataToObject(provider, cstrname, spname, p1);
            return retObject;
        }

        public List<dynamic> GetDataContactFCbyMasterCust(JObject json)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");

            string spname = "public.get_data_contact_fc_v2";
            string p1 = "p_loanid," + json.GetValue("id").ToString() + ",i";
            var retObject = new List<dynamic>();
            retObject = bc.getDataToObject(provider, cstrname, spname, p1);
            return retObject;
        }

        public JArray GetDataContactFC(JObject json)
        {
            var jaReturn = new JArray();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");

            var split = "||";
            var schema = "public";

            string spname = "get_data_contact_fc";
            string p1 = "p_loanid" + split + json.GetValue("id").ToString() + split + "i";
            var retObject = new List<dynamic>();
            retObject = bc.ExecSqlWithReturnCustomSplit(provider, cstrname, split, schema, spname, p1);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;
        }

   

        public JArray Getlistjanjibayarfc(string usrid)
        {
            var jaReturn = new JArray();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");

            var split = "||";
            var schema = "public";

            string spname = "getlist_fc_janji_bayar";
            string p1 = "@usrid" + split + usrid + split + "i";
            var retObject = new List<dynamic>();
            retObject = bc.ExecSqlWithReturnCustomSplit(provider, cstrname, split, schema, spname, p1);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;
        }

        public JArray Getlistriwayatfc(string usrid)
        {
            var jaReturn = new JArray();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");

            var split = "||";
            var schema = "public";

            string spname = "getlist_fc_riwayat";
            string p1 = "@usrid" + split + usrid + split + "i";
            var retObject = new List<dynamic>();
            retObject = bc.ExecSqlWithReturnCustomSplit(provider, cstrname, split, schema, spname, p1);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;
        }

        public JArray Getdetailriwayatfc(string loan_id)
        {
            var jaReturn = new JArray();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");

            var split = "||";
            var schema = "public";

            string spname = "get_detail_histor_fc";
            string p1 = "@id" + split + loan_id + split + "bg";
            var retObject = new List<dynamic>();
            retObject = bc.ExecSqlWithReturnCustomSplit(provider, cstrname, split, schema, spname, p1);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;
        }


        public JArray ddlreasonfc()
        {
            var jaReturn = new JArray();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");

            var split = "||";
            var schema = "public";

            string spname = "get_ddl_reason_fc";
            var retObject = new List<dynamic>();
            retObject = bc.ExecSqlWithReturnCustomSplit(provider, cstrname, split, schema, spname);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;
        }
        #endregion

        #region bucket

        public List<dynamic> GetlistBucket()
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            string spname = "public.get_list_bucket";

            var retObject = new List<dynamic>();
            retObject = bc.getDataToObject(provider, cstrname, spname);
            return retObject;
        }

        public JArray getdetailbucketbyid(string id)
        {
            var jaReturn = new JArray();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");

            var split = "||";
            var schema = "public";

            string spname = "getdatabucketbyid";
            string p1 = "@id" + split + id + split + "bg";
            var retObject = new List<dynamic>();
            retObject = bc.ExecSqlWithReturnCustomSplit(provider, cstrname, split, schema, spname, p1);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;
        }

        public JArray getdetailbucketdetailbyid(string id)
        {
            var jaReturn = new JArray();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");

            var split = "||";
            var schema = "public";

            string spname = "getdatabucketdetailbyid";
            string p1 = "@bct_id" + split + id + split + "bg";
            var retObject = new List<dynamic>();
            retObject = bc.ExecSqlWithReturnCustomSplit(provider, cstrname, split, schema, spname, p1);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;
        }
        #endregion

        #region notifikasi
        public List<dynamic> Getlistnotifikasi()
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            string spname = "public.get_list_notifikasi";

            var retObject = new List<dynamic>();
            retObject = bc.getDataToObject(provider, cstrname, spname);
            return retObject;
        }

        public List<dynamic> getLatestNotifikasiCode()
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            string spname = "public.get_latest_notifikasi_code";

            var retObject = new List<dynamic>();
            retObject = bc.getDataToObject(provider, cstrname, spname);
            return retObject;
        }
        #endregion
    }
}
