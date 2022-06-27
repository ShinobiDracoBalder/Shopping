using Shooping.Common.Responses;

namespace Shopping.Web.Helpers
{
    public interface IMailHelper
    {
        Response SendMail(string toName, string toEmail, string subject, string body);
    }
}
