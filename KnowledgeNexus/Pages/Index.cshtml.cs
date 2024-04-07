using KnowledgeNexus.Data;
using KnowledgeNexus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace KnowledgeNexus.Pages
{
    public class IndexModel : PageModel
    {
        
       
        private readonly ILogger<IndexModel> _logger;
        private readonly KnowledgeNexusContext _context;
        public IList<Books> Books { get; set; } = default!; 

        public IndexModel(ILogger<IndexModel> logger, KnowledgeNexusContext context)
        {
            _logger = logger;
            _context = context; 
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Books = await _context.Books.ToListAsync();

            // Check if the cookie exists 
            if (!Request.Cookies.ContainsKey("ShoppingCartCookie"))
            {
                // Create a new cookie
                Response.Cookies.Append("ShoppingCartCookie", "value", new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddDays(1)
                });
            }

            return Page(); 

        } 


    }
}
