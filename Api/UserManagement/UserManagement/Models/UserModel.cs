using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserManagement.Models
{
    public class UserModel
    {
        public string ID { get; set; } 
        public string UserName { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string BirthDateConvert { get; set; }
        public string GendelName { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Nullable<decimal> IsDelete { get; set; }
        public Nullable<decimal> Gendel { get; set; }
    }
}