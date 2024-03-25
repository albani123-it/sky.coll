using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sky.coll.Libs;
using Newtonsoft.Json.Linq;
using Npgsql;
using System.Data;
using System.Dynamic;
using System.Data.SqlClient;

namespace sky.coll.Controllers
{
    public class BasetrxController : Controller
    {

        private lDbConn dbconn = new lDbConn();
        private BaseController bc = new BaseController();
        private lGeneral lge = new lGeneral();
    
        private lConvert lc = new lConvert();
        public string execproses(string provider,string schema, string namefile)
        {
            string strout = "";
            var cstrname = dbconn.constringName("skylog");
            string namesp = schema + "." + namefile;

            if (provider == "postgresql")
            {
                JObject jo = new JObject();
                var conn = dbconn.constringList(provider, cstrname);
                NpgsqlTransaction trans;
                NpgsqlConnection connection = new NpgsqlConnection(conn);
                connection.Open();
                trans = connection.BeginTransaction();
                try
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(namesp, connection, trans);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 1300;
                    cmd.ExecuteNonQuery();
                  
                    trans.Commit();
                    strout = "success";
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    strout = ex.Message;
                }
                connection.Close();
                NpgsqlConnection.ClearPool(connection);

            }
            else
            {
                JObject jo = new JObject();
                var conn = dbconn.constringList(provider, cstrname);
                
                SqlTransaction trans;
                SqlConnection connection = new SqlConnection(conn);
                connection.Open();
                trans = connection.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand(namesp, connection, trans);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 600;
                    cmd.ExecuteNonQuery();

                    trans.Commit();
                    strout = "success";
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    strout = ex.Message;
                }
                connection.Close();
                SqlConnection.ClearPool(connection);
            }
           
            return strout;
        }

        public string UpdateSmsContentApprovalStatus(JObject json)
        {
            string strout = "";
            JObject jo = new JObject();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            var cstrname_en = dbconn.constringName("skyen");

            var conn = dbconn.constringList_v2(provider, cstrname);
            var conn_en = dbconn.constringList_v2(provider, cstrname_en);

            NpgsqlTransaction trans;
            NpgsqlConnection connection = new NpgsqlConnection(conn);
            connection.Open();
            trans = connection.BeginTransaction();

            NpgsqlTransaction trans_en;
            NpgsqlConnection connection_en = new NpgsqlConnection(conn_en);
            connection_en.Open();
            trans_en = connection_en.BeginTransaction();

            NpgsqlCommand cmd;
            try
            {
                cmd = new NpgsqlCommand("version_crud.update_smscontent_approval_status", connection, trans);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id", Convert.ToInt32(json.GetValue("id").ToString()));
                cmd.Parameters.AddWithValue("p_user", Convert.ToString(json.GetValue("approval_usr").ToString()));
                cmd.Parameters.AddWithValue("p_status", Convert.ToString(json.GetValue("status").ToString()));
                cmd.Parameters.AddWithValue("p_sync", Convert.ToString(json.GetValue("sync").ToString()));
                cmd.ExecuteNonQuery();

                cmd = new NpgsqlCommand("masters.update_version_smscontent_approval_status", connection_en, trans_en);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id", Convert.ToInt32(json.GetValue("id").ToString()));
                cmd.Parameters.AddWithValue("p_user", Convert.ToString(json.GetValue("approval_usr").ToString()));
                cmd.Parameters.AddWithValue("p_status", Convert.ToString(json.GetValue("status").ToString()));
                cmd.Parameters.AddWithValue("p_sync", Convert.ToString(json.GetValue("sync").ToString()));
                cmd.ExecuteNonQuery();


                trans.Commit();
                trans_en.Commit();
                strout = "success";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                trans_en.Rollback();
                strout = ex.Message;
            }

            connection.Close();
            connection_en.Close();
            NpgsqlConnection.ClearPool(connection);
            NpgsqlConnection.ClearPool(connection_en);
            return strout;
        }

        public string UpdateCallScriptApprovalStatus(JObject json)
        {
            string strout = "";
            JObject jo = new JObject();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            var cstrname_en = dbconn.constringName("skyen");

            var conn = dbconn.constringList_v2(provider, cstrname);
            var conn_en = dbconn.constringList_v2(provider, cstrname_en);

            NpgsqlTransaction trans;
            NpgsqlConnection connection = new NpgsqlConnection(conn);
            connection.Open();
            trans = connection.BeginTransaction();

            NpgsqlTransaction trans_en;
            NpgsqlConnection connection_en = new NpgsqlConnection(conn_en);
            connection_en.Open();
            trans_en = connection_en.BeginTransaction();

            NpgsqlCommand cmd;
            try
            {
                cmd = new NpgsqlCommand("version_crud.update_callscript_approval_status", connection, trans);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id", Convert.ToInt32(json.GetValue("id").ToString()));
                cmd.Parameters.AddWithValue("p_user", Convert.ToString(json.GetValue("approval_usr").ToString()));
                cmd.Parameters.AddWithValue("p_status", Convert.ToString(json.GetValue("status").ToString()));
                cmd.Parameters.AddWithValue("p_sync", Convert.ToString(json.GetValue("sync").ToString()));
                cmd.ExecuteNonQuery();

                cmd = new NpgsqlCommand("masters.update_version_callscript_approval_status", connection_en, trans_en);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id", Convert.ToInt32(json.GetValue("id").ToString()));
                cmd.Parameters.AddWithValue("p_user", Convert.ToString(json.GetValue("approval_usr").ToString()));
                cmd.Parameters.AddWithValue("p_status", Convert.ToString(json.GetValue("status").ToString()));
                cmd.Parameters.AddWithValue("p_sync", Convert.ToString(json.GetValue("sync").ToString()));
                cmd.ExecuteNonQuery();


                trans.Commit();
                trans_en.Commit();
                strout = "success";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                trans_en.Rollback();
                strout = ex.Message;
            }

            connection.Close();
            connection_en.Close();
            NpgsqlConnection.ClearPool(connection);
            NpgsqlConnection.ClearPool(connection_en);
            return strout;
        }

        public string UpdateReasonApprovalStatus(JObject json)
        {
            string strout = "";
            JObject jo = new JObject();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");
            var cstrname_en = dbconn.constringName("skyen");

            var conn = dbconn.constringList_v2(provider, cstrname);
            var conn_en = dbconn.constringList_v2(provider, cstrname_en);

            NpgsqlTransaction trans;
            NpgsqlConnection connection = new NpgsqlConnection(conn);
            connection.Open();
            trans = connection.BeginTransaction();

            NpgsqlTransaction trans_en;
            NpgsqlConnection connection_en = new NpgsqlConnection(conn_en);
            connection_en.Open();
            trans_en = connection_en.BeginTransaction();

            NpgsqlCommand cmd;
            try
            {
                cmd = new NpgsqlCommand("version_crud.update_reason_approval_status", connection, trans);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id", Convert.ToInt32(json.GetValue("id").ToString()));
                cmd.Parameters.AddWithValue("p_user", Convert.ToString(json.GetValue("approval_usr").ToString()));
                cmd.Parameters.AddWithValue("p_status", Convert.ToString(json.GetValue("status").ToString()));
                cmd.Parameters.AddWithValue("p_sync", Convert.ToString(json.GetValue("sync").ToString()));
                cmd.ExecuteNonQuery();

                cmd = new NpgsqlCommand("masters.update_version_reason_approval_status", connection_en, trans_en);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id", Convert.ToInt32(json.GetValue("id").ToString()));
                cmd.Parameters.AddWithValue("p_user", Convert.ToString(json.GetValue("approval_usr").ToString()));
                cmd.Parameters.AddWithValue("p_status", Convert.ToString(json.GetValue("status").ToString()));
                cmd.Parameters.AddWithValue("p_sync", Convert.ToString(json.GetValue("sync").ToString()));
                cmd.ExecuteNonQuery();


                trans.Commit();
                trans_en.Commit();
                strout = "success";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                trans_en.Rollback();
                strout = ex.Message;
            }

            connection.Close();
            connection_en.Close();
            NpgsqlConnection.ClearPool(connection);
            NpgsqlConnection.ClearPool(connection_en);
            return strout;
        }


        public string Updatespvfc(JObject json)
        {
            string strout = "";
            JObject jo = new JObject();
            var provider = dbconn.sqlprovider();
            //var cstrname = dbconn.constringName("skycoll");
            var cstrname_core = dbconn.constringName("skycore");

            //var conn = dbconn.constringList_v2(provider, cstrname);
            var conn_core = dbconn.constringList_v2(provider, cstrname_core);

            //NpgsqlTransaction trans;
            //NpgsqlConnection connection = new NpgsqlConnection(conn);
            //connection.Open();
            //trans = connection.BeginTransaction();

            NpgsqlTransaction trans_core;
            NpgsqlConnection connection_core = new NpgsqlConnection(conn_core);
            connection_core.Open();
            trans_core = connection_core.BeginTransaction();

            NpgsqlCommand cmd;
            try
            {
                //cmd = new NpgsqlCommand("public.usr_updateuserspvfc", connection, trans);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("p_usrid", Convert.ToInt32(json.GetValue("usrid").ToString()));
                //cmd.Parameters.AddWithValue("p_spvid", Convert.ToInt32(json.GetValue("svpid").ToString()));
                //cmd.ExecuteNonQuery();

                cmd = new NpgsqlCommand("public.usr_updateuserfc", connection_core, trans_core);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_usrid", Convert.ToInt32(json.GetValue("usrid").ToString()));
                cmd.Parameters.AddWithValue("p_spvname", Convert.ToString(json.GetValue("usrspvname").ToString()));
                cmd.ExecuteNonQuery();


                //trans.Commit();
                trans_core.Commit();
                strout = "success";

            }
            catch (Exception ex)
            {
                //trans.Rollback();
                trans_core.Rollback();
                strout = ex.Message;
            }

            //connection.Close();
            connection_core.Close();
            //NpgsqlConnection.ClearPool(connection);
            NpgsqlConnection.ClearPool(connection_core);
            return strout;
        }

        public string insertdeskcallcontact(JObject json)
        {
            string strout = "";
            JObject jo = new JObject();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");

            var conn = dbconn.constringList_v2(provider, cstrname);

            NpgsqlTransaction trans;
            NpgsqlConnection connection = new NpgsqlConnection(conn);
            connection.Open();
            trans = connection.BeginTransaction();

       

            NpgsqlCommand cmd;
            try
            {
                cmd = new NpgsqlCommand("public.insert_deskcall_contact", connection, trans);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_cif", Convert.ToString(json.GetValue("cif").ToString()));
                cmd.Parameters.AddWithValue("p_accno", Convert.ToString(json.GetValue("accno").ToString()));
                cmd.Parameters.AddWithValue("p_phone", Convert.ToString(json.GetValue("phoneno").ToString()));
                cmd.Parameters.AddWithValue("p_address", Convert.ToString(json.GetValue("address").ToString()));
                cmd.Parameters.AddWithValue("p_city", Convert.ToString(json.GetValue("city").ToString()));
                cmd.Parameters.AddWithValue("p_form", Convert.ToString(json.GetValue("form").ToString()));
                cmd.Parameters.AddWithValue("p_usrid", Convert.ToInt32(json.GetValue("usrid").ToString()));
                cmd.ExecuteNonQuery();

              
                trans.Commit();
                strout = "success";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                strout = ex.Message;
            }

            connection.Close();
            NpgsqlConnection.ClearPool(connection);
            return strout;
        }

        public string insertdeskcalldc(JObject json)
        {
            string strout = "";
            JObject jo = new JObject();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");

            var conn = dbconn.constringList_v2(provider, cstrname);

            NpgsqlTransaction trans;
            NpgsqlConnection connection = new NpgsqlConnection(conn);
            connection.Open();
            trans = connection.BeginTransaction();



            NpgsqlCommand cmd;
            try
            {
                cmd = new NpgsqlCommand("public.update_deskcall_add_contact", connection, trans);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_loanid", Convert.ToInt32(json.GetValue("id").ToString()));
                cmd.Parameters.AddWithValue("p_addid", Convert.ToInt32(json.GetValue("addid").ToString()));
                cmd.Parameters.AddWithValue("p_callname", Convert.ToString(json.GetValue("name").ToString()));
                cmd.Parameters.AddWithValue("p_idreason", Convert.ToString(json.GetValue("reasonid").ToString()));
                cmd.Parameters.AddWithValue("p_idresult", Convert.ToString(json.GetValue("resultid").ToString()));
                cmd.Parameters.AddWithValue("p_note", Convert.ToString(json.GetValue("notes").ToString()));
                cmd.ExecuteNonQuery();

                cmd = new NpgsqlCommand("public.insert_deskcall_dc", connection, trans);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_callid", Convert.ToString(json.GetValue("callid").ToString()));
                cmd.Parameters.AddWithValue("p_brncid", Convert.ToString(json.GetValue("brncid").ToString()));
                cmd.Parameters.AddWithValue("p_accno", Convert.ToString(json.GetValue("accno").ToString()));
                cmd.Parameters.AddWithValue("p_name", Convert.ToString(json.GetValue("name").ToString()));
                cmd.Parameters.AddWithValue("p_idreason", Convert.ToString(json.GetValue("reasonid").ToString()));
                cmd.Parameters.AddWithValue("p_idresult", Convert.ToString(json.GetValue("resultid").ToString()));
                cmd.Parameters.AddWithValue("p_amount", Convert.ToString(json.GetValue("amount").ToString()));
                cmd.Parameters.AddWithValue("p_note", Convert.ToString(json.GetValue("notes").ToString()));
                cmd.Parameters.AddWithValue("p_historyby", Convert.ToString(json.GetValue("usrid").ToString()));
                cmd.Parameters.AddWithValue("p_longitude", Convert.ToString(json.GetValue("longitude").ToString()));
                cmd.Parameters.AddWithValue("p_latitude", Convert.ToString(json.GetValue("latitude").ToString()));
                cmd.Parameters.AddWithValue("p_kolek", Convert.ToString(json.GetValue("kolek").ToString()));
                cmd.Parameters.AddWithValue("p_callresulthh", Convert.ToString(json.GetValue("resultdate").ToString()));
                //cmd.Parameters.AddWithValue("p_callresulthhmm", Convert.ToString(json.GetValue("callresulthhmm").ToString()));
                cmd.Parameters.AddWithValue("p_callresultmm", Convert.ToString(json.GetValue("resulttime").ToString()));
                cmd.Parameters.AddWithValue("p_dpd", Convert.ToString(json.GetValue("dpd").ToString()));
                cmd.Parameters.AddWithValue("p_callby", Convert.ToString(json.GetValue("callby").ToString()));
                cmd.ExecuteNonQuery();


                trans.Commit();
                strout = "success";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                strout = ex.Message;
            }

            connection.Close();
            NpgsqlConnection.ClearPool(connection);
            return strout;
        }


        public string insertrequestcall(JObject json)
        {
            string strout = "";
            JObject jo = new JObject();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");

            var conn = dbconn.constringList_v2(provider, cstrname);

            NpgsqlTransaction trans;
            NpgsqlConnection connection = new NpgsqlConnection(conn);
            connection.Open();
            trans = connection.BeginTransaction();
            List<dynamic> retObject = new List<dynamic>();


            NpgsqlCommand cmd;
            try
            {
                cmd = new NpgsqlCommand("public.insert_deskcall_request_call", connection, trans);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_callid", Convert.ToString(json.GetValue("callid").ToString()));
                cmd.Parameters.AddWithValue("p_phoneno", Convert.ToString(json.GetValue("phoneno").ToString()));
                cmd.Parameters.AddWithValue("p_statusid", Convert.ToString(json.GetValue("statusid").ToString()));
                cmd.Parameters.AddWithValue("p_usrid", Convert.ToString(json.GetValue("usrid").ToString()));
                NpgsqlDataReader dr = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                retObject = bc.GetDataObj(dr);
                dr.Close();
                long id = retObject[0].id;

                trans.Commit();
                strout = "success|" + id;

            }
            catch (Exception ex)
            {
                trans.Rollback();
                strout = ex.Message;
            }

            connection.Close();
            NpgsqlConnection.ClearPool(connection);
            return strout;
        }

        public string updaterequestcallbyid(string id)
        {
            string strout = "";
            JObject jo = new JObject();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");

            var conn = dbconn.constringList_v2(provider, cstrname);

            NpgsqlTransaction trans;
            NpgsqlConnection connection = new NpgsqlConnection(conn);
            connection.Open();
            trans = connection.BeginTransaction();



            NpgsqlCommand cmd;
            try
            {
                cmd = new NpgsqlCommand("public.update_status_request_call", connection, trans);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id", Convert.ToInt32(id));
                cmd.ExecuteNonQuery();


                trans.Commit();
                strout = "success";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                strout = ex.Message;
            }

            connection.Close();
            NpgsqlConnection.ClearPool(connection);
            return strout;
        }


        public string insert_fieldcoll_hasil_visit(JObject json)
        {
            string strout = "";
            JObject jo = new JObject();
            var jaData = new JArray();
            List<dynamic> retObject = new List<dynamic>();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");

            var conn = dbconn.constringList_v2(provider, cstrname);

            NpgsqlTransaction trans;
            NpgsqlConnection connection = new NpgsqlConnection(conn);
            connection.Open();
            trans = connection.BeginTransaction();
            NpgsqlCommand cmd;
            NpgsqlDataReader dr;
            try
            {
                cmd = new NpgsqlCommand("public.update_fieldcall_hasil_visit", connection, trans);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_loanid", Convert.ToInt32(json.GetValue("id").ToString()));
                cmd.Parameters.AddWithValue("p_callid", Convert.ToInt32(json.GetValue("callid").ToString()));
                cmd.Parameters.AddWithValue("p_callname", Convert.ToString(json.GetValue("name").ToString()));
                cmd.Parameters.AddWithValue("p_idreason", Convert.ToString(json.GetValue("reasonid").ToString()));
                cmd.Parameters.AddWithValue("p_note", Convert.ToString(json.GetValue("notes").ToString()));
                cmd.ExecuteNonQuery();

                cmd = new NpgsqlCommand("public.insert_history_hasil_visit_fc", connection, trans);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Transaction = trans;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("p_callid", Convert.ToString(json.GetValue("callid").ToString()));
                cmd.Parameters.AddWithValue("p_brncid", Convert.ToString(json.GetValue("brncid").ToString()));
                cmd.Parameters.AddWithValue("p_accno", Convert.ToString(json.GetValue("accno").ToString()));
                cmd.Parameters.AddWithValue("p_name", Convert.ToString(json.GetValue("name").ToString()));
                cmd.Parameters.AddWithValue("p_idreason", Convert.ToString(json.GetValue("reasonid").ToString()));
                cmd.Parameters.AddWithValue("p_idresult", Convert.ToString(json.GetValue("resultid").ToString()));
                cmd.Parameters.AddWithValue("p_note", Convert.ToString(json.GetValue("notes").ToString()));
                cmd.Parameters.AddWithValue("p_historyby", Convert.ToString(json.GetValue("usrid").ToString()));
                cmd.Parameters.AddWithValue("p_longitude", Convert.ToString(json.GetValue("longitude").ToString()));
                cmd.Parameters.AddWithValue("p_latitude", Convert.ToString(json.GetValue("latitude").ToString()));
                cmd.Parameters.AddWithValue("p_kolek", Convert.ToString(json.GetValue("kolek").ToString()));
                cmd.Parameters.AddWithValue("p_dpd", Convert.ToString(json.GetValue("dpd").ToString()));
                cmd.Parameters.AddWithValue("p_callby", Convert.ToString(json.GetValue("callby").ToString()));
                dr = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                retObject = bc.GetDataObj(dr);
                dr.Close();
                long idhistory = retObject[0].idhistory;

                jaData = JArray.Parse(json.GetValue("photoid").ToString());

                if (jaData.Count > 0)
                {
                    for (int i = 0; i < jaData.Count; i++)
                    {
                        var joRawdata = JObject.Parse(jaData[i].ToString());
                        cmd = new NpgsqlCommand("public.update_fieldcall_photo", connection, trans);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_id", Convert.ToInt32(joRawdata.GetValue("id").ToString()));
                        cmd.Parameters.AddWithValue("p_idhistory", Convert.ToInt32(idhistory));
                        cmd.ExecuteNonQuery();
                    }

                }

                trans.Commit();
                strout = "success";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                strout = ex.Message;
            }

            connection.Close();
            NpgsqlConnection.ClearPool(connection);
            return strout;
        }

        public string insert_fieldcoll_janjibayar(JObject json)
        {
            string strout = "";
            JObject jo = new JObject();
            var jaData = new JArray();
            List<dynamic> retObject = new List<dynamic>();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");

            var conn = dbconn.constringList_v2(provider, cstrname);

            NpgsqlTransaction trans;
            NpgsqlConnection connection = new NpgsqlConnection(conn);
            connection.Open();
            trans = connection.BeginTransaction();
            NpgsqlCommand cmd;
            NpgsqlDataReader dr;
            try
            {
                cmd = new NpgsqlCommand("public.update_fieldcall_janji_bayar", connection, trans);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_loanid", Convert.ToInt32(json.GetValue("id").ToString()));
                cmd.Parameters.AddWithValue("p_callid", Convert.ToInt32(json.GetValue("callid").ToString()));
                cmd.Parameters.AddWithValue("p_callname", Convert.ToString(json.GetValue("name").ToString()));
                cmd.Parameters.AddWithValue("p_idresult", Convert.ToInt32("9"));
                cmd.Parameters.AddWithValue("p_amount", Convert.ToString(json.GetValue("amount").ToString()));
                cmd.Parameters.AddWithValue("p_note", Convert.ToString(json.GetValue("notes").ToString()));
                cmd.Parameters.AddWithValue("p_tgljanji", Convert.ToString(json.GetValue("tgljanji").ToString()));
                cmd.Parameters.AddWithValue("p_jam", Convert.ToString(json.GetValue("jam").ToString()));
                cmd.ExecuteNonQuery();

                cmd = new NpgsqlCommand("public.insert_history_jani_bayar_fc", connection, trans);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Transaction = trans;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("p_callid", Convert.ToString(json.GetValue("callid").ToString()));
                cmd.Parameters.AddWithValue("p_brncid", Convert.ToString(json.GetValue("brncid").ToString()));
                cmd.Parameters.AddWithValue("p_accno", Convert.ToString(json.GetValue("accno").ToString()));
                cmd.Parameters.AddWithValue("p_name", Convert.ToString(json.GetValue("name").ToString()));
                cmd.Parameters.AddWithValue("p_idreason", Convert.ToString(json.GetValue("reasonid").ToString()));
                cmd.Parameters.AddWithValue("p_idresult", Convert.ToString("9"));
                cmd.Parameters.AddWithValue("p_amount", Convert.ToString(json.GetValue("amount").ToString()));
                cmd.Parameters.AddWithValue("p_note", Convert.ToString(json.GetValue("notes").ToString()));
                cmd.Parameters.AddWithValue("p_historyby", Convert.ToString(json.GetValue("usrid").ToString()));
                cmd.Parameters.AddWithValue("p_longitude", Convert.ToString(json.GetValue("longitude").ToString()));
                cmd.Parameters.AddWithValue("p_latitude", Convert.ToString(json.GetValue("latitude").ToString()));
                cmd.Parameters.AddWithValue("p_kolek", Convert.ToString(json.GetValue("kolek").ToString()));
                cmd.Parameters.AddWithValue("p_dpd", Convert.ToString(json.GetValue("dpd").ToString()));
                cmd.Parameters.AddWithValue("p_tgljanji", Convert.ToString(json.GetValue("tgljanji").ToString()));
                cmd.Parameters.AddWithValue("p_jam", Convert.ToString(json.GetValue("jam").ToString()));
                cmd.Parameters.AddWithValue("p_callby", Convert.ToString(json.GetValue("callby").ToString()));
                dr = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                retObject = bc.GetDataObj(dr);
                dr.Close();
                long idhistory = retObject[0].idhistory;

                jaData = JArray.Parse(json.GetValue("photoid").ToString());

                if (jaData.Count > 0)
                {
                    for (int i = 0; i < jaData.Count; i++)
                    {
                        var joRawdata = JObject.Parse(jaData[i].ToString());
                        cmd = new NpgsqlCommand("public.update_fieldcall_photo", connection, trans);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_id", Convert.ToInt32(joRawdata.GetValue("id").ToString()));
                        cmd.Parameters.AddWithValue("p_idhistory", Convert.ToInt32(idhistory));
                        cmd.ExecuteNonQuery();
                    }

                }

                trans.Commit();
                strout = "success";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                strout = ex.Message;
            }

            connection.Close();
            NpgsqlConnection.ClearPool(connection);
            return strout;
        }

        public string insertuploadphoto(JObject json)
        {
            string strout = "";
            JObject jo = new JObject();
            var jaData = new JArray();
            List<dynamic> retObject = new List<dynamic>();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");

            var conn = dbconn.constringList_v2(provider, cstrname);

            NpgsqlTransaction trans;
            NpgsqlConnection connection = new NpgsqlConnection(conn);
            connection.Open();
            trans = connection.BeginTransaction();
            NpgsqlCommand cmd;
            try
            {
                cmd = new NpgsqlCommand("public.insert_fieldcall_photo", connection, trans);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_title", Convert.ToString(json.GetValue("title").ToString()));
                cmd.Parameters.AddWithValue("p_desc", Convert.ToString(json.GetValue("desc").ToString()));
                cmd.Parameters.AddWithValue("p_url", Convert.ToString(json.GetValue("url").ToString()));
                cmd.Parameters.AddWithValue("p_latitude", Convert.ToString(json.GetValue("latitude").ToString()));
                cmd.Parameters.AddWithValue("p_longitude", Convert.ToString(json.GetValue("longitude").ToString()));
                cmd.Parameters.AddWithValue("p_usrid", Convert.ToInt32(json.GetValue("usrid").ToString()));
                NpgsqlDataReader dr = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                retObject = bc.GetDataObj(dr);
                dr.Close();
                long id = retObject[0].idphoto;

                trans.Commit();
                strout = "success|" + id;

            }
            catch (Exception ex)
            {
                trans.Rollback();
                strout = ex.Message;
            }

            connection.Close();
            NpgsqlConnection.ClearPool(connection);
            return strout;
        }

        public string insertuploadphotocontact(JObject json)
        {
            string strout = "";
            JObject jo = new JObject();
            var jaData = new JArray();
            List<dynamic> retObject = new List<dynamic>();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");

            var conn = dbconn.constringList_v2(provider, cstrname);

            NpgsqlTransaction trans;
            NpgsqlConnection connection = new NpgsqlConnection(conn);
            connection.Open();
            trans = connection.BeginTransaction();
            NpgsqlCommand cmd;
            try
            {
                cmd = new NpgsqlCommand("public.insert_fieldcall_contact_photo", connection, trans);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_title", Convert.ToString(json.GetValue("title").ToString()));
                cmd.Parameters.AddWithValue("p_desc", Convert.ToString(json.GetValue("desc").ToString()));
                cmd.Parameters.AddWithValue("p_url", Convert.ToString(json.GetValue("url").ToString()));
                cmd.Parameters.AddWithValue("p_latitude", Convert.ToString(json.GetValue("latitude").ToString()));
                cmd.Parameters.AddWithValue("p_longitude", Convert.ToString(json.GetValue("longitude").ToString()));
                cmd.Parameters.AddWithValue("p_usrid", Convert.ToInt32(json.GetValue("usrid").ToString()));
                NpgsqlDataReader dr = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                retObject = bc.GetDataObj(dr);
                dr.Close();
                long id = retObject[0].idphoto;

                trans.Commit();
                strout = "success|" + id;

            }
            catch (Exception ex)
            {
                trans.Rollback();
                strout = ex.Message;
            }

            connection.Close();
            NpgsqlConnection.ClearPool(connection);
            return strout;
        }

        public string insertfieldcollcontact(JObject json)
        {
            string strout = "";
            JObject jo = new JObject();
            List<dynamic> retObject = new List<dynamic>();
            var jaData = new JArray();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");

            var conn = dbconn.constringList_v2(provider, cstrname);

            NpgsqlTransaction trans;
            NpgsqlConnection connection = new NpgsqlConnection(conn);
            connection.Open();
            trans = connection.BeginTransaction();



            NpgsqlCommand cmd;
            try
            {
                cmd = new NpgsqlCommand("public.insert_fieldcoll_contact", connection, trans);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_cif", Convert.ToString(json.GetValue("cif").ToString()));
                cmd.Parameters.AddWithValue("p_accno", Convert.ToString(json.GetValue("accno").ToString()));
                cmd.Parameters.AddWithValue("p_phone", Convert.ToString(json.GetValue("phoneno").ToString()));
                cmd.Parameters.AddWithValue("p_address", Convert.ToString(json.GetValue("address").ToString()));
                cmd.Parameters.AddWithValue("p_city", Convert.ToString(json.GetValue("city").ToString()));
                cmd.Parameters.AddWithValue("p_form", Convert.ToString(json.GetValue("form").ToString()));
                cmd.Parameters.AddWithValue("p_latitude", Convert.ToString(json.GetValue("latitude").ToString()));
                cmd.Parameters.AddWithValue("p_longitude", Convert.ToString(json.GetValue("longitude").ToString()));
                cmd.Parameters.AddWithValue("p_usrid", Convert.ToInt32(json.GetValue("usrid").ToString()));
                NpgsqlDataReader dr = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                retObject = bc.GetDataObj(dr);
                dr.Close();
                long idadd = retObject[0].vidadd;
                jaData = JArray.Parse(json.GetValue("photoid").ToString());

                if (jaData.Count > 0)
                {
                    for (int i = 0; i < jaData.Count; i++)
                    {
                        var joRawdata = JObject.Parse(jaData[i].ToString());
                        cmd = new NpgsqlCommand("public.update_fieldcall_contact_photo", connection, trans);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_id", Convert.ToInt32(joRawdata.GetValue("id").ToString()));
                        cmd.Parameters.AddWithValue("p_idadd", Convert.ToInt32(idadd));
                        cmd.ExecuteNonQuery();
                    }

                }

                trans.Commit();
                strout = "success";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                strout = ex.Message;
            }

            connection.Close();
            NpgsqlConnection.ClearPool(connection);
            return strout;
        }


        public string insertbucket(JObject json)
        {
            string strout = "";
            JObject jo = new JObject();
            List<dynamic> retObject = new List<dynamic>();
            var jaData = new JArray();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");

            var conn = dbconn.constringList_v2(provider, cstrname);

            NpgsqlTransaction trans;
            NpgsqlConnection connection = new NpgsqlConnection(conn);
            connection.Open();
            trans = connection.BeginTransaction();



            NpgsqlCommand cmd;
            try
            {
                cmd = new NpgsqlCommand("public.insert_bucket", connection, trans);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_code", Convert.ToString(json.GetValue("code").ToString()));
                cmd.Parameters.AddWithValue("p_name", Convert.ToString(json.GetValue("name").ToString()));
                cmd.Parameters.AddWithValue("p_userid", Convert.ToString(json.GetValue("usr").ToString()));
                cmd.Parameters.AddWithValue("p_logid", Convert.ToInt32(json.GetValue("logid").ToString()));
                NpgsqlDataReader dr = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                retObject = bc.GetDataObj(dr);
                dr.Close();
                int vid = retObject[0].vid;
                jaData = JArray.Parse(json.GetValue("detail").ToString());

                if (jaData.Count > 0)
                {
                    for (int i = 0; i < jaData.Count; i++)
                    {
                        var joRawdata = JObject.Parse(jaData[i].ToString());
                        cmd = new NpgsqlCommand("public.insert_bucket_detail", connection, trans);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_bct_id", Convert.ToInt32(vid));
                        cmd.Parameters.AddWithValue("p_usrid", Convert.ToInt32(joRawdata.GetValue("usrid").ToString()));
                       
                        cmd.ExecuteNonQuery();
                    }

                }

                trans.Commit();
                strout = "success";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                strout = ex.Message;
            }

            connection.Close();
            NpgsqlConnection.ClearPool(connection);
            return strout;
        }

        public string updatebucket(JObject json)
        {
            string strout = "";
            JObject jo = new JObject();
            List<dynamic> retObject = new List<dynamic>();
            var jaData = new JArray();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");

            var conn = dbconn.constringList_v2(provider, cstrname);

            NpgsqlTransaction trans;
            NpgsqlConnection connection = new NpgsqlConnection(conn);
            connection.Open();
            trans = connection.BeginTransaction();

            NpgsqlCommand cmd;
            try
            {
                cmd = new NpgsqlCommand("public.update_bucket", connection, trans);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id", Convert.ToInt32(json.GetValue("id").ToString()));
                cmd.Parameters.AddWithValue("p_code", Convert.ToString(json.GetValue("code").ToString()));
                cmd.Parameters.AddWithValue("p_name", Convert.ToString(json.GetValue("name").ToString()));
                cmd.Parameters.AddWithValue("p_userid", Convert.ToString(json.GetValue("usr").ToString()));
                cmd.Parameters.AddWithValue("p_logid", Convert.ToInt32(json.GetValue("logid").ToString()));
                cmd.ExecuteNonQuery();


                jaData = JArray.Parse(json.GetValue("detail").ToString());

                if (jaData.Count > 0)
                {
                    cmd = new NpgsqlCommand("public.delete_bucket_detail_by_bct_id", connection, trans);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_bct_id", Convert.ToInt32(json.GetValue("id").ToString()));
                    cmd.ExecuteNonQuery();


                    for (int i = 0; i < jaData.Count; i++)
                    {
                        var joRawdata = JObject.Parse(jaData[i].ToString());
                        cmd = new NpgsqlCommand("public.insert_bucket_detail", connection, trans);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_bct_id", Convert.ToInt32(json.GetValue("id").ToString()));
                        cmd.Parameters.AddWithValue("p_usrid", Convert.ToInt32(joRawdata.GetValue("usrid").ToString()));

                        cmd.ExecuteNonQuery();
                    }

                }

                trans.Commit();
                strout = "success";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                strout = ex.Message;
            }

            connection.Close();
            NpgsqlConnection.ClearPool(connection);
            return strout;
        }


        public string deletebucket(JObject json)
        {
            string strout = "";
            JObject jo = new JObject();
            List<dynamic> retObject = new List<dynamic>();
            var jaData = new JArray();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");

            var conn = dbconn.constringList_v2(provider, cstrname);

            NpgsqlTransaction trans;
            NpgsqlConnection connection = new NpgsqlConnection(conn);
            connection.Open();
            trans = connection.BeginTransaction();

            NpgsqlCommand cmd;
            try
            {
                cmd = new NpgsqlCommand("public.delete_bucket_byid", connection, trans);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id", Convert.ToInt32(json.GetValue("id").ToString()));
                cmd.Parameters.AddWithValue("p_userid", Convert.ToString(json.GetValue("usr").ToString()));
                cmd.Parameters.AddWithValue("p_logid", Convert.ToInt32(json.GetValue("logid").ToString()));
                cmd.ExecuteNonQuery();

                trans.Commit();
                strout = "success";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                strout = ex.Message;
            }

            connection.Close();
            NpgsqlConnection.ClearPool(connection);
            return strout;
        }


        public string insertnotifikasi(JObject json)
        {
            string strout = "";
            JObject jo = new JObject();
            List<dynamic> retObject = new List<dynamic>();
            var jaData = new JArray();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");

            var conn = dbconn.constringList_v2(provider, cstrname);

            NpgsqlTransaction trans;
            NpgsqlConnection connection = new NpgsqlConnection(conn);
            connection.Open();
            trans = connection.BeginTransaction();



            NpgsqlCommand cmd;
            try
            {
                cmd = new NpgsqlCommand("public.insert_notifikasi", connection, trans);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_code", Convert.ToString(json.GetValue("code").ToString()));
                cmd.Parameters.AddWithValue("p_form", Convert.ToString(json.GetValue("form").ToString()));
                cmd.Parameters.AddWithValue("p_msg", Convert.ToString(json.GetValue("msg").ToString()));
                cmd.Parameters.AddWithValue("p_status", Convert.ToBoolean(json.GetValue("status").ToString()));
                cmd.Parameters.AddWithValue("p_userid", Convert.ToString(json.GetValue("userid").ToString()));
                cmd.Parameters.AddWithValue("p_logid", Convert.ToInt32(json.GetValue("logid").ToString()));
                NpgsqlDataReader dr = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                retObject = bc.GetDataObj(dr);
                dr.Close();
                long vid = retObject[0].vid;
                jaData = JArray.Parse(json.GetValue("detail").ToString());

                if (jaData.Count > 0)
                {
                    for (int i = 0; i < jaData.Count; i++)
                    {
                        var joRawdata = JObject.Parse(jaData[i].ToString());
                        cmd = new NpgsqlCommand("public.insert_notifikasi_detail", connection, trans);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_ntf_id", Convert.ToInt32(vid));
                        cmd.Parameters.AddWithValue("p_to", Convert.ToString(joRawdata.GetValue("to").ToString()));

                        cmd.ExecuteNonQuery();
                    }

                }

                trans.Commit();
                strout = "success";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                strout = ex.Message;
            }

            connection.Close();
            NpgsqlConnection.ClearPool(connection);
            return strout;
        }

        public string updatenotifikasi(JObject json)
        {
            string strout = "";
            JObject jo = new JObject();
            List<dynamic> retObject = new List<dynamic>();
            var jaData = new JArray();
            var provider = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skycoll");

            var conn = dbconn.constringList_v2(provider, cstrname);

            NpgsqlTransaction trans;
            NpgsqlConnection connection = new NpgsqlConnection(conn);
            connection.Open();
            trans = connection.BeginTransaction();

            NpgsqlCommand cmd;
            try
            {
                cmd = new NpgsqlCommand("public.update_notifikasi", connection, trans);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_id", Convert.ToInt32(json.GetValue("id").ToString()));
                cmd.Parameters.AddWithValue("p_code", Convert.ToString(json.GetValue("code").ToString()));
                cmd.Parameters.AddWithValue("p_form", Convert.ToString(json.GetValue("form").ToString()));
                cmd.Parameters.AddWithValue("p_msg", Convert.ToString(json.GetValue("msg").ToString()));
                cmd.Parameters.AddWithValue("p_userid", Convert.ToString(json.GetValue("userid").ToString()));
                cmd.Parameters.AddWithValue("p_logid", Convert.ToInt32(json.GetValue("logid").ToString()));
                cmd.ExecuteNonQuery();


                jaData = JArray.Parse(json.GetValue("detail").ToString());

                if (jaData.Count > 0)
                {
                    cmd = new NpgsqlCommand("public.delete_notifikasi_detail_byid", connection, trans);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_ntf_id", Convert.ToInt32(json.GetValue("id").ToString()));
                    cmd.ExecuteNonQuery();


                    for (int i = 0; i < jaData.Count; i++)
                    {
                        var joRawdata = JObject.Parse(jaData[i].ToString());
                        cmd = new NpgsqlCommand("public.insert_notifikasi_detail", connection, trans);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_ntf_id", Convert.ToInt32(json.GetValue("id").ToString()));
                        cmd.Parameters.AddWithValue("p_to", Convert.ToString(joRawdata.GetValue("to").ToString()));

                        cmd.ExecuteNonQuery();
                    }

                }

                trans.Commit();
                strout = "success";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                strout = ex.Message;
            }

            connection.Close();
            NpgsqlConnection.ClearPool(connection);
            return strout;
        }
    }
}
