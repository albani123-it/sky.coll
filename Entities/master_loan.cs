using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;
using System;

namespace sky.coll.Entities
{
    public class master_loan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        public int customer_id { get; set; }

        public int prd_segment_id { get; set; }
        public string cu_cif { get; set; }
        public string acc_no { get; set; }
        public string ccy { get; set; }
        public int product { get; set; }
        public double plafond { get; set; }
        public DateTime maturity_date  { get; set; }
        public DateTime start_date { get; set; }

        public int sisa_tenor { get; set; }
        public int tenor { get; set; }
        public double installment_pokok { get; set; }
            public double interest_rate { get; set; }
        public double tunggakan_pokok { get; set; }
        public double tunggakan_bunga { get; set; }
        public double tunggakan_denda { get; set; }
        public double tunggakan_total { get; set; }
        public double kewajiban_total { get; set; }
        public DateTime last_pay_date { get; set; }
        public double outstanding { get; set; }
        public double pay_total { get; set; }
        public int dpd { get; set; }
        public int kolektibilitas { get; set; }
        public string econa_name { get; set; }
        public string econ_phone { get; set; }
        public string marketing_code { get; set; }
        public string channel_branch_code { get; set; }
        public int notaris_id { get; set; }
        public DateTime stg_date { get; set; }
        public string fasilitas { get; set; }
        public int status { get; set; }
        public string payin_account { get; set; }
        public DateTime file_date { get; set; }
        public string loan_number { get; set; }


    }
}
