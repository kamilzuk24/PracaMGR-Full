namespace PracaMGR.Models
{
    public class Odleglosc
    {
        public Przystanki p { get; set; }
        public double odl { get; set; }

        public Odleglosc(Przystanki a, double tmp)
        {
            this.p = a;
            this.odl = tmp;
        }
    }
}