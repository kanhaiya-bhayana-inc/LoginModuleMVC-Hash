using LoginModuleHashP.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scrypt;

namespace LoginModuleHashP.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        DbUserSignUpLoginEntities db = new DbUserSignUpLoginEntities();
        // GET: User
        public ActionResult Index()
        {
            return View(db.TblUserInfoups.ToList());
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(TblUserInfoup request)
        {
            if (db.TblUserInfoups.Any(x => x.UserName == request.UserName))
            {
                ViewBag.Notification = "This account has already existed";
                return View();
            }
            else
            {
                /*request.UserPassword = encoder.Encode(request.UserPassword);*/
                var pss = Helper.Encrypt(request.UserPassword);
                TblUserInfoup obj = new TblUserInfoup();
                /*request.UserPassword = pss;*/
                obj.UserName = request.UserName;
                obj.UserPassword = pss;
                obj.RePassword = pss;

                db.TblUserInfoups.Add(obj);
                db.SaveChanges();

                /*Session["UserId"] = request.UserID.ToString();
                Session["UserName"] = request.UserName.ToString();*/
                return RedirectToAction("Index");
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(TblUserInfoup requrest)
        {
            string pss = Helper.Encrypt(requrest.UserPassword);
            var checkLogin = db.TblUserInfoups.Where(x => x.UserName.Equals(requrest.UserName) && x.UserPassword.Equals(pss)).FirstOrDefault();
            if (checkLogin != null)
            {
                Session["UserId"] = requrest.UserID.ToString();
                Session["UserName"] = requrest.UserName.ToString();
                return RedirectToAction("Index", User);
            }
            else
            {
                ViewBag.Notification = "Invalid crecentials!";
            }
            return View();
        }

    }
}
