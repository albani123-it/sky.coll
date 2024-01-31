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

    }
}
