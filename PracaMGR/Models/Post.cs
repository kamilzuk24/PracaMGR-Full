using PagedList;

namespace PracaMGR.Models
{
    public class Post
    {
        public Posty post { get; set; }
        public IPagedList<Odpowiedzi> odpowiedz { get; set; }
    }
}