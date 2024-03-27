using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace sky.coll.Entities
{
    public class master_customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        public string? cu_cif { get; set; }
        public string? cu_name { get; set; }
        public DateTime? cu_borndate { get; set; }
        public string? cu_bornplace { get; set; }
        public int? cu_idtype { get; set; }
        public string? cu_idnumber { get; set; }
        public int? cu_gender { get; set; }
        public int? cu_maritalstatus { get; set; }
        public int? cu_nationality { get; set; }
        public int? cu_incometype { get; set; }
        public string? cu_income { get; set; }
        public int? cu_cusstype { get; set; }
        public string? pekerjaan { get; set; }
        public string? jabatan { get; set; }
        public int? cu_occupation { get; set; }
        public string? cu_company { get; set; }
        public string? cu_email { get; set; }
        public string? cu_address { get; set; }
        public string? cu_rt { get; set; }
        public string? cu_rw { get; set; }
        public int? cu_kelurahan { get; set; }
        public string? kelurahan { get; set; }
        public string? kecamatan { get; set; }
        public int? cu_kecamatan { get; set; }
        public string? city { get; set; }
        public int? cu_city { get; set; }
        public string? provinsi { get; set; }
        public int? cu_provinsi { get; set; }
        public string? cu_zipcode { get; set; }
        public string? cu_hmphone { get; set; }
        public string? cu_mobilephone { get; set; }
        public int? branch_id { get; set; }
        public DateTime? stg_date { get; set; }

    }
}
