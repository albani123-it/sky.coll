using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sky.coll.General.Responses;
using sky.coll.Interfaces;
using System.Threading.Tasks;
using System;

namespace sky.coll.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("skycoll/[controller]")]
   
    public class NasabahController : Controller
    {
        private ICustomer _Customer { get; set; }
       public NasabahController(ICustomer Customer)
        {
            _Customer = Customer;
        }

        [HttpGet("GetListNasabah")]
        public async Task<ActionResult<GeneralResponses>> GetlistNasabah()
        {
            try
            {
                var GetData = await _Customer.GetListNasabah();
                if (GetData.Error == true)
                {
                    return BadRequest(GetData.Return);
                }
                else
                {
                    return Ok(GetData.Return);
                }

            }
            catch (Exception ex)
            {
                var Return = new GeneralResponses()
                {
                    Message = ex.Message,
                    Error = true
                };
                return BadRequest(Return);
            }
        }

    }
}
