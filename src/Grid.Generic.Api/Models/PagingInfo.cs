using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grid.Generic.Api.Models
{
    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }

        public int TotalPages
        {
            get
            {
                if (TotalItems != 0 && ItemsPerPage != 0)
                    return (int)Math.Ceiling((double)TotalItems / ItemsPerPage);
                else
                    return 0;
            }
        }

        public PagingInfo() { }

        public PagingInfo(int currentPage, int itemsPerPage, int totalItems)
        {
            CurrentPage = currentPage;
            ItemsPerPage = itemsPerPage;
            TotalItems = totalItems;
        }
    }
}
