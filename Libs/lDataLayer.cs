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

        public List<dynamic> GetDetailColl(int lsc_id)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            string spname = "version_crud.get_coll_detail";
            string p1 = "p_id," + lsc_id + ",bg";
            var retObject = new List<dynamic>();
            retObject = bc.getDataToObject(provider, cstrname, spname,p1);
            return retObject;
        }
        public List<dynamic> CheckSmsContentByCode(string code)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            string spname = "version_crud.check_coll_detail";
            var retObject = new List<dynamic>();
            string p1 = "p_code," + code + ",s";
            retObject = bc.getDataToObject(provider, cstrname, spname, p1);
            return retObject;
        }

        public List<dynamic> GetSmsContentById(string id)
        {
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            string spname = "version_crud.get_smscontent_detail_byid";
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

    }
}
