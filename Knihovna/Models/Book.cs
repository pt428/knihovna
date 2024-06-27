namespace Knihovna.Models
{
    public class Book
    {
        public int Id { get; set; }
       
        public string AuthorName { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public string Year { get; set; }
        public bool Reserved { get; set; }
        public bool Borrowed { get; set; }
   public AppUser UserWhoBorrowed { get; set; }=new AppUser();


    }
}
