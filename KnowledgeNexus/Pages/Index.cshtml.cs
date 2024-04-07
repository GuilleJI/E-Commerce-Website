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

        //Creating Cookie to store Book Id's (cookies 1 of 7)
        public List<int> BookIds { get; set; } = new List<int>();

        //Display the Cart Sum (cookies 2 of 7)
        public int cartSum { get; set; }

        public IndexModel(ILogger<IndexModel> logger, KnowledgeNexusContext context)
        {
            _logger = logger;
            _context = context; 
        }

        public async Task OnGetAsync()
        {
            Books = await _context.Books.ToListAsync();

            //Inside out onGetAsync() function, Get exisiting value for the Cookie (cookies 3 of 7)
            string? CookieValue = Request.Cookies["BookIds"];

            //If that cookie does not exist: (cookies 4 of 7)
            if (CookieValue == null) 
            {
                //we then create a cookie and set it up to its initial value, which is zero (cookies 5 of 7)
                createCookie(0);
            }
            else //However, if the cookies does exist, we parse its value into BookId list (cookies 6 of 7)
            {
                cartSum = CookieValue.Split("-").Length; 

            }

        }

        //Finally we create a function to create that cookie and set its value to count. In addition, we'll set up the cookie with a 1 day expiry (24hrs) (cookies 7 of 7)
        private void createCookie(int count)
        {
            Response.Cookies.Append("BookIds", count.ToString(), new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(1)
            });
        }



    }
}
