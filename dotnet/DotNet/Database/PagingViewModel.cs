using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class PagingViewModel
    {
        public int Start { get; set; }
        public int Count { get; set; }
        public string? OrderBy { get; set; }
        public bool IsAscending { get; set; }
    }
}
