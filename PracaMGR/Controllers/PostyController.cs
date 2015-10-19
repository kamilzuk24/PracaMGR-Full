using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PracaMGR;
using PracaMGR.Models;

namespace Praca.Controllers
{
    public class PostyController : Controller
    {
        JakTraficEntities data = new JakTraficEntities();

        // GET: Posty      
        public ActionResult Index(int id, int page = 1)
        {
            TempData["kategoria"] = id;
           
            var list = new List<Posty>(from a in data.Posty
                                       where a.Id_kategoria.Equals(id) 
                                       orderby a.Data descending
                                       select a);

            IPagedList<Posty> li = list.ToPagedList(page,10);
            return View(li);
        }

        public ActionResult Odpowiedzi(int id,int page = 1)
        {
            var a = new Post { post = data.Posty.Single(i => i.Id == id) };
            var li = data.Odpowiedzi.Where(item => item.Id_Post.Equals(id)).ToList();
            li.Reverse();
            a.odpowiedz = li.ToPagedList(page, 10); 
            return View(a);
        }

        [HttpPost]
        [Authorize(Roles = "SimpleUser,Admin")]
        public ActionResult Dodaj(String tresc, int post)
        {
            var a = new Odpowiedzi { Id_Post = post, Data = DateTime.Now, Tresc = tresc, Uzytkownik = User.Identity.Name };
            data.Odpowiedzi.Add(a);
            data.SaveChanges();

            return RedirectToAction("Odpowiedzi/" + post);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SimpleUser,Admin")]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                String g = TempData["kategoria"].ToString();
                var a = new Posty {Id_kategoria = int.Parse(g), Data = DateTime.Now, Uzytkownik = User.Identity.Name};
                UpdateModel(a);
                data.Posty.Add(a);
                data.SaveChanges();

                return RedirectToAction("Index/"+int.Parse(g));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            return View(data.Posty.FirstOrDefault(x => x.Id == id));
        }

        [HttpPost]
        [Authorize(Roles = "SimpleUser,Admin")]
        public ActionResult Edit(FormCollection collection)
        {
            try
            {
                int id = int.Parse(collection["Id"].ToString());
                var a = data.Posty.FirstOrDefault(x => x.Id == id);
                UpdateModel(a);              
                data.SaveChanges();

                return RedirectToAction("Index/" + a.Id_kategoria);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            Posty a = data.Posty.Single(i => i.Id == id);
            return View(a);
        }

        [HttpPost]
        [Authorize(Roles = "SimpleUser,Admin")]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {             
                String g = TempData["kategoria"].ToString();
                Posty a = data.Posty.Single(i => i.Id == id);
                if (User.Identity.Name == a.Uzytkownik || User.IsInRole("Admin"))
                {
                    data.Odpowiedzi.RemoveRange(from b in data.Odpowiedzi where b.Id_Post == id select b);
                    data.Posty.Remove(a);
                    data.SaveChanges();

                    return RedirectToAction("Index/" + int.Parse(g));
                }
                else
                {
                    return new HttpUnauthorizedResult();
                }
            }
            catch
            {
                String g = TempData["kategoria"].ToString();
                return RedirectToAction("Index/" + int.Parse(g));
            }
        }

        public ActionResult EditAnswer(int id)
        {
            Odpowiedzi a = data.Odpowiedzi.Single(i => i.Id == id);
            return View(a);
        }

        [HttpPost]
        public ActionResult EditAnswer(FormCollection collection)
        {
            try
            {
                int id = int.Parse(collection["Id"].ToString());
                var a = data.Odpowiedzi.FirstOrDefault(x => x.Id == id);
                UpdateModel(a);
                data.SaveChanges();

                return RedirectToAction("Odpowiedzi/" + a.Id_Post);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult DeleteAnswer(int id)
        {
             var odpowiedz = data.Odpowiedzi.SingleOrDefault(x => x.Id == id);
             
             if (odpowiedz != null)
             {
                 int id_post = odpowiedz.Id_Post;
                 if (User.Identity.Name == odpowiedz.Uzytkownik || User.IsInRole("Admin"))
                 {
                     data.Odpowiedzi.Remove(odpowiedz);
                     data.SaveChanges();

                     return RedirectToAction("Odpowiedzi", new { id = id_post });
                 }
                 else
                 {
                     return new HttpUnauthorizedResult();
                 }
             }
             else
             {
                 return View("Error");
             }
         }
    }
}
