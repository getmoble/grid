namespace Grid.Generic.Api
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Filters;

    public class WebApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }
    }
}
