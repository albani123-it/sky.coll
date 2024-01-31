using sky.coll.Controllers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace sky.coll.Libs
{

    public class lData
    {
        private BaseController bc = new BaseController();
        private lDbConn dbconn = new lDbConn();
        private lConvert lc = new lConvert();
        private MessageController mc = new MessageController();

      
        public JArray ListMenuByParentId(string id)
        {
            var jaReturn = new JArray();
            var joReq = new JObject();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycore");

            List<dynamic> retObject = new List<dynamic>();
            string spname = "get_menu_list";
            string p1 = "p_id," + id + ",i";
            retObject = bc.getDataToObject(provider, cstrname, spname, p1);
            jaReturn = lc.convertDynamicToJArray(retObject);

            return jaReturn;
        }

        public List<dynamic> GetProspectListAll()
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skyen");
            List<dynamic> retObject = new List<dynamic>();
            //string spname = "submission.get_list_prospect_analyst_all";
            string spname = "submission.get_list_prospect_analyst_all_v2";
            retObject = bc.getDataToObject(provider, cstrname, spname);
            return retObject;
        }

        public JObject GetListUser(string user_id)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycore");
            var jaReturn = new JArray();
            var joReturn = new JObject();

            List<dynamic> retObject = new List<dynamic>();
            string spname = "public.get_member_user_byuser";
            string p1 = "p_usr_id," + user_id + ",s";
            retObject = bc.getDataToObject(provider, cstrname, spname, p1);
            jaReturn = lc.convertDynamicToJArray(retObject);

            joReturn = new JObject();
            if (jaReturn.Count > 0)
            {
                joReturn = new JObject();
                joReturn.Add("status", mc.GetMessage("api_output_ok"));
                joReturn.Add("data", jaReturn);
            }

            return joReturn;
            
        }



        public List<dynamic> GetProspectListV2(JObject json)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skyen");
            string split = "||";
            List<dynamic> retObject = new List<dynamic>();
            //string spname = "submission.get_list_prospect_analyst_byuser";
            string spname = "submission.get_list_prospect_analyst_byuser_v2";
            string p1 = "p_usr" + split + json.GetValue("users").ToString() + split + "s";
            retObject = bc.getDataToObjectCustomSplit(provider, cstrname, split, spname, p1);
            return retObject;
        }

        public List<dynamic> GetDecisionStatus()
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skyen");
            List<dynamic> retObject = new List<dynamic>();
            string spname = "public.get_decision_status";
            retObject = bc.getDataToObject(provider, cstrname, spname);
            return retObject;
        }

        public List<dynamic> GetRshProduct()
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skyen");

            List<dynamic> retObject = new List<dynamic>();
            string spname = "public.get_rsh_product";
            retObject = bc.getDataToObject(provider, cstrname, spname);
            return retObject;
        }

        public List<dynamic> GetProductRuleSetHistory()
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skyen");

            List<dynamic> retObject = new List<dynamic>();
            string spname = "public.get_product_rulset_history";
            retObject = bc.getDataToObject(provider, cstrname, spname);
            return retObject;
        }

        public List<dynamic> GetUserDecision()
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skyen");

            List<dynamic> retObject = new List<dynamic>();
            string spname = "submission.get_user_decision";
            retObject = bc.getDataToObject(provider, cstrname, spname);
            return retObject;
        }

        public List<dynamic> GetProspectListFilter(JObject json)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skyen");
            string split = "||";
            List<dynamic> retObject = new List<dynamic>();
            string spname = "submission.get_list_prospect_analyst_filter_v3";
            string p1 = "p_app_name" + split + json.GetValue("app_name") + split + "s";
            string p2 = "p_prd_code" + split + json.GetValue("prd_code") + split + "s";
            string p3 = "p_start_date" + split + json.GetValue("start_date") + split + "s";
            string p4 = "p_end_date" + split + json.GetValue("end_date") + split + "s";
            string p5 = "p_status" + split + json.GetValue("status") + split + "s";
            string p6 = "p_users" + split + json.GetValue("users") + split + "s";
            string p7 = "p_fname" + split + json.GetValue("fname") + split + "s";
            string p8 = "p_ktp" + split + json.GetValue("ktp") + split + "s";
            string p9 = "p_dob" + split + json.GetValue("dob") + split + "s";
            string p10 = "p_cf_los_app_no" + split + json.GetValue("cf_los_app_no") + split + "s";
            retObject = bc.getDataToObjectCustomSplit(provider, cstrname, split, spname, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
            return retObject;
        }

        #region dedup open api
        public JArray DedupProspectListFilter(JObject json)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skyen");
            string split = "||";
            List<dynamic> retObject = new List<dynamic>();
            var jaData = new JArray();
            string spname = "submission.dedup_list_prospect_analyst_filter";
            string p1 = "p_app_name" + split + json.GetValue("app_name") + split + "s";
            string p2 = "p_status" + split + json.GetValue("status") + split + "s";
            string p3 = "p_fname" + split + json.GetValue("fname") + split + "s";
            string p4 = "p_ktp" + split + json.GetValue("ktp") + split + "s";
            string p5 = "p_dob" + split + json.GetValue("dob") + split + "s";
            string p6 = "p_cf_los_app_no" + split + json.GetValue("cf_los_app_no") + split + "s";
            retObject = bc.getDataToObjectCustomSplit(provider, cstrname, split, spname, p1, p2, p3, p4, p5, p6);
            jaData = lc.convertDynamicToJArray(retObject);
            return jaData = lc.convertDynamicToJArray(retObject);
        }

        public JArray DedupProspectListFilter2(JObject json)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skyen");
            string split = "||";
            List<dynamic> retObject = new List<dynamic>();
            var jaData = new JArray();
            string spname = "submission.dedup_list_prospect_analyst_filter_rev2";
            string p1 = "p_cst_email" + split + json.GetValue("cst_email") + split + "s";
            string p2 = "p_status" + split + json.GetValue("status") + split + "s";
            string p3 = "p_fname" + split + json.GetValue("fname") + split + "s";
            string p4 = "p_ktp" + split + json.GetValue("ktp") + split + "s";
            string p5 = "p_dob" + split + json.GetValue("dob") + split + "s";
            string p6 = "p_cf_los_app_no" + split + json.GetValue("cf_los_app_no") + split + "s";
            retObject = bc.getDataToObjectCustomSplit(provider, cstrname, split, spname, p1, p2, p3, p4, p5, p6);
            jaData = lc.convertDynamicToJArray(retObject);
            return jaData = lc.convertDynamicToJArray(retObject);
        }
        #endregion
    }
}
