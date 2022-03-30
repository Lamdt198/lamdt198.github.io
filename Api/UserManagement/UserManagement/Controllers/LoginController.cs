using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using UserManagement.Context.Manager;
using UserManagement.Models;
using Newtonsoft.Json.Linq;
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
    public class LoginController : ApiController
    {
        /// <summary>Đăng nhập.</summary>
        /// <param name="modelData">The model data.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lamdt2 3/30/2022 created
        /// </Modified>
        [HttpPost] 
        public AjaxResult Login(JObject modelData)
        { 
            LoginManager LoginManager = new LoginManager();
            string Password = Share.GetValue<string>(modelData, "password");
            string UserName = Share.GetValue<string>(modelData, "username");
            var LoginInfo = LoginManager.Login(UserName, Password); 
            return new AjaxResult(LoginInfo.IsError, LoginInfo.Title, LoginInfo);
        }
     
    }
  
}
