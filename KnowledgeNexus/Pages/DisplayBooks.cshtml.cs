using Humanizer;
using KnowledgeNexus.Data;
using KnowledgeNexus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeNexus.Pages
{
    public class DisplayBooksModel : PageModel
    {
        //Class properties
        private readonly KnowledgeNexusContext _context;

        public Books Books { get; set; } = default!;

        //Constructor 
        public DisplayBooksModel(KnowledgeNexusContext context)
        {
            _context = context;
        }

        //Get 
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound(); 
            }
            var books = await _context.Books.FirstOrDefaultAsync(m => m.BooksId == id);

            if (books == null)
            {
                return NotFound();
            }
            else
            {
                Books = books;
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            var cartCookie = Request.Cookies["ShoppingCart"];

            // Get the product ID from the books object
            int productId = Books.BooksId;

            //If the cart cookie already exist, append the new BooksId to it
            if(cartCookie != null)
            {
                cartCookie += "," + productId;
            }
            else // If the cart cookie doesn't exist, create a new one with the BooksId
            {
                cartCookie = productId.ToString(); 
            }

            // Update the cookie with the new cart content
            Response.Cookies.Append("ShoppingCart", cartCookie);

            // Redirect to the home page 
            return RedirectToPage("/Index"); 
        }
       
       


    }
}
