using SaApp1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SaApp1.Controllers
{
    [Authorize]
    public class InfoController : Controller
    {
        private SaApp1DBEntities _db = new SaApp1DBEntities();

        // GET: Info
        public ActionResult Index(LoginUser loginUser)
        {
            return RedirectToAction("Index", "Home");
        }

        // GET: Info/Edit/5
        public ActionResult Edit(int id)
        {
            var realPerson = new RealPerson(id);

            return View(realPerson);
        }

        // POST: Info/Edit/5
        [HttpPost]
        public ActionResult Edit(RealPerson returnedRealPerson)
        {
            try
            {
                var originalPerson = new RealPerson(returnedRealPerson.ID);

                if (!ModelState.IsValid)
                    return View(returnedRealPerson);

                _db.People.Attach(originalPerson.Person);
                if (originalPerson.PersonInfo != null)
                    _db.PersonInfoes.Attach(originalPerson.PersonInfo);

                var currentPassword = originalPerson.Person.Password;
                _db.Entry(originalPerson.Person).CurrentValues.SetValues(returnedRealPerson.Person);
                if (currentPassword != originalPerson.Person.Password.MD5())
                {
                    originalPerson.Person.Password = currentPassword;
                }
                else if (returnedRealPerson.Person.Password.MD5() == currentPassword && returnedRealPerson.NewPassword == returnedRealPerson.ConfirmPassword && !string.IsNullOrEmpty(returnedRealPerson.NewPassword))
                {
                    originalPerson.Person.Password = returnedRealPerson.NewPassword.MD5();
                }

                if (originalPerson.PersonInfo != null)
                {
                    _db.Entry(originalPerson.PersonInfo).CurrentValues.SetValues(returnedRealPerson.PersonInfo);

                }
                else
                {
                    originalPerson.PersonInfo = _db.PersonInfoes.Add(returnedRealPerson.PersonInfo);
                    originalPerson.PersonInfo.PersonId = originalPerson.Person.Id;
                }
                _db.SaveChanges();

                return View(originalPerson);
            }
            // https://stackoverflow.com/questions/7795300/validation-failed-for-one-or-more-entities-see-entityvalidationerrors-propert
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(returnedRealPerson);
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}

// https://www.c-sharpcorner.com/UploadFile/0c1bb2/post-data-without-whole-postback/
