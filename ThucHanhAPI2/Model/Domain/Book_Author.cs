namespace ThucHanhAPI2.Model.Domain
{
    public class Book_Author
    {
        public int Id { get; set; }
        public int BookId { get; set; }

        public Book? Book { get; set; }
        public Author? Author { get; set; }
    }
}
