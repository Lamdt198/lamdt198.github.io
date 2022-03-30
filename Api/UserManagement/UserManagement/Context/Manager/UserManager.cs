using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UserManagement.Models;
using Newtonsoft.Json.Linq;
using System.Web.Configuration;
using Newtonsoft.Json;
using System.ComponentModel;
using UserEnum = UserManagement.Models.UserEnum;

namespace UserManagement.Context.Manager
{

    /// <summary></summary>
    /// <Modified>
    /// Name Date Comments
    /// lamdt2 3/30/2022 created
    /// </Modified>
    public class UserManager
    {  
        #region Định dạng dữ liệu
        /// <summary>
        /// Định dạng thời gian
        /// </summary>
        /// <param name="DateTime"></param>
        /// <returns></returns>
        public string DinhDangDateTimeddMMyyyy(DateTime? DateTime)
        {
            try
            {
                if (DateTime == null)
                { return ""; }
                DateTime DateTimeConvert = Convert.ToDateTime(DateTime);
                return DateTimeConvert.ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// Định dạng loại giới tính
        /// </summary>
        /// <param name="Gendel"></param>
        /// <returns></returns>
        public string GendelName(decimal? Gendel)
        {
            try
            {
                var GendelName = ""; 
                if (Gendel == null)
                    GendelName = "";
                else if (Gendel == UserEnum.GENDEL.MALE.GetHashCode())
                    GendelName = EnumAttributesHelper.GetDescription(UserEnum.GENDEL.MALE);
                else if (Gendel == UserEnum.GENDEL.FEMALE.GetHashCode())
                    GendelName = EnumAttributesHelper.GetDescription(UserEnum.GENDEL.FEMALE);
                else if (Gendel == UserEnum.GENDEL.OTHERS.GetHashCode())
                    GendelName = EnumAttributesHelper.GetDescription(UserEnum.GENDEL.OTHERS); 
                return GendelName;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        /// <summary>Danh sách người dùng sau phân trang</summary>
        /// <param name="SearchText"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="Gendel"></param>
        /// <param name="CurrentPage"></param>
        /// <param name="PageSize"></param>
        /// <returns>
        ///   <br />
        /// </returns>
        public ResponseAPIModel<UserModel> GetListUser(string SearchText, DateTime? StartDate, DateTime? EndDate, int? Gendel, int CurrentPage = 1, int PageSize = 10)
        {
            ResponseAPIModel<UserModel> conf = new ResponseAPIModel<UserModel>();
            int IsDelete = UserEnum.DELETE_STATUS.ISDELETE.GetHashCode();
            int ItemSkip = (CurrentPage * PageSize) - PageSize;
            int TotalRecord = 0;
            var ListUser = new List<UserModel>();
            using (var context = new UserManagementEntities())
            {
                var ListTemp = (from ObjUser in context.Users
                                where ObjUser.IsDelete != IsDelete && (Gendel == null || ObjUser.Gendel == Gendel) && (string.IsNullOrEmpty(SearchText) || ObjUser.UserName.ToLower().Trim().Contains(SearchText) || ObjUser.Email.ToLower().Trim().Contains(SearchText))
                                  && ((!StartDate.HasValue && !EndDate.HasValue) || (StartDate.HasValue && EndDate.HasValue && StartDate <= ObjUser.BirthDate && ObjUser.BirthDate <= EndDate))
                                select new UserModel
                                {
                                    ID = ObjUser.ID,
                                    UserName = ObjUser.UserName,
                                    PhoneNumber = ObjUser.PhoneNumber,
                                    Email = ObjUser.Email,
                                    IsDelete = ObjUser.IsDelete,
                                    BirthDate = ObjUser.BirthDate,
                                    Gendel = ObjUser.Gendel
                                }).ToList();
                TotalRecord = ListTemp.Count();
                ListUser = ListTemp.OrderBy(x => x.UserName).Skip(ItemSkip).Take(PageSize).ToList();

                // Phân trang
                if (TotalRecord >= 0)
                {
                    conf.Message = "Thành công";
                    conf.RowNumber = CurrentPage == 1 ? 0 : ((CurrentPage * PageSize) - PageSize); 
                    conf.TotalRecord = TotalRecord;
                    var Pages = new List<int>();
                    var TotalPage = TotalRecord > 0 ? ((TotalRecord % PageSize == 0) ? TotalRecord / PageSize : ((TotalRecord - (TotalRecord % PageSize)) / PageSize) + 1) : 0;
                    for (int i = 1; i <= TotalPage; i++)
                    {
                        Pages.Add(i);
                    }
                    conf.TotalPage = Pages;
                    
                    // Định dạng lại dữ liệu người dùng
                    foreach (var item in ListUser)
                    {
                        item.BirthDateConvert = DinhDangDateTimeddMMyyyy(item.BirthDate);
                        item.GendelName = GendelName(item.Gendel);
                    } 
                    conf.LstData = ListUser;
                    return conf;
                }
            }
            return conf;
        }

        /// <summary>Thêm mới người dùng</summary>
        /// <param name="modelData"></param>
        /// <returns>
        ///   <br />
        /// </returns>
        public AjaxResult CreateUser(JObject modelData)
        {
            string Password = Share.GetValue<string>(modelData, "Password");
            string UserName = Share.GetValue<string>(modelData, "UserName");
            string BirthDate = Share.GetValue<string>(modelData, "BirthDate");
            string PhoneNumber = Share.GetValue<string>(modelData, "PhoneNumber");
            string Email = Share.GetValue<string>(modelData, "Email");
            string Gendel = Share.GetValue<string>(modelData, "Gendel");
            AjaxResult Msg = new AjaxResult();
            try
            {
                User User = new User();

                using (var context = new UserManagementEntities())
                {
                    // Check trùng thông tin đăng nhập(Email, số điện thoại)
                    var CheckExitEmail = context.Users.Where(s => s.Email == Email && s.IsDelete != 1).Count();
                    var CheckExitPhoneNumber = context.Users.Where(s => s.PhoneNumber == PhoneNumber && s.IsDelete != 1).Count();
                    if (CheckExitEmail > 0)
                    {
                        Msg.IsError = true;
                        Msg.Title = "Thêm mới người dùng " + UserName + " không thành công." + "Email đã tồn tại!";
                        return Msg;
                    }
                    if (CheckExitPhoneNumber > 0)
                    {
                        Msg.IsError = true;
                        Msg.Title = "Thêm mới người dùng " + UserName + " không thành công." + "Số điện thoại đã tồn tại!";
                        return Msg;
                    }

                    // Lưu thông tin người dùng
                    User.ID = Guid.NewGuid().ToString();
                    User.PhoneNumber = PhoneNumber;
                    User.Email = Email;
                    User.Gendel = int.Parse(Gendel);
                    User.UserName = UserName;
                    User.CreatedDate = DateTime.Now;
                    User.Password = PasswordHasher.HashString(Password);
                    User.BirthDate = string.IsNullOrEmpty(BirthDate) ? (DateTime?)null : DateTime.ParseExact(BirthDate, "yyyy-MM-dd", null);
                    context.Users.Add(User);
                    context.SaveChanges();
                    Msg.IsError = false;
                    Msg.Title = "Thêm mới người dùng " + UserName + " thành công";
                }
            }
            catch (Exception ex)
            {
                Msg.IsError = true;
                Msg.Title = "Thêm mới người dùng " + UserName + " không thành công" + ex;
            }
            return Msg;
        }

        /// <summary>Sửa thông tin người dùng</summary>
        /// <param name="modelData"></param>
        /// <returns>
        ///   <br />
        /// </returns>
        public AjaxResult UpdateUser(JObject modelData)
        {
            string ID = Share.GetValue<string>(modelData, "ID");
            string UserName = Share.GetValue<string>(modelData, "UserName");
            string BirthDateConvert = Share.GetValue<string>(modelData, "BirthDateConvert");
            string PhoneNumber = Share.GetValue<string>(modelData, "PhoneNumber");
            string Email = Share.GetValue<string>(modelData, "Email");
            int Gendel = Share.GetValue<int>(modelData, "Gendel");
            AjaxResult Msg = new AjaxResult();
            try
            {


                using (var context = new UserManagementEntities())
                {
                    // Check trùng thông tin đăng nhập(Email, số điện thoại)
                    var CheckExitEmail = context.Users.Where(s => s.Email == Email && s.ID != ID && s.IsDelete != 1).Count();
                    var CheckExitPhoneNumber = context.Users.Where(s => s.PhoneNumber == PhoneNumber && s.ID != ID && s.IsDelete != 1).Count();
                    if (CheckExitEmail > 0)
                    {
                        Msg.IsError = true;
                        Msg.Title = "Cập nhật người dùng " + UserName + " không thành công." + "Email đã tồn tại!";
                        return Msg;
                    }
                    if (CheckExitPhoneNumber > 0)
                    {
                        Msg.IsError = true;
                        Msg.Title = "Cập nhật người dùng " + UserName + " không thành công." + "Số điện thoại đã tồn tại!";
                        return Msg;
                    }
                  
                    // Lưu thông tin người dùng
                    var User = context.Users.FirstOrDefault(s => s.ID == ID);
                    if (User.IsDelete == UserEnum.DELETE_STATUS.LOCKDELETE.GetHashCode())
                    {
                        Msg.IsError = true;
                        Msg.Title = "Người dùng mặc định không sửa!";
                        return Msg;
                    }
                    User.PhoneNumber = PhoneNumber;
                    User.Email = Email;
                    User.Gendel = Gendel;
                    User.UserName = UserName;
                    User.UpdatedDate = DateTime.Now;
                    User.BirthDate = string.IsNullOrEmpty(BirthDateConvert) ? (DateTime?)null : DateTime.ParseExact(BirthDateConvert, "yyyy-MM-dd", null);
                    context.SaveChanges();
                    Msg.IsError = false;
                    Msg.Title = "Cập nhật người dùng " + UserName + " thành công";
                }
            }
            catch (Exception ex)
            {
                Msg.IsError = true;
                Msg.Title = "Cập nhật người dùng " + UserName + " không thành công" + ex;
            }
            return Msg;
        }

        /// <summary>Tạo trạng thái đã xóa cho người dùng</summary>
        /// <param name="ID"></param>
        /// <returns>
        ///   <br />
        /// </returns>
        public AjaxResult DeleteUser(string ID)
        {
            AjaxResult Msg = new AjaxResult();
            using (var context = new UserManagementEntities())
            {
                try
                {
                    var User = context.Users.Where(s => s.ID == ID).FirstOrDefault();
                    if (User == null)
                    {
                        Msg.IsError = true;
                        Msg.Title = "Xóa người dùng không thành công-Người dùng không tôn tại!";
                        return Msg;
                    }
                    if (User.IsDelete == UserEnum.DELETE_STATUS.LOCKDELETE.GetHashCode())
                    {
                        Msg.IsError = true;
                        Msg.Title = "Người dùng mặc định không được xóa!";
                        return Msg;
                    }
                    User.IsDelete =  UserEnum.DELETE_STATUS.ISDELETE.GetHashCode(); 
                    context.SaveChanges();
                    Msg.IsError = false;
                    Msg.Title = "Xóa người dùng thành công!";
                }
                catch (Exception ex)
                {
                    Msg.IsError = true;
                    Msg.Title = "Xóa người dùng không thành công!" + ex;
                }
            }
            return Msg;
        }

        /// <summary>Thông tin người dùng theo ID</summary>
        /// <param name="ID"></param>
        /// <returns>
        ///   <br />
        /// </returns>
        public UserModel GetUser(string ID)
        {
            var User = new UserModel();
            using (var context = new UserManagementEntities())
            {
                User = (from ObjUser in context.Users
                        where ObjUser.ID == ID
                        select new UserModel
                        {
                            ID = ObjUser.ID,
                            UserName = ObjUser.UserName,
                            PhoneNumber = ObjUser.PhoneNumber, 
                            Email = ObjUser.Email,
                            BirthDate = ObjUser.BirthDate,
                            Gendel = ObjUser.Gendel
                        }).FirstOrDefault();
                User.BirthDateConvert = DinhDangDateTimeddMMyyyy(User.BirthDate);
            }
            return User;
        }
    }
}