using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using PracaMGR.Models;

namespace PracaMGR.Controllers
{
    public class RaportyController : Controller
    {
        private JakTraficEntities db = new JakTraficEntities();

        // GET: Raporty
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {          
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult LinieRaport()
        {
            var linieRaport = new List<LinieRaport>(from a in db.LinieRaport orderby a.Wystapienia descending select a);
            var model = new LinieRaportViewModel();
            var labels = "";
            var data = "";
            var suma = (from a in linieRaport select a.Wystapienia).Sum();


            foreach(var a in linieRaport)
            {
                labels += "'Linia " + a.Linie.Symbol + "'" + ",";
                double w = Math.Round(((double)a.Wystapienia/(double)suma)*100.0, 2);
                data += w.ToString().Replace(",",".") + ",";
            }
            model.labels = labels;
            model.data = data;
            model.labels.Substring(0, model.labels.Length - 1);
            model.data.Substring(0, model.data.Length - 1);

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> TrasyRaport()   
        {
            var trasyRaport = db.TrasyRaport;
            return View(await trasyRaport.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult PrzystankiRaport()
        {
            var przystankiRaport = new List<PrzystankiRaport>(from a in db.PrzystankiRaport orderby a.Wystapienia descending select a);
            var model = new LinieRaportViewModel();
            var data = "";
            var suma = (from a in przystankiRaport select a.Wystapienia).Sum();

            foreach (var a in przystankiRaport)
            {                
                double w = Math.Round(((double)a.Wystapienia / (double)suma) * 1000000.0, 0);
                data += "{ center: new google.maps.LatLng(" + a.Przystanki.X.ToString().Replace(",", ".") + "," + a.Przystanki.Y.ToString().Replace(",",".") + "), Ilosc: " + w + "},";
            }
            model.data = data;
            model.data.Substring(0, model.data.Length - 1);

            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
