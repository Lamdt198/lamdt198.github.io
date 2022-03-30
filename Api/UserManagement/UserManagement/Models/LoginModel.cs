using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserManagement.Models
{
    public class LoginModel
    {
        public string TOKEN { get; set; }
        public string ID { get; set; }
        public string UserName { get; set; } 
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? IsDelete { get; set; }
        public bool IsError { get; set; }
        public string Title { get; set; }
    }
}