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

        public List<int> ProductIDs { get; set; } = new List<int>();

        public int CartSum { get; set; } = 0;

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

                //
                // Get the existing cookie value
                string? cookieValue = Request.Cookies["ProductIDs"]; // Can only read the cookie value 1st character, not the whole string

                // Cookie does not exist
                if (cookieValue == null)
                {
                    // Create cookie and set its initial value to 0
                    createCookie("0");
                }
                else// If the cookie exists, parse its value into ProductIDs list
                {
                    

                    CartSum = cookieValue.Split("-").Length;

                }
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            // Check if the form has been submitted with actual form data
            if (HttpContext.Request.Form.Count > 0)
            {
                // Get the existing cookie value
                string? cookieValue = Request.Cookies["ProductIDs"];

                // Get the product ID from the form
                string productId = HttpContext.Request.Form["id"];

                if (cookieValue != null)
                {
                    // Add the new product ID to the existing cookie
                    cookieValue += "-" + productId;
                    createCookie(cookieValue);
                }
            }

            return RedirectToPage("Index");
        }

        private void createCookie(string value)
        {
            Response.Cookies.Append("ProductIDs", value, new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(1)
            });
        }

    }
}
