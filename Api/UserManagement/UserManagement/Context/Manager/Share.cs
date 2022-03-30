using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserManagement.Context.Manager
{
    public class Share
    {
        public static T GetValue<T>(JObject obj, string fName)
        {
            if (obj != null)
            {
                var r = obj[fName];
                //if (r != null)
                //{ 
                //    if (r.Type.ToString() == typeof(T).Name)
                        return r.Value<T>(); 
                //}
            }
            return default(T);
        }
    }
}