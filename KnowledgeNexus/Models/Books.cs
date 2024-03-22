namespace KnowledgeNexus.Models
{
    public class Books
    {
        public int BookId { get; set; }
        public string BookName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public decimal Price { get; set; }
    }

}
