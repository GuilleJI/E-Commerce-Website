using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KnowledgeNexus.Data;
using KnowledgeNexus.Models;

namespace KnowledgeNexus.Pages.BookAdmin
{
    public class DeleteModel : PageModel
    {
        private readonly KnowledgeNexus.Data.KnowledgeNexusContext _context;

        public DeleteModel(KnowledgeNexus.Data.KnowledgeNexusContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Books Books { get; set; } = default!;

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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var books = await _context.Books.FindAsync(id);
            if (books != null)
            {
                Books = books;
                _context.Books.Remove(Books);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
