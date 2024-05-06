using Microsoft.EntityFrameworkCore.Storage;

namespace KnowledgeNexus.Models
{
    public class Books
    {
        //Primary key
        public int BooksId { get; set; }

        //Book name
        public string Name { get; set; } = string.Empty;

        //Book description
        public string Description { get; set; } = string.Empty;

        //Book image file name
        public string FileName { get; set; } = string.Empty;

        //Book price
        public decimal Price { get; set; }

        

    }

}
