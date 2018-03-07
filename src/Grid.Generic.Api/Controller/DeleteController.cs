namespace Grid.Generic.Api.Controller
{
    using Data;
    using System.Web.Http;

    public class DeleteController : BaseWebApiController
    {
        private readonly GridDataContext _dataContext;

        public DeleteController(GridDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [Route("generic/delete/{type}/{id}")]
        [HttpPost]
        [WebApiExceptionFilter]
        public IHttpActionResult Delete(string type, long id)
        {
            return WrapIntoApiResult(() =>
            {
                var query = "delete from " + type + " where id=" + id;
                var data = _dataContext.Database.ExecuteSqlCommand(query);
                return data;
            });
        }
    }
}
