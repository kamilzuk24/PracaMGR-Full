using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PracaMGR.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PracaMGR.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserManagerController : Controller
    {
        private JakTraficEntities db = new JakTraficEntities();

        // GET: UserManager
        public ActionResult Index()
        {            
            return View();
        }

        public ActionResult Days()
        {
            var days = db.DniSwiateczne.ToList();
            return View(days);
        }

        public ActionResult AddDay()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddDay(FormCollection collection)
        {
            DniSwiateczne a = new DniSwiateczne();
            a.data = DateTime.ParseExact(collection["data"].ToString(),"dd-MM-yyyy", CultureInfo.InvariantCulture);
            a.opis = collection["opis"].ToString();

            db.DniSwiateczne.Add(a);
            db.SaveChanges();

            return RedirectToAction("Days");
        }

        public ActionResult EditDay(int id)
        {
            var days = db.DniSwiateczne.FirstOrDefault(x => x.Id == id);           
            return View(days);
        }
        [HttpPost]
        public ActionResult EditDay(FormCollection collection)
        {
            int id = int.Parse(collection["Id"].ToString());
            var a = db.DniSwiateczne.FirstOrDefault(x => x.Id == id);
            a.data = DateTime.ParseExact(collection["data"].ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture);
            a.opis = collection["opis"].ToString();
            db.SaveChanges();

            return RedirectToAction("Days");
        }

        public ActionResult DeleteDay(int id)
        {
            var days = db.DniSwiateczne.FirstOrDefault(x => x.Id == id);
            db.DniSwiateczne.Remove(days);
            db.SaveChanges();

            return RedirectToAction("Days");
        }



        public ActionResult SimpleUsers()
        {
            using (var context = new ApplicationDbContext())
            {
                var allUsers = context.Users
                                    .Where(u => u.Roles.Any(r => r.Role.Name == "SimpleUser")).OrderBy(x => x.UserName)
                                    .ToList();

                return View(allUsers);
            }            
        }

        public ActionResult Admins()
        {
            using (var context = new ApplicationDbContext())
            {
                var allUsers = context.Users
                                    .Where(u => u.Roles.Any(r => r.Role.Name == "Admin")).OrderBy(x => x.UserName)
                                    .ToList();

                return View(allUsers);
            }
        }

        public ActionResult Edit(string id)
        {
            var context = new ApplicationDbContext();
            var model = context.Users.FirstOrDefault(x => x.Id == id);

            if (model != null)
            {
                var statusy = new List<SelectListItem>();
                if (model.Status == "Deactivated")
                {
                    statusy.Add(new SelectListItem() { Text = "Deactivated", Value = "Deactivated", Selected = true });
                    statusy.Add(new SelectListItem() { Text = "Activated", Value = "Activated" });
                }
                else
                {
                    statusy.Add(new SelectListItem() { Text = "Deactivated", Value = "Deactivated" });
                    statusy.Add(new SelectListItem() { Text = "Activated", Value = "Activated", Selected = true });
                }

                ViewBag.statusy = statusy;

                return View(model);
            }
            else
            {
                ViewBag.Error = "Nie ma takiego użytkownika.";
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult Edit(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var context = new ApplicationDbContext();
                    var user = context.Users.FirstOrDefault(x => x.Id == model.Id);
                    if (user.UserName != model.UserName)
                    {
                        if (context.Users.FirstOrDefault(x => x.UserName == model.UserName) == null)
                        {
                            user.UserName = model.UserName;
                        }
                        else
                        {
                            ModelState.AddModelError("", "Wybrany login istnieje w bazie.");
                            return View(model);
                        }
                    }

                    user.Name = model.Name;
                    user.LastName = model.LastName;
                    user.Status = model.Status;
                    context.SaveChanges();

                    if (user.Roles.Any(r => r.Role.Name == "SimpleUser"))
                    {
                        return RedirectToAction("SimpleUsers");
                    }
                    else
                    {
                        return RedirectToAction("Admins");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Wystąpił błąd spróbuj ponownie. " + ex.Message.ToString());
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult Delete(string id)
        {
            if(User.Identity.GetUserId() == id)
            {
                ViewBag.Error = "Nie możesz usunąć własnego konta";
                return View("Error");
            }

            var context = new ApplicationDbContext();
            var user = context.Users.FirstOrDefault(x => x.Id == id);
            var roles = user.Roles.Any(r => r.Role.Name == "SimpleUser");

            if (user != null)
            {
                context.Users.Remove(user);
                context.SaveChanges();
            }
            else
            {
                ViewBag.Error = "Nie ma takiego użytkownika.";
                return View("Error");
            }

            if (roles)
            {
                return RedirectToAction("SimpleUsers");
            }
            else
            {
                return RedirectToAction("Admins");
            }
        }

        public ActionResult AddAdminAccount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddAdminAccount(AdminRegisterModel model)
        {
            var context = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = new ApplicationUser() { UserName = model.UserName, Name = model.Name, LastName = model.LastName, Status = "Activated" };

            var result = userManager.Create(user, model.Password);
            if (result.Succeeded)
            {
                userManager.AddToRole(user.Id, "Admin");
            }   
            else
            {
                ViewBag.Error = "Błąd podczas dodawania użytkownika.";
                return View("Error");
            }        

            return RedirectToAction("Admins");
        }

    }
}