using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            var model = new ContactDao().GetActiveContact();
            return View(model);
        }
        [HttpPost]
        public JsonResult Send(string name,string moblie,string address,string email,string content)
        {
            var feedBack = new Feedback();
            feedBack.Name = name;
            feedBack.Phone = moblie;
            feedBack.Address = address;
            feedBack.Email = email;
            feedBack.Content = content;
            feedBack.CreateDate = DateTime.Now;

            var id = new ContactDao().InsertFeedBack(feedBack);
            if (id>0)
            {
                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
        }
    }
}