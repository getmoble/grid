using System.Web;
using System.Web.Mvc;

namespace Grid.Areas.Recruit
{
    public class CandidateAuthAttribute: AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return base.AuthorizeCore(httpContext);
        }
    }
}