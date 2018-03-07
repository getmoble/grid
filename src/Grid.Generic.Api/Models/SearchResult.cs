using System.Collections.Generic;

namespace Grid.Generic.Api.Models
{
        public class SearchResult<T>
        {
            public PagingInfo PagingInfo { get; set; }
            public IEnumerable<T> Items { get; set; }
        }
   
}
