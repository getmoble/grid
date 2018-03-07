using System.Web.Routing;

namespace Grid.Generic.Api.Models
{
    public class GridObject: RouteValueDictionary
    {
        public GridObject()
        {
            
        }

        public GridObject(object val): base(val)
        {
            
        }
    }
}
