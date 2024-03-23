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
    public class DetailsModel : PageModel
    {
        private readonly KnowledgeNexus.Data.KnowledgeNexusContext _context;

        public DetailsModel(KnowledgeNexus.Data.KnowledgeNexusContext context)
        {
            _context = context;
        }

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
    }
}
