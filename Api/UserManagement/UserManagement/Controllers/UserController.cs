using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using UserManagement.Context.Manager;
using UserManagement.Models;

 
namespace UserManagement.Controllers
{
    /// <summary>
    ///   <br />
    /// </summary>
    /// <Modified>
    /// Name Date Comments
    /// lamdt2 3/30/2022 created
    /// </Modified>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        //CultureInfo provider = CultureInfo.InvariantCulture;

        /// <summary>Search danh sách người dùng sau phân trang.</summary>
        /// <param name="SearchText">The search text.</param>
        /// <param name="StartDate">The start date.</param>
        /// <param name="EndDate">The end date.</param>
        /// <param name="Gendel">The gendel.</param>
        /// <param name="currentPage">The current page.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lamdt2 3/30/2022 created
        /// </Modified>
        [HttpGet] 
        public AjaxResult GetListUser(string SearchText, string StartDate, string EndDate, string Gendel, int currentPage = 1)
        {
            
            try
            {
                // Kiểu dữ liệu datetime tìm kiếm thời gian
                if (!string.IsNullOrEmpty(StartDate)) StartDate = StartDate.ToLower().Replace(",", " ");
                if (!string.IsNullOrEmpty(EndDate)) EndDate = EndDate.ToLower().Replace(",", " ");
              
                DateTime? StartDateConvert = string.IsNullOrEmpty(StartDate) ? (DateTime?)null : DateTime.ParseExact(StartDate, "yyyy-MM-dd", null); 
                DateTime? EndDateConvert = string.IsNullOrEmpty(EndDate) ? (DateTime?)null : DateTime.ParseExact(EndDate, "yyyy-MM-dd", null);
                var aa = DateTime.Now;
                if (StartDateConvert > EndDateConvert)
                {
                    return new AjaxResult(true, "Ngày bắt đầu không được lớn hơn ngày kết thúc", new  ResponseAPIModel<UserModel>());
                }

                if (StartDateConvert.HasValue && !EndDateConvert.HasValue)
                {
                    EndDateConvert = DateTime.Now;
                }
               
                var GendelConvert = string.IsNullOrEmpty(Gendel) ? (int?)null : int.Parse(Gendel);

                // Lấy danh sách người dùng
                UserManager userManager = new UserManager();
                int pageSize = 6;
                var response = userManager.GetListUser(SearchText, StartDateConvert, EndDateConvert, GendelConvert, currentPage, pageSize);
              
                return new AjaxResult(false, "Ok", response);
            }
            catch (Exception ex)
            {
                return new AjaxResult(true, ex.Message, new List<UserModel>());
            } 
        }

        [HttpGet] 
        public AjaxResult GetUser(string ID)
        {
            try
            { 
                UserManager userManager = new UserManager();
                var User = userManager.GetUser(ID);
                return new AjaxResult(false, "Ok", User);
            }
            catch (Exception ex)
            {
                return new AjaxResult(true, ex.Message, new List<UserModel>());
            } 
        }
        
        [HttpPost]
        public AjaxResult CreateUser(JObject modelData) 
        { 
            try
            {
                
                UserManager userManager = new UserManager();
                var Msg = userManager.CreateUser(modelData);
                return new AjaxResult(Msg.IsError, Msg.Title);
            }
            catch (Exception ex)
            {
                return new AjaxResult(true, ex.Message, new List<UserModel>());
            }
        }

        [HttpPut]
        public AjaxResult UpdateUser(JObject modelData)
        {
            try
            {
                //LoginManager LoginManager = new LoginManager();
                //var mUserInfo = LoginManager.GetLoginedInfo();
                //if (mUserInfo == null)
                //    return new AjaxResult(true, "Hết phiên đăng nhập. Vui lòng đăng nhập lại.");
                UserManager userManager = new UserManager();
                var Msg = userManager.UpdateUser(modelData);
                return new AjaxResult(Msg.IsError, Msg.Title);
            }
            catch (Exception ex)
            {
                return new AjaxResult(true, ex.Message, new List<UserModel>());
            }
        }

        [HttpDelete]
        public AjaxResult DeleteUser(string ID)
        {
            try
            { 
                UserManager userManager = new UserManager();
                var Msg = userManager.DeleteUser(ID);
                return Msg;
            }
            catch (Exception ex)
            {
                return new AjaxResult(true, ex.Message, new List<UserModel>());
            }
        }
    }
}
