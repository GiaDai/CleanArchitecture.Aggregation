namespace CleanArchitecture.Aggregation.WebApi.Models
{
    public class Book
    {
        public string Title { get; set; }
        public string Isbn { get; set; }
        public int PageCount { get; set; }
        public string ThumbnailUrl { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Status { get; set; }
        public string Authors { get; set; }
        public string Categories { get; set; }

        // create a static funciton generate random book
        public static Book GenerateRandomBook()
        {
            return new Book
            {
                Title = "Book Title",
                Isbn = "1234567890",
                PageCount = 100,
                ThumbnailUrl = "https://www.google.com",
                ShortDescription = "Short Description",
                LongDescription = "Long Description",
                Status = "Available",
                Authors = "Author 1, Author 2",
                Categories = "Category 1, Category 2"
            };
        }
    }


}
