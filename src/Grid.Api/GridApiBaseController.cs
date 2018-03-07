using System;
using System.Linq;
using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Infrastructure;

namespace Grid.Api
{
    public class GridApiBaseController: Controller
    {
        public Principal WebUser => User as Principal;

        protected ApiResult<T> TryExecute<T>(Func<T> action, string message)
        {
            try
            {
                var result = action();
                return ApiResult<T>.Success(result, message);
            }
            catch (Exception ex)
            {

                return ApiResult<T>.Failure(ex);
            }
        }

        protected string GetModelErrors()
        {
            return string.Join(Environment.NewLine, ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage + " " + v.Exception));
        }

        protected ApiResult<T> ApiResultFromModelErrors<T>()
        {
            return ApiResult<T>.Failure(GetModelErrors());
        }
    }
}
