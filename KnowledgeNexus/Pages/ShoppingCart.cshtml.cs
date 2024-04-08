using KnowledgeNexus.Data;
using KnowledgeNexus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
            string? cookieValue = Request.Cookies["ProductIDs"];

            // Cookie does not exist
            if (cookieValue == null)
            {
                // Create cookie and set its initial value to 0
                createCookie("");
            }
            else// If the cookie exists, parse its value into ProductIDs list
            {
                // Parse the value of the cookie and render the page to display a list of the products with details including image. 
                string[] ids = cookieValue.Split("-");

                // Add product IDs to the list
                ProductIDs.AddRange(ids.Select(int.Parse));

                // Get the products from the database
                Books = await _context.Books.Where(g => ProductIDs.Contains(g.BooksId)).ToListAsync();

                // Calculate the sum of the cart
                CartSum = ProductIDs.Count;
            }
        }

        private void createCookie(string count)
        {
            Response.Cookies.Append("ProductIDs", count, new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(1)
            });
        }
    }
}