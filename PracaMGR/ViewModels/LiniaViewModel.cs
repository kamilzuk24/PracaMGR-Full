namespace PracaMGR.ViewModels
{
    public class LiniaViewModel
    {
        public Linie Record { get; set; }
        public double Ocena { get; set; }

        public LiniaViewModel()
        {
            this.Ocena = 0;
        }
    }
}