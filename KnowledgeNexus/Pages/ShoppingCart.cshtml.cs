using KnowledgeNexus.Data;
using KnowledgeNexus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeNexus.Pages
{
    public class ShoppingCartModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly KnowledgeNexusContext _context;

        public List<int> ProductIDs { get; set; } = new List<int>(); 
        public IList<Books> Books { get; set; } = default!;
        
        // Index only need to read the cookie, and sum up, display the CartSum
        public int CartSum { get; set; } = 0;


        public ShoppingCartModel(ILogger<IndexModel> logger, KnowledgeNexusContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task OnGetAsync()
        {
            //
            // Get the existing cookie value
            string? cookieValue = Request.Cookies["ProductIDs"]; // Can only read the cookie value 1st character, not the whole string

            // Cookie does not exist
            if (cookieValue == null)
            {
                // Create cookie and set its initial value to 0
                createCookie(0);
            }
            else// If the cookie exists, parse its value into ProductIDs list
            {
                // Fix how to get the length of "9,3,2,8,9,9"
                CartSum = cookieValue.Split("-").Length;

                // Parse the value of the cookie and render the page to display a list of the products with details including image. 
                string[] ids = cookieValue.Split("-");

                // Add product IDs to the list
                ProductIDs.AddRange(ids.Select(int.Parse));



            }
            // Get the products from the database
            Books = await _context.Books.Where(g => ProductIDs.Contains(g.BooksId)).ToListAsync();
        }

        private void createCookie(int count )
        {
            Response.Cookies.Append("ProductIDs", count.ToString(), new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(1)
            });
        }
    }
}
