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

        //Injecting cookies into DetailsPage ()
        public List <int> BookIds { get; set; } = new List<int>();
        public int cartSum { get; set; } = 0;

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
                // Get the existing code value 
                string? CookieValue = Request.Cookies["BookIds"];


                //if cookie does not exist
                if (CookieValue == null)
                {
                    //Create cookie and set its initial value to 0 
                    createCookie("0");
                }
                else //If cookies does exis, simply parse its value into ProductIds List
                {
                    cartSum = CookieValue.Split("-").Length;
                    
                }

            }
            return Page();
        }

        private void createCookie(string value) //we create a function to create that cookie and set its value to count.In addition, we'll set up the cookie with a 1 day expiry (24hrs)
        {
            Response.Cookies.Append("BookIds", value, new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(1)
            });
        }

        public IActionResult OnPost()
        {
            //Make sure the form has been submitted with the actual form data
            if (HttpContext.Request.Form.Count > 0)
            {
                //Get existing cookie value 
                string? CookieValue = Request.Cookies["BookIds"];

                //Get the Book Id from the form
                string BookId = HttpContext.Request.Form["id"];

                if(CookieValue != null) 
                {
                    //Add the new Book ID to the existing cookie 
                    CookieValue += "-" + BookId;
                    createCookie(CookieValue);
                }
            }
            return RedirectToPage("Index");
        }



    }
}
