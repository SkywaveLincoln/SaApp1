using SaApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace SaApp1.Controllers
{
    public class HomeController : Controller
    {

        private SaApp1DBEntities _db = new SaApp1DBEntities();

        // GET: Home
        public ActionResult Index(LoginUser loginUser)
        {
            loginUser = new LoginUser();
            return View(loginUser);
        }

        // Login method
        public ActionResult LoginMethod(LoginUser loginUser)
        {
            var md5Password = loginUser.Password.MD5();
            var loggedInUser = _db.People.FirstOrDefault(m => (m.Name + " " + m.Surname) == loginUser.FullName && 
                md5Password == m.Password);
            if(loggedInUser != null && loginUser.FullName != "admin")
            {
                // update LastLogin with current date
                loggedInUser.LastLogin = DateTime.Now;
                _db.SaveChanges();
                return RedirectToAction("Edit", "Info", new { id = loggedInUser.Id });
            }

            if (loginUser.FullName == "admin" && loginUser.Password.MD5() == "21232F297A57A5A743894A0E4A801FC3")
            {
                ViewBag.Message = "Admin user logged in";
                return View("Index");
            }

            ViewBag.Message = "Incorrect username or password";
            return View("Index");
        }

    }
}

// https://stackoverflow.com/questions/21659771/how-to-make-a-submit-button-with-mvc-4/22032099
// https://www.c-sharpcorner.com/article/simple-login-application-using-Asp-Net-mvc/
// https://www.c-sharpcorner.com/UploadFile/ff2f08/multiple-models-in-single-view-in-mvc/
// https://www.youtube.com/watch?v=OgdpC35qHc0&t=688s
// https://www.mikesdotnetting.com/article/335/simple-authentication-in-razor-pages-without-a-database
