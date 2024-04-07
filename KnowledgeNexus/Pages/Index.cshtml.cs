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

        public IndexModel(ILogger<IndexModel> logger, KnowledgeNexusContext context)
        {
            _logger = logger;
            _context = context; 
        }

        public async Task OnGetAsync()
        {
            Books = await _context.Books.ToListAsync();

            //Check if the cookies exists
            if (!Request.Cookies.ContainsKey("BookRecord"))
            {
                //Create a new cookie with a 1-day expiry
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(1),
                    IsEssential = true // Make the cookie essential
                };

                // Set the cookie value 
                Response.Cookies.Append("BookRecord", "cookie_value", cookieOptions);
            }

            //Your existing code to fetch books can go here
            Books = await _context.Books.ToListAsync(); 

        }

    }
}
