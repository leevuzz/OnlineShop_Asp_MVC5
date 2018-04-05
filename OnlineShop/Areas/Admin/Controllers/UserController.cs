    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.Dao;
using OnlineShop.Common;
using PagedList;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        // GET: Admin/User
        [HasCredential(RoleID ="VIEW_USER")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new UserDao();
            var model = dao.LissAllPaging(searchString, page, pageSize);
            ViewBag.searchString = searchString;
            return View(model);
        }
        // Create
        [HttpGet]
        [HasCredential(RoleID = "ADD_USER")]

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [HasCredential(RoleID = "ADD_USER")]

        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var encrytPass = Encryptor.MD5Hash(user.Password);
                user.Password = encrytPass;
                long id = dao.Insert(user);
                if (id > 0)
                {
                    SetAlert("Thêm User thành công !", "success");
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm user không thành công !");
                }
            }
            return View("Index");

        }
        // Update
        [HasCredential(RoleID = "EDIT_USER")]

        public ActionResult Edit(int id)
        {
            var user = new UserDao().ViewDetail(id);
            return View(user);
        }
        [HttpPost]
        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {

                var dao = new UserDao();
                if (!string.IsNullOrEmpty(user.Password))
                {
                    var encrytPass = Encryptor.MD5Hash(user.Password);
                    user.Password = encrytPass;
                }

                var result = dao.Update(user);
                if (result)
                {
                    SetAlert("Sửa User thành công !", "success");

                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật user không thành công !");
                }
            }
            return View("Index");

        }
        // Delete
        [HttpDelete]
        [HasCredential(RoleID = "DELETE_USER")]
        public ActionResult Delete(int id)
        {
            new UserDao().Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [HasCredential(RoleID ="EDIT_USER")]
        public JsonResult ChangeStatus(long id)
        {
            var result = new UserDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}