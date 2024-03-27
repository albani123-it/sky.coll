using Microsoft.EntityFrameworkCore;
using sky.coll.DTO;
using sky.coll.General.Responses;
using sky.coll.Insfrastructures;
using sky.coll.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace sky.coll.Services
{
    public class CustomerServices:dbConfig,ICustomer
    {
        public async Task<(bool Error, GeneralResponses Return)> GetListNasabah()
        {
            try
            {
                var Data = await master_customer.Select(es => new NasabahDTO
                {
                    cu_cif=es.cu_cif,
                    cu_name=es.cu_name,
                    pekerjaan=es.pekerjaan,
                    cu_address=es.cu_address,
                    kelurahan=es.kelurahan,
                    kecamatan=es.kecamatan,
                    city=es.city,
                    provinsi=es.provinsi,
                    cu_mobilephone=es.cu_mobilephone
                }).ToListAsync();
                var Returns = new GeneralResponses()
                {
                    Error=false,
                    Message="OK",
                    Data=new GeneralContent()
                    {
                        listNasabahDTO=Data
                    }
                };
                return (Returns.Error, Returns);
            }
            catch(Exception ex)
            {
                var Returns = new GeneralResponses()
                {
                    Error=true,
                    Message=ex.Message
                };
                return (Returns.Error, Returns);
            }
        }
    }
}
