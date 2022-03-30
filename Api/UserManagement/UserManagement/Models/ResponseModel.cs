using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserManagement.Models
{
    public class ResponseAPIModel<T>
    {  
        public int TotalRecord { get; set; }
        public List<int> TotalPage { get; set; } 
        public int RowNumber { get; set; }
        public string Message { get; set; }
        public List<T> LstData { get; set; }
    }
}