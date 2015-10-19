using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using PracaMGR.ViewModels;

namespace PracaMGR.Controllers
{
    public class ForumController : Controller
    {
        JakTraficEntities data = new JakTraficEntities();

        // GET: Forum
        public ActionResult Index(int page = 1)
        {
            var list = new List<Kategorie>(data.Kategorie);
            IPagedList<Kategorie> li = list.ToPagedList(page, 10);

            return View(li);
        }

        public ActionResult Linie(int page = 1)
        {
            var list = new List<LiniaViewModel>();
            
            foreach (var a in data.Linie)
            {
                double suma = 0;
                int licz = 0;
                foreach (var b in data.Oceny)
                {
                    if (a.Symbol.Equals(b.Symbol_linii)) { suma = suma + b.Suma; licz++; }
                }
                var c = new LiniaViewModel { Record = a, Ocena = licz > 0 ? Math.Round(suma / (double)licz, 2) : 0 };

                list.Add(c);
            }
            IPagedList<LiniaViewModel> li = list.ToPagedList(page, 10);      

            return View(li);
        }

        [Authorize]
        public ActionResult Ocena(String id)
        {
            var a = new Oceny { Symbol_linii = id };
            return View(a);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Ocena(String id, FormCollection collection)
        {
            try
            {
                var a = new Oceny();               
                UpdateModel(a);

                a.Symbol_linii = id;
                a.Uzytkownik = User.Identity.Name;
                var ss = Math.Round((double)(a.Punktualnosc+a.Poczucie_bezpieczenstwa+a.Kultura_os_kierowcy+a.Komfort_jazdy+a.Czystosc)/5,2);
                a.Suma = ss; 
           
                data.Oceny.Add(a);
                data.SaveChanges();

                return RedirectToAction("Linie");
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        // GET: Forum/Create
        public ActionResult Create()
        {            
                return View();           
        }

        // POST: Forum/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var a = new Kategorie();
                UpdateModel(a);
                data.Kategorie.Add(a);
                data.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Forum/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            Kategorie a = data.Kategorie.Single(i => i.Id == id);
            return View(a);
        }

        // POST: Forum/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                var a = data.Kategorie.Single(i => i.Id == id);
                UpdateModel(a);
                data.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            Kategorie a = data.Kategorie.Single(i => i.Id == id);
            return View(a);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Kategorie a = data.Kategorie.Single(i => i.Id == id);  
                
                data.Odpowiedzi.RemoveRange((from f in data.Odpowiedzi
                                                   from d in data.Posty
                                                   where d.Id_kategoria == id && f.Id_Post == d.Id
                                                   select f).Distinct());
                data.Posty.RemoveRange(from f in data.Posty
                                             where f.Id_kategoria == id
                                             select f);

                data.Kategorie.Remove(a);
                data.SaveChanges();
                

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
