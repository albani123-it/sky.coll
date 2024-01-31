using sky.coll.Controllers;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace sky.coll.Libs
{
   
    public class lPgsql
    {
        public lDbConn dbconn = new lDbConn();
        private BaseController bc = new BaseController();


        public List<dynamic> execSQLWithOutput(string str)
        {
            var dbprv = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skyen");
            var conn = dbconn.constringList_v2(dbprv, cstrname);

            NpgsqlConnection nconn = new NpgsqlConnection(conn);
            var retObject = new List<dynamic>();
            nconn.Open();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(str, nconn);
                cmd.CommandType = CommandType.Text;
                NpgsqlDataReader dr = cmd.ExecuteReader();
                if (dr == null || dr.FieldCount == 0)
                {
                    nconn.Close();
                    return retObject;
                }
                retObject = bc.GetDataObj(dr);
            }
            catch (Exception ex)
            {
                retObject = new List<dynamic>();
                dynamic row = new ExpandoObject();
                row.status = "Invalid";
                row.message = "Invalid (" + ex.Message + ").";
                retObject.Add((ExpandoObject)row);
            }
            nconn.Close();
            return retObject;
        }

        public void execSql(string sql)
        {
            var dbprv = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skyen");
            var conn = dbconn.constringList_v2(dbprv, cstrname);
            NpgsqlConnection nconn = new NpgsqlConnection(conn);
            nconn.Open();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(sql, nconn);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            catch
            {

            }
            nconn.Close();
        }

        public string execSqlWithExecption(string sql)
        {
            var dbprv = dbconn.sqlprovider();
            var cstrname = dbconn.constringName("skyen");
            var conn = dbconn.constringList_v2(dbprv, cstrname);
            string message = "";
            NpgsqlConnection nconn = new NpgsqlConnection(conn);
            nconn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand(sql, nconn);
            cmd.CommandType = CommandType.Text;
            try
            {
                cmd.ExecuteNonQuery();
                message = "success";
            }
            catch (NpgsqlException e)
            {
                message = e.Message;
            }
            finally
            {
                nconn.Close();
            }
            return message;
        }


        internal string GenerateScriptDTB(JArray jaData)
        {
            Random r = new Random();
            List<dynamic> retObject = new List<dynamic>();
            var spname = "gen_descision_dtb";
            //var qry1 = "";
            var qry2 = "";
            var output = "";

            var hdrid = "0";
            if (jaData.Count > 0)
            {
                hdrid = jaData[0]["dtd_dth_id"].ToString();
                spname += hdrid;
                //get header element
                var joHeader = new JObject();
                for (int i = 0; i < jaData.Count; i++)
                {
                    if (Convert.ToInt32(jaData[i]["dtd_counter"].ToString()) == 1)
                    {
                        joHeader = JObject.Parse(jaData[i].ToString());
                        output = jaData[i]["dtd_output"].ToString();
                    }
                }

                for (int i = 1; i < jaData.Count; i++)
                {
                    var hdr1 = "";
                    var hdr2 = "";
                    var start = 2;
                    var finish = 22;
                    var counter = 1;
                    var joDetail = new JObject();
                    joDetail = JObject.Parse(jaData[i].ToString());

                    for (int a = start; a < finish; a++)
                    {
                        var field = "";
                        field = "dtd_rule" + counter.ToString();
                        if (a == 2)
                        {
                            var x = joHeader.GetValue(field).ToString();
                            if (!string.IsNullOrWhiteSpace(joHeader.GetValue(field).ToString()))
                            {
                                var arrHdr = (joHeader.GetValue(field).ToString()).Split("|");
                                if (arrHdr.Length > 1)
                                {
                                    hdr1 = arrHdr[0].ToString();
                                    hdr2 = arrHdr[1].ToString();
                                }
                                else
                                {
                                    hdr1 = arrHdr[0].ToString();
                                    hdr2 = "varchar";
                                }

                                if (i == 1)
                                {
                                    if (hdr2.ToLower() == "integer")
                                    {
                                        qry2 += " (case when coalesce( " + hdr1 + ",0) <= " + joDetail.GetValue(field).ToString() + " ";
                                    }
                                    //else if (hdr2.ToLower() == "numeric" || hdr2.ToLower() == "decimal")
                                    else if (hdr2.ToLower().Contains("numeric") || hdr2.ToLower().Contains("decimal"))
                                    {
                                        qry2 += " (case when coalesce(" + hdr1 + ",0) <= " + joDetail.GetValue(field).ToString() + " ";
                                    }
                                    else
                                    {
                                        if ((joDetail.GetValue(field).ToString()).ToLower() == "any")
                                        {
                                            qry2 += " (case when " + hdr1 + " like '%%' ";
                                        }
                                        else
                                        {
                                            qry2 += " (case when " + hdr1 + " in (" + joDetail.GetValue(field).ToString() + ") ";
                                        }

                                    }
                                }
                                else
                                {
                                    if (hdr2.ToLower() == "integer")
                                    {
                                        qry2 += "  when coalesce(" + hdr1 + ",0) <= " + joDetail.GetValue(field).ToString() + " ";
                                    }
                                    //else if (hdr2.ToLower() == "numeric" || hdr2.ToLower() == "decimal")
                                    else if (hdr2.ToLower().Contains("numeric") || hdr2.ToLower().Contains("decimal"))
                                    {
                                        qry2 += "  when coalesce(" + hdr1 + ",0) <= " + joDetail.GetValue(field).ToString() + " ";
                                    }
                                    else
                                    {
                                        if ((joDetail.GetValue(field).ToString()).ToLower() == "any")
                                        {
                                            qry2 += "  when " + hdr1 + " like '%%' ";
                                        }
                                        else
                                        {
                                            qry2 += "  when " + hdr1 + " in (" + joDetail.GetValue(field).ToString() + ") ";
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(joHeader.GetValue(field).ToString()))
                            {
                                var arrHdr = (joHeader.GetValue(field).ToString()).Split("|");
                                if (arrHdr.Length > 1)
                                {
                                    hdr1 = arrHdr[0].ToString();
                                    hdr2 = arrHdr[1].ToString();
                                }
                                else
                                {
                                    hdr1 = arrHdr[0].ToString();
                                    hdr2 = "varchar";
                                }

                                if (hdr2.ToLower() == "integer")
                                {
                                    qry2 += " and coalesce(" + hdr1 + ",0) <= " + joDetail.GetValue(field).ToString() + " ";
                                }
                                //else if (hdr2.ToLower() == "numeric" || hdr2.ToLower() == "decimal")
                                else if (hdr2.ToLower().Contains("numeric") || hdr2.ToLower().Contains("decimal"))
                                {
                                    qry2 += " and coalesce(" + hdr1 + ",0) <= " + joDetail.GetValue(field).ToString() + " ";
                                }
                                else
                                {
                                    if ((joDetail.GetValue(field).ToString()).ToLower() == "any")
                                    {
                                        qry2 += " and " + hdr1 + " like '%%' ";
                                    }
                                    else
                                    {
                                        qry2 += " and " + hdr1 + " in (" + joDetail.GetValue(field).ToString() + ") ";
                                    }
                                }
                            }
                        }

                        counter = counter + 1;
                    }

                    qry2 += " then " + joDetail.GetValue("dtd_output").ToString();

                }
                qry2 += " end )";
            }

          
            return qry2;
        }
    }
}
