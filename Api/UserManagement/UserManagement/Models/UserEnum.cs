using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace UserManagement.Models
{
    public class UserEnum
    {  
        public enum DELETE_STATUS
        {
            [Description("Khóa xóa")]
            LOCKDELETE  = 0,

            [Description("Đã xóa")]
            ISDELETE = 1,
        }
        public enum GENDEL
        {
            [Description("Nam")]
            MALE = 0,

            [Description("Nữ")]
            FEMALE = 1,

            [Description("Khác")]
            OTHERS = 2,
        }
 
    }
    public static class EnumAttributesHelper
    { 
        public static string GetDescription(this Enum value)
        {
            try
            {
                var da = (DescriptionAttribute[])(value.GetType().GetField(value.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), false);
                return da.Length > 0 ? da[0].Description : value.ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}