using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserManagement.Models
{
    public class MessageModel
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public bool IsError { get; set; } 
        public object ObjData { get; set; } 
      
        public MessageModel()
        {
            Title = string.Empty;
            IsError = false;
            ObjData = null;  
        }
    }
}