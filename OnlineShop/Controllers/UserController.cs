using Model.Dao;
using Model.EF;
using OnlineShop.Common;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook;
using System.Configuration;

namespace OnlineShop.Controllers
{
    public class UserController : Controller
    {
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FaceBookCallbacl");
                return uriBuilder.Uri;
            }
        }
        // GET: User
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult LoginFacebook()
        {
            var faceBook = new FacebookClient();
            var loginUrl = faceBook.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
            });
            return Redirect(loginUrl.AbsoluteUri);
        }
        public ActionResult FaceBookCallbacl(string code)
        {
            var faceBook = new FacebookClient();
            dynamic result = faceBook.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });
            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                faceBook.AccessToken = accessToken;
                dynamic me = faceBook.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string userName = me.email;
                string firstname = me.first_name;
                string middlename = me.middle_name;
                string lastname = me.last_name;
                var user = new User();
                user.Email = email;
                user.UserName = userName;
                user.Name = firstname + " " + middlename + " " + lastname;
                user.CreateDate = DateTime.Now;
                user.Status = true;
                var resultInsert = new UserDao().InsertForFacebook(user);
                if (resultInsert > 0)
                {
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.ID;

                    Session.Add(CommonConstants.USER_SESSION, userSession);
                   
                }
            }
            return Redirect("/");
        }
        public ActionResult Logout()
        {
            Session[CommonConstants.USER_SESSION] = null;
            return Redirect("/");
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Login(model.UserName, Encryptor.MD5Hash(model.Password));
                if (result == 1)
                {
                    var user = dao.GetById(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.ID;

                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    return Redirect("/");

                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại.");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đang bị khóa.");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng.");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập không đúng.");
                }
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Register(RegisterModel register)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if (dao.CheckUserName(register.UserName))
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại !");
                }
                else if (dao.CheckEmail(register.Email))
                {
                    ModelState.AddModelError("", "Email đã tồn tại !");
                }
                else
                {
                    var user = new User();
                    user.UserName = register.UserName;
                    user.Name = register.Name;
                    user.Password = Encryptor.MD5Hash(register.Password);
                    user.Phone = register.Phone;
                    user.Address = register.Address;
                    user.Email = register.Email;
                    user.CreateDate = DateTime.Now;
                    user.Status = true;
                    var result = dao.Insert(user);
                    if (result > 0)
                    {
                        ViewBag.Success = "Đăng ký thành công !";
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng ký không thành công !");
                    }
                }
            }
            return View(register);
        }

    }
}