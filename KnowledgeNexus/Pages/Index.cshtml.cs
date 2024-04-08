using KnowledgeNexus.Data;
using KnowledgeNexus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeNexus.Pages
{
    public class IndexModel : PageModel
    {
        
       
        private readonly ILogger<IndexModel> _logger;
        private readonly KnowledgeNexusContext _context;
        public IList<Books> Books { get; set; } = default!;

        // Cookie to store product IDs
        public List<int> ProductIDs { get; set; } = new List<int>();
        public int CartSum { get; set; } = 0;
        public IndexModel(ILogger<IndexModel> logger, KnowledgeNexusContext context)
        {
            _logger = logger;
            _context = context; 
        }

        public async Task OnGetAsync()
        {
            Books = await _context.Books.ToListAsync();

            // Get the existing cookie value
            string? cookieValue = Request.Cookies["ProductIDs"];

            // Cookie does not exist
            if (cookieValue == null)
            {
                // Create cookie and set its initial value to 0
                createCookie("");
            }
            else// If the cookie exists, parse its value into ProductIDs list
            {

                CartSum = cookieValue.Split("-").Length;

            }
        

        }
            // A helper function to create a cookie and set its value to count
            private void createCookie(string count)
            {
                Response.Cookies.Append("ProductIDs", count.ToString(), new CookieOptions()
                {
                    Expires = DateTime.Now.AddDays(1)
                });
            }
    }
}
