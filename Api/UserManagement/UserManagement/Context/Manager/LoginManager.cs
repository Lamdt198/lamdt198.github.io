using System;
using System.Collections.Generic; 
using System.Linq; 
using UserManagement.Models; 
using System.Web.Http;
using Microsoft.IdentityModel.Tokens; 
using Newtonsoft.Json; 
using System.IdentityModel.Tokens.Jwt; 
using System.Security.Claims;
using System.Text; 
using System.Web.Configuration;
namespace UserManagement.Context.Manager
{
    /// <summary>
    ///   <br />
    /// </summary>
    /// <Modified>
    /// Name Date Comments
    /// lamdt2 3/30/2022 created
    /// </Modified>
    public class LoginManager : System.Web.Http.ApiController
    {

        /// <summary>Đăng nhập</summary>
        /// <param name="Username">The username.</param>
        /// <param name="Password">The password.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lamdt2 3/30/2022 created
        /// </Modified>
        public LoginModel Login(string Username, string Password)
        {
            LoginModel loginModel = new LoginModel();
            try
            {
                if (Username.Trim() != "" && Password.Trim() != "")
                {
                    using (var context = new UserManagementEntities())
                    {
                        //Check thông tin đăng nhập
                        var userInfo = context.Users.Where(ag => (ag.PhoneNumber.ToLower() == Username.ToLower() || ag.Email.ToLower() == Username.ToLower()) && ag.IsDelete != 1).FirstOrDefault();
                        if (userInfo != null)
                        {
                            if (PasswordHasher.VerifyHashedString(userInfo.Password, Password))
                            {
                                // Tạo token
                                loginModel.TOKEN = GetJsonWebToken(userInfo);
                                loginModel.PhoneNumber = userInfo.PhoneNumber;
                                loginModel.Email = userInfo.Email;
                                loginModel.Password = userInfo.Password;
                                loginModel.ID = userInfo.ID;
                                loginModel.IsDelete = userInfo.IsDelete;
                                loginModel.IsError = false;
                                loginModel.Title = "Đăng nhập thành công";
                            }
                            else
                            {
                                loginModel.IsError = true;
                                loginModel.Title = "Mật khẩu không đúng!";
                            }
                        }
                        else
                        {
                            loginModel.IsError = true;
                            loginModel.Title = "Tài khoản không tồn tại!";
                        }
                    }
                }
                else
                {
                    loginModel.IsError = true;
                    loginModel.Title = "Đã có lỗi xảy ra!";
                } 
            }
            catch (Exception ex)
            {
                loginModel.IsError = true;
                loginModel.Title = "Đã có lỗi xảy ra!" + ex; 
            }
            return loginModel;
        }


        /// <summary>Tạo json web token</summary>
        /// <param name="UserInfo">The user information.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// lamdt2 3/30/2022 created
        /// </Modified>
        public string GetJsonWebToken(User UserInfo)
        {
            string jwtSecretKey = WebConfigurationManager.AppSettings["jwtSecretKey"];
            string jwtIssuer = WebConfigurationManager.AppSettings["jwtIssuer"];
            string jwtExpires = WebConfigurationManager.AppSettings["jwtExpires"];

            string key = jwtSecretKey;
            var issuer = jwtIssuer;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Create a List of Claims, Keep claims name short    
            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            permClaims.Add(new Claim(ClaimTypes.Name, JsonConvert.SerializeObject(new
            {
                ID = UserInfo.ID,
                UserName = UserInfo.Email,
                PhoneNumber = UserInfo.PhoneNumber,
                DM_DONVI_ID = UserInfo.IsDelete,
                DM_PHONGBAN_ID = UserInfo.UserName
            })));


            //Create Security Token object by giving required parameters    
            var token = new JwtSecurityToken(issuer, //Issure    
                            issuer,  //Audience    
                            permClaims,
                            expires: DateTime.Now.AddDays(Convert.ToInt32(jwtExpires)),
                            signingCredentials: credentials);
            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt_token;
        }

        //public LoginModel mUserInfo { get; set; } 
        //public LoginModel GetLoginedInfo()
        //{
        //    //if (!this.ControllerContext.Request.RequestUri.ToString().ToLower().EndsWith("/Api/Login/Login")) 
        //    //{
        //        var claimsIdentity = this.User.Identity as ClaimsIdentity;
        //        if (claimsIdentity.IsAuthenticated)
        //        {
        //            if (mUserInfo == null)
        //                mUserInfo = new LoginModel();
        //            var LoginValue = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
        //            mUserInfo = JsonConvert.DeserializeObject<LoginModel>(LoginValue);
        //            return mUserInfo;
        //        }
        //        else
        //        {

        //        }
        //    //}
        //    return null;
        //}
    }
}