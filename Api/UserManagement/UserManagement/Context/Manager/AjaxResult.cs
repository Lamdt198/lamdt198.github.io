using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserManagement.Context.Manager
{
    public class AjaxResult
    {
        public bool IsError { get; set; }
        public string Title { get; set; }
        public object ObjData { get; set; }

        public AjaxResult() { }
        public AjaxResult(bool IsError, string Title, object ObjData = null)
        {
            this.IsError = IsError;
            this.Title = Title.Replace("\r", "").Replace("\r", "\\");
            this.ObjData = ObjData;
        }
    }
}