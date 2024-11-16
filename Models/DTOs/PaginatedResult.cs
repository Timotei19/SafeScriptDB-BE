using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class PagedResult<T>
    {
        public List<T> Records { get; set; }
        public int TotalRecords { get; set; }
    }

}
