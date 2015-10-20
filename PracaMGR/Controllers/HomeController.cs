using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PracaMGR;
using PracaMGR.Models;
using System.Globalization;

namespace Praca.Controllers
{
    public class HomeController : Controller
    {
        readonly JakTraficEntities data = new JakTraficEntities();

        [HttpPost]
        public JsonResult Trasa(StartStop model)
        {
            var przystanki = new List<Przystanki>(data.Przystanki);
            DateTime data_start; 

            if(!DateTime.TryParseExact(model.czas, "dd-MM-yyyy HH:mm", null, DateTimeStyles.None, out data_start))
            {
                return Json(false);
            }
            String s = model.startX +" "+model.startY+" "+model.stopX+" "+model.stopY;

            double x1 = model.startX, y1 = model.startY;
            double x2 = model.stopX, y2 = model.stopY;

            var start = new Przystanki();
            var stop = new Przystanki();

            var start1 = (from a in przystanki let tmp = Odleglosc(x1, a.X, y1, a.Y) select new Odleglosc(a, tmp)).ToList();
            start1 = (from a in start1 orderby a.odl select a).Take(3).ToList();

            var stop1 = (from a in przystanki let tmp = Odleglosc(x2, a.X, y2, a.Y) select new Odleglosc(a, tmp)).ToList();
            stop1 = (from a in stop1 orderby a.odl select a).Take(3).ToList();

            var list = new List<Propozycja>();

            foreach (var a in start1)
            {
                foreach (var b in stop1)
                {
                    list.AddRange(Pobierz(a.p.Numer, b.p.Numer, data_start));
                    list.AddRange(PobierzT(a.p.Numer, b.p.Numer, data_start));
                }
            }

            foreach (var a in start1)
            {
                PrzystankiRaport tt = data.PrzystankiRaport.SingleOrDefault(x => x.Numer_Przystanek == a.p.Numer);

                if(tt != null)
                {
                    tt.Wystapienia++;
                    UpdateModel(tt);
                    data.SaveChanges();
                }
                else
                {
                    var temp = new PrzystankiRaport {Numer_Przystanek = a.p.Numer, Przystanki = a.p, Wystapienia = 1};
                    data.PrzystankiRaport.Add(temp);
                    data.SaveChanges();
                }
                
            }
            foreach (var b in stop1)
            {
                PrzystankiRaport tt = data.PrzystankiRaport.SingleOrDefault(x => x.Numer_Przystanek == b.p.Numer);

                if (tt != null)
                {
                    tt.Wystapienia++;
                    UpdateModel(tt);
                    data.SaveChanges();
                }
                else
                {
                    var temp = new PrzystankiRaport {Numer_Przystanek = b.p.Numer, Przystanki = b.p, Wystapienia = 1};
                    data.PrzystankiRaport.Add(temp);
                    data.SaveChanges();
                }
            }

            foreach(var a in list)
            {
                for (int i = 0; i < a.przystanki.Length-1; i++)
                {
                    var tt = int.Parse(a.przystanki[i]);
                    
                    a.przystanki[i] = przystanki.Single(ss => ss.Numer == tt).Nazwa;
                }
                
            }

            foreach(var a in list)
            {
                if (!string.IsNullOrEmpty(a.linia1))
                {
                    string[] pp = a.linia1.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                    if(pp.Length > 1)
                    {
                        var tt1 = pp[0];
                        var tt2 = pp[1];
                        var t1 = data.LinieRaport.SingleOrDefault(x => x.Linie.Symbol == tt1);
                        var t2 = data.LinieRaport.SingleOrDefault(x => x.Linie.Symbol == tt2);

                        if (t1 != null)
                        {
                            t1.Wystapienia++;
                            UpdateModel(t1);
                            data.SaveChanges();
                        }
                        else
                        {
                            var temp = new LinieRaport();
                            var linie = data.Linie.Single(x => x.Symbol == tt1);
                            temp.Id_linia = linie.Id;
                            temp.Linie = linie;
                            temp.Wystapienia = 1;
                            data.LinieRaport.Add(temp);
                            data.SaveChanges();
                        }

                        if (t2 != null)
                        {
                            t2.Wystapienia++;
                            UpdateModel(t2);
                            data.SaveChanges();
                        }
                        else
                        {
                            var temp = new LinieRaport();
                            var linie = data.Linie.Single(x => x.Symbol == tt2);
                            temp.Id_linia = linie.Id;
                            temp.Linie = linie;
                            temp.Wystapienia = 1;
                            data.LinieRaport.Add(temp);
                            data.SaveChanges();
                        }
                    }
                    else
                    {
                        var t1 = data.LinieRaport.SingleOrDefault(x => x.Linie.Symbol == a.linia1);

                        if (t1 != null)
                        {
                            t1.Wystapienia++;
                            UpdateModel(t1);
                            data.SaveChanges();
                        }
                        else
                        {
                            var temp = new LinieRaport();
                            var linie = data.Linie.Single(x => x.Symbol == a.linia1);
                            temp.Id_linia = linie.Id;
                            temp.Linie = linie;
                            temp.Wystapienia = 1;
                            data.LinieRaport.Add(temp);
                            data.SaveChanges();
                        }
                    }                    
                }           
            }

            list = (from a in list orderby a.czas_przyjazdu, a.do_odjazdu, a.czas_przejazdu select a).ToList();

            return Json(JsonConvert.SerializeObject(list));
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Główna";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public static double Odleglosc(double x1, double x2, double y1, double y2)
        {
            return Math.Acos(((Math.Sin(Rad(x1)) * Math.Sin(Rad(x2))) + Math.Cos(Rad(x1)) * Math.Cos(Rad(x2)) * Math.Cos(Rad(y2 - y1)))) * 6371;
        }
        public static double Rad(double angle)
        {
            return (Math.PI / 180.0) * angle;
        }

        public List<Propozycja> Pobierz(int start, int stop, DateTime data_start)
        {
            var propozycje = new List<Propozycja>();
            var przystanki = new List<Przystanki>(from a in data.Przystanki select a);

            //w linii
            var bezposrednio = new List<Trasy>();
            foreach (Trasy a in data.Trasy)
            {
                if (a.Typ == "All" || a.Typ == "Dni Robocze")
                {
                    String[] przystanek = a.Przystanki.Split(new String[] { "," }, StringSplitOptions.None);
                    String[] czas = a.Czas.Split(new String[] { "," }, StringSplitOptions.None);
                    bool s1 = false, s2 = false;

                    int pozycja_start = 0;
                    int pozycja_stop = 0;

                    for (int i = 0; i < przystanek.Length; i++)
                    {
                        if (int.Parse(przystanek[i]) == start && s1 == false) { s1 = true; pozycja_start = i; }
                        if (int.Parse(przystanek[i]) == stop && s2 == false && s1 == true) { s2 = true; pozycja_stop = i; }
                    }
                    if (s1 == true && s2 == true && pozycja_start < pozycja_stop)
                    {
                        bezposrednio.Add(a);
                    }
                }
            }

            int h = data_start.Hour;
            int m = data_start.Minute;
            String day = data_start.DayOfWeek.ToString();
            Console.WriteLine(h + ":" + m + " " + day);


            foreach (Trasy a in bezposrednio)
            {
                Console.WriteLine(a.Linia);

                String[] przystanek = a.Przystanki.Split(new String[] { "," }, StringSplitOptions.None);
                String[] czas = a.Czas.Split(new String[] { "," }, StringSplitOptions.None);

                int pozycja_start = -1;
                int pozycja_stop = -1;
                int start_min = 0;
                int stop_min = 0;
                String przystanki_odwiedzane = "";

                for (int i = 0; i < przystanek.Length; i++)
                {
                    if (pozycja_start == -1) start_min += int.Parse(czas[i]);
                    if (pozycja_stop == -1) stop_min += int.Parse(czas[i]);


                    if (int.Parse(przystanek[i]) == start && pozycja_start == -1) pozycja_start = i;
                    if (int.Parse(przystanek[i]) == stop && pozycja_start > -1) pozycja_stop = i;
                }
                for (int i = pozycja_start; i < pozycja_stop + 1; i++)
                {
                    przystanki_odwiedzane += przystanek[i] + ",";
                }

                int vars = int.Parse(przystanek[0]);

                var result = from r in data.Odjazdy
                             where r.Symbol_linii == a.Linia &&
                                 //r.Typ == "Soboty" &&
                                   r.Nr_przystanku == vars
                             select r;

                String od = "";

                foreach (Odjazdy o in result)
                {
                    String[] godziny = o.Godziny.Split(new String[] { ", " }, StringSplitOptions.None);


                    for (int i = 0; i < godziny.Length - 1; i++)
                    {
                        if (string.IsNullOrEmpty(a.Znacznik))
                        {
                            int h1 = int.Parse(godziny[i].Substring(0, 2));
                            int m1 = int.Parse(godziny[i].Substring(3, 2));
                            String zn = "";
                            if (godziny[i].Length > 5) zn = godziny[i].Substring(5, godziny[i].Length - 5);
                            if (zn.Length > 0 && !string.IsNullOrEmpty(a.Znacznik))
                            {
                                if (zn.Contains(a.Znacznik)) Console.WriteLine("o");
                            }
                            m1 += start_min;
                            if (m1 >= 60)
                            {
                                h1++;
                                m1 -= 60;
                            }

                            if ((h * 60 + m) < (h1 * 60 + m1))
                            {
                                od = h1 + ":" + m1 + " " + zn;
                                i = godziny.Length;
                            }
                        }
                        else
                        {
                            int h1 = int.Parse(godziny[i].Substring(0, 2));
                            int m1 = int.Parse(godziny[i].Substring(3, 2));
                            String zn = "";
                            if (godziny[i].Length > 5) zn = godziny[i].Substring(5, godziny[i].Length - 5);
                            if (zn.Length > 0 && !string.IsNullOrEmpty(a.Znacznik))
                            {
                                if (zn.Contains(a.Znacznik))
                                {
                                    m1 += start_min;
                                    if (m1 >= 60)
                                    {
                                        h1++;
                                        m1 -= 60;
                                    }

                                    if ((h * 60 + m) < (h1 * 60 + m1))
                                    {
                                        od = h1 + ":" + m1 + " " + zn;
                                        i = godziny.Length;
                                    }
                                }
                            }
                        }

                    }
                    if (od == "")
                    {
                        if (string.IsNullOrEmpty(a.Znacznik))
                        {
                            int h1 = int.Parse(godziny[0].Substring(0, 2));
                            int m1 = int.Parse(godziny[0].Substring(3, 2));
                            String zn = "";
                            if (godziny[0].Length > 5) zn = godziny[0].Substring(5, godziny[0].Length - 5);
                            m1 += start_min;
                            if (m1 >= 60)
                            {
                                h1++;
                                m1 -= 60;
                            }

                            od = h1 + ":" + m1;
                        }
                        else
                        {
                            int counter = 0;
                            for (int i = 0; i < godziny.Length - 1; i++)
                            {
                                String zn = "";
                                if (godziny[i].Length > 5) zn = godziny[i].Substring(5, godziny[i].Length - 5);
                                if (zn.Contains(a.Znacznik))
                                {
                                    counter = i;
                                    i = godziny.Length - 1;
                                }
                            }

                            int h1 = int.Parse(godziny[counter].Substring(0, 2));
                            int m1 = int.Parse(godziny[counter].Substring(3, 2));

                            m1 += start_min;
                            if (m1 >= 60)
                            {
                                h1++;
                                m1 -= 60;
                            }

                            od = h1 + ":" + m1;
                        }
                    }
                }

                int do_odjazdu = 0;
                int na_miejscu = 0;
                int od_h = int.Parse(od.Substring(0, od.IndexOf(":")));
                int od_m = int.Parse(od.Substring(od.IndexOf(":") + 1, 2));

                if ((h * 60 + m) < (od_h * 60 + od_m)) do_odjazdu = (od_h * 60 + od_m) - (h * 60 + m);
                else do_odjazdu = ((24 * 60) - (h * 60 + m)) + (od_h * 60 + od_m);

                na_miejscu = (od_h * 60 + od_m) + (stop_min - start_min);

                Propozycja pr = new Propozycja();
                pr.linia1 = a.Linia;
                pr.czas_odjazdu = new TimeSpan(od_h, od_m, 0);
                pr.czas_przyjazdu = new TimeSpan((int)(Math.Floor(na_miejscu / 60.0)), (int)(Math.Floor(na_miejscu % 60.0)), 0);
                pr.czas_przejazdu = (stop_min - start_min);
                pr.do_odjazdu = do_odjazdu;
                pr.przystanki = przystanki_odwiedzane.Split(new String[] { "," }, StringSplitOptions.None);

                Pozycja[] pp = new Pozycja[50];
                for (int j = 0; j < pr.przystanki.Length - 1; j++)
                {
                    int vv = int.Parse(pr.przystanki[j]);
                    Przystanki tm = przystanki.Single(i => i.Numer == vv);
                    Pozycja tr = new Pozycja();
                    tr.X = tm.X;
                    tr.Y = tm.Y;
                    pp[j] = tr;
                }
                var przystanek_start = przystanki.Single(i => i.Numer == int.Parse(pr.przystanki[0]));
                var przystanek_stop = przystanki.Single(i => i.Numer == int.Parse(pr.przystanki[pr.przystanki.Length - 2]));
                pr.pozycje = pp;
                pr.tekst = "<li class='list-group-item'><b>Wejdź na: " + przystanek_start.Nazwa_rozklad + "</b></li>" +
                           "<li class='list-group-item'><b>Odjazd: " + pr.czas_odjazdu + "</b></li>" +
                           "<li class='list-group-item'><b>Wyjdź na: " + przystanek_stop.Nazwa_rozklad + "</b></li>" +
                           "<li class='list-group-item'><b>Na Miejscu: " + pr.czas_przyjazdu + "</b></li>" +
                           "<li class='list-group-item'><b>Czas podróży: " + pr.czas_przejazdu + " min</b></li>";    


                propozycje.Add(pr);
            }

            return propozycje;
        }

        public List<Propozycja> PobierzT(int start, int stop, DateTime data_start)
        {
            var propozycje = new List<Propozycja>();
            var przystanki = new List<Przystanki>(from a in data.Przystanki select a);

            var dayOfWeek = data_start.DayOfWeek;
            var day = data_start.Day;
            var month = data_start.Month;
            var year = data_start.Year;

            if (dayOfWeek.ToString().Equals("Saturday"))
            {
                Console.WriteLine("Soboty");
            }
            else if (dayOfWeek.ToString().Equals("Sunday"))
            {
                Console.WriteLine("Niedziela - Dni świateczne");
            }
            else
            {
                Console.WriteLine("Zwykłe dni");
            }


            var startP = new List<Trasy>();
            var stopP = new List<Trasy>();

            foreach (var a in data.Trasy)
            {
                var tab = a.Przystanki.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                var t = Array.IndexOf(tab, stop + "");
                var u = tab.Length;

                if (Array.IndexOf(tab, start + "") >= 0 &&
                    Array.IndexOf(tab, stop + "") < 0 && Array.IndexOf(tab, start + "") != tab.Length - 1 &&
                    (a.Typ.Equals("Dni Robocze") || a.Typ.Equals("All")))
                {
                    startP.Add(a);
                }
                if (Array.IndexOf(tab, stop + "") >= 0 &&
                    Array.IndexOf(tab, start + "") < 0 &&
                    (a.Typ.Equals("Dni Robocze") || a.Typ.Equals("All")))
                {
                    stopP.Add(a);
                }
            }

            foreach (Trasy a in startP)
            {
                int dogodny1 = 0;
                int dogodny2 = 0;
                int dogodny1index = 0;
                int dogodny2index = 0;
                string l1 = "", l2 = "";
                double odleglosc = 100000;

                foreach (Trasy b in stopP)
                {
                    if (a.Linia != b.Linia)
                    {
                        String[] przystanekP = a.Przystanki.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        String[] przystanekS = b.Przystanki.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                        int poczatek = Array.IndexOf(przystanekP, start.ToString());
                        int koniec = Array.IndexOf(przystanekS, stop.ToString());

                        int h = data_start.Hour;
                        int m = data_start.Minute;

                        if (poczatek >= 0 && koniec >= 0)//Określenie dogodnej przesiadki
                        {
                            for (int i = poczatek + 1; i < przystanekP.Length; i++)
                            {
                                for (int j = 0; j < koniec; j++)
                                {
                                    var p1 = int.Parse(przystanekP[i]);
                                    var p2 = int.Parse(przystanekS[j]);

                                    var P1 = przystanki.SingleOrDefault(x => x.Numer == p1);
                                    var P2 = przystanki.SingleOrDefault(x => x.Numer == p2);
                                    if (P1 != null && P2 != null)
                                    {
                                        var odl = Odleglosc(P1.X, P2.X, P1.Y, P2.Y);
                                        if (odl < odleglosc)
                                        {
                                            odleglosc = odl;
                                            dogodny1 = P1.Numer;
                                            dogodny2 = P2.Numer;
                                            dogodny1index = i;
                                            dogodny2index = j;
                                            l1 = a.Linia;
                                            l2 = b.Linia;
                                        }
                                    }
                                }
                            }
                            Console.WriteLine("Odleglosc: " + odleglosc + " P1: " + dogodny1 + " P2 " + dogodny2 + " L1: " + l1 + " L2: " + l2);

                            String[] przystanek = a.Przystanki.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                            String[] czas = a.Czas.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                            int przejazd1 = 0, przejazd2 = 0;// dlugosc przejazdów 1 i 2
                            int calosc = 0;
                            String przystanki_odwiedzane = "";

                            for (int i = poczatek; i < dogodny1index + 1; i++)
                            {
                                przejazd1 += int.Parse(czas[i]);
                                calosc += int.Parse(czas[i]);
                                przystanki_odwiedzane += przystanek[i] + ",";
                            }

                            przystanek = b.Przystanki.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                            czas = b.Czas.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                            for (int i = dogodny2index; i < koniec + 1; i++)
                            {
                                przejazd2 += int.Parse(czas[i]);
                                calosc += int.Parse(czas[i]);
                                przystanki_odwiedzane += przystanek[i] + ",";
                            }
                            //Console.WriteLine(przystanki_odwiedzane);

                            int odjazd1 = start;
                            int odjazd2 = dogodny2;

                            var result1 = new List<Odjazdy>(from r in data.Odjazdy
                                                            where r.Symbol_linii == a.Linia &&
                                                            r.Typ == "Dni Robocze" &&
                                                            r.Nr_przystanku == odjazd1
                                                            select r);

                            var result2 = new List<Odjazdy>(from r in data.Odjazdy
                                                            where r.Symbol_linii == b.Linia &&
                                                            r.Typ == "Dni Robocze" &&
                                                            r.Nr_przystanku == odjazd2
                                                            select r);

                            var startT = new TimeSpan();
                            var wysiadkaT = new TimeSpan();
                            int czekanieT = 0;
                            var wsiadanieT = new TimeSpan();
                            var koniecT = new TimeSpan();

                            //String od = "";
                            int mm = 0, hh = 0;
                            foreach (Odjazdy o in result1)
                            {
                                String[] godziny = o.Godziny.Split(new String[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

                                int h1t = int.Parse(godziny[godziny.Length - 1].Substring(0, 2));
                                int m1t = int.Parse(godziny[godziny.Length - 1].Substring(3, 2));

                                if (h * 60 + m > h1t * 60 + m1t)
                                {
                                    startT = new TimeSpan(int.Parse(godziny[0].Substring(0, 2)), int.Parse(godziny[0].Substring(3, 2)), 0);
                                    mm = startT.Minutes;
                                    hh = startT.Hours;
                                }
                                else
                                {

                                    for (int i = 0; i < godziny.Length; i++)
                                    {
                                        int h1 = int.Parse(godziny[i].Substring(0, 2));
                                        int m1 = int.Parse(godziny[i].Substring(3, 2));

                                        if ((h * 60 + m) < (h1 * 60 + m1))
                                        {
                                            startT = new TimeSpan(h1, m1, 0);
                                            i = godziny.Length;
                                            mm = m1;
                                            hh = h1;
                                        }
                                    }
                                }
                            }

                            wysiadkaT = startT.Add(new TimeSpan(0, przejazd1, 0));

                            int minDodacZaOdleglosc = (int)(odleglosc * 10);
                             
                            var nextTime = wysiadkaT.Add(new TimeSpan(0, minDodacZaOdleglosc, 0));
                            h = nextTime.Hours;
                            m = nextTime.Minutes;
                            foreach (Odjazdy o in result2)
                            {
                                String[] godziny = o.Godziny.Split(new String[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

                                int h1t = int.Parse(godziny[godziny.Length - 1].Substring(0, 2));
                                int m1t = int.Parse(godziny[godziny.Length - 1].Substring(3, 2));

                                if (h * 60 + m > h1t * 60 + m1t)
                                {
                                    wsiadanieT = new TimeSpan(int.Parse(godziny[0].Substring(0, 2)), int.Parse(godziny[0].Substring(3, 2)), 0);
                                }
                                else
                                {
                                    for (int i = 0; i < godziny.Length; i++)
                                    {
                                        int h1 = int.Parse(godziny[i].Substring(0, 2));
                                        int m1 = int.Parse(godziny[i].Substring(3, 2));

                                        if ((h * 60 + m) < (h1 * 60 + m1))
                                        {
                                            wsiadanieT = new TimeSpan(h1, m1, 0);
                                            i = godziny.Length;
                                        }
                                    }
                                }
                            }
                            czekanieT = (int)(wsiadanieT.TotalMinutes - wysiadkaT.TotalMinutes);

                            koniecT = wsiadanieT.Add(new TimeSpan(0, przejazd2, 0));

                            var pr = new Propozycja();
                            pr.linia1 = string.IsNullOrEmpty(b.Linia) ? a.Linia : a.Linia + ", " + b.Linia;
                            pr.linia2 = b.Linia;
                            pr.czas_odjazdu = startT;
                            pr.wysiadka = wysiadkaT;
                            pr.przesiadka = wsiadanieT;
                            pr.czas_przyjazdu = koniecT;
                            pr.czekanie = czekanieT;
                            pr.przystanki = przystanki_odwiedzane.Split(new String[] { "," }, StringSplitOptions.None);

                            h = data_start.Hour;
                            m = data_start.Minute;
                            int do_odjazdu = 0;
                            if ((h * 60 + m) < (startT.Hours * 60 + startT.Minutes)) do_odjazdu = (startT.Hours * 60 + startT.Minutes) - (h * 60 + m);
                            else do_odjazdu = ((24 * 60) - (h * 60 + m)) + (startT.Hours * 60 + startT.Minutes);
                            pr.do_odjazdu = do_odjazdu;

                            pr.czas_przejazdu = (int)(koniecT.TotalMinutes - startT.TotalMinutes);
                            if (pr.czas_przejazdu <= 0)
                            {
                                pr.czas_przejazdu = (int)((3600 - startT.TotalMinutes) + koniecT.TotalMinutes);
                            }

                            pr.tekst = "<li class='list-group-item'><b>Wejdź na: " + przystanki.Single(i => i.Numer == int.Parse(pr.przystanki[0])).Nazwa_rozklad + "</b></li>" +
                                       "<li class='list-group-item'><b>Odjazd: " + pr.czas_odjazdu + "</b></li>" +
                                       "<li class='list-group-item'><b>Wyjdź: " + wysiadkaT + " na " + przystanki.Single(i => i.Numer == dogodny1).Nazwa_rozklad + "</b></li>" +
                                       ((odleglosc*10)>0 ? "<li class='list-group-item'><b>Przejdź: " + Math.Round(odleglosc,2) + " km ("+ (int)(Math.Round(odleglosc, 2)*10) + " min)</b></li>" : "") +
                                       "<li class='list-group-item'><b>Poczekaj " + pr.czekanie + " min</b></li>" +
                                       "<li class='list-group-item'><b>Wsiądź: " + wsiadanieT + " na " + przystanki.Single(i => i.Numer == dogodny2).Nazwa_rozklad + "</b></li>" +
                                       "<li class='list-group-item'><b>Wyjdź: " + koniecT + " na " + przystanki.Single(i => i.Numer == int.Parse(pr.przystanki[pr.przystanki.Length - 2])).Nazwa_rozklad + "</b></li>" +
                                       "<li class='list-group-item'><b>Na Miejscu: " + pr.czas_przyjazdu + "</b></li>" +
                                       "<li class='list-group-item'><b>Czas podróży: " + pr.czas_przejazdu + " min</b></li>";

                            Pozycja[] pp = new Pozycja[50];
                            for (int j = 0; j < pr.przystanki.Length - 1; j++)
                            {
                                int vv = int.Parse(pr.przystanki[j]);
                                Przystanki tm = przystanki.Single(i => i.Numer == vv);
                                Pozycja tr = new Pozycja();
                                tr.X = tm.X;
                                tr.Y = tm.Y;
                                pp[j] = tr;
                            }
                            pr.pozycje = pp;

                            pr.przesiadkaPrzystanek = dogodny1 + "," + dogodny2;
                            propozycje.Add(pr);
                        }
                    }
                }
            }

            return propozycje;
        }
    }
}