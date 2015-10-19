using System;

namespace PracaMGR.Models
{
    public class Propozycja
    {        
        public String linia1 { get; set; }

        public String linia2 { get; set; }

        public TimeSpan czas_odjazdu { get; set; }

        public TimeSpan czas_przyjazdu { get; set; }

        public TimeSpan wysiadka { get; set; }

        public TimeSpan przesiadka { get; set; }

        public String[] przystanki { get; set; }

        public Pozycja[] pozycje { get; set; }

        public int czas_przejazdu { get; set; }

        public int do_odjazdu { get; set; }

        public int czekanie { get; set; }

        public string przesiadkaPrzystanek { get; set; }

        public string tekst { get; set; }
    }
}