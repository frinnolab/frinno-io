using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace frinno_core.DTOs
{
    public class DataListResponse<T> where T : class
    {
        public int TotalItems { get; set; }
        public List<T> Data { get; set; }
    }
}