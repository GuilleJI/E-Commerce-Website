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

       
       


    }
}
