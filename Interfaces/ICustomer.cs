using sky.coll.General.Responses;
using System.Threading.Tasks;

namespace sky.coll.Interfaces
{
    public interface ICustomer
    {
        public  Task<(bool Error, GeneralResponses Return)> GetListNasabah();

    }
}
