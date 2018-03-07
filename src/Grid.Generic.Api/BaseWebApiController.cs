using Grid.Api.Infrastructure;
using System;
using System.Web.Http;
using Grid.Infrastructure;

namespace Grid.Generic.Api
{
    public enum ActionType
    {
        GET,
        POST,
        PUT,
        DELETE
    }
    public class BaseWebApiController: ApiController
    {
        public Principal WebUser => User as Principal;

        public IHttpActionResult WrapIntoApiResult<T>(Func<T> action, ActionType actionType = ActionType.GET)
        {
            return WrapIntoApiResult(action, InternalServerError, actionType);
        }
        public IHttpActionResult WrapIntoApiResult<T>(Func<T> action, Func<Exception, IHttpActionResult> onError, ActionType actionType = ActionType.GET)
        {
            try
            {
                var payload = action();
                var apiResult = new ApiResult<T>
                {
                    Status = true,
                    Message = "Success",
                    Result = payload
                };

                return Json(apiResult);
            }
            catch (Exception ex)
            {

                var apiResult = new ApiResult<T>
                {
                    Status = false,
                    Message = ex.Message
                };

                //ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(apiResult);
            }
        }
    }
}
