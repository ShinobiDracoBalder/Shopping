using Shooping.Common.Responses;
using Shopping.Web.Models;

namespace Shopping.Web.Interfaces
{
    public interface IOrdersHelper
    {
        Task<Response> ProcessOrderAsync(ShowCartViewModel model);
        Task<Response> CancelOrderAsync(int id);
    }
}
