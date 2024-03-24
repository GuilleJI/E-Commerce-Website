using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KnowledgeNexus.Data;
using KnowledgeNexus.Models;
using Microsoft.AspNetCore.Authorization;

namespace KnowledgeNexus.Pages.BookAdmin
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly KnowledgeNexus.Data.KnowledgeNexusContext _context;

        public EditModel(KnowledgeNexus.Data.KnowledgeNexusContext context)
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

            var books =  await _context.Books.FirstOrDefaultAsync(m => m.BooksId == id);
            if (books == null)
            {
                return NotFound();
            }
            Books = books;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingBook = await _context.Books.FirstOrDefaultAsync(m => m.BooksId == Books.BooksId);

            if (existingBook == null)
            {
                return NotFound();
            }

            //Updating properties of existingBook with values from Books
            existingBook.Name = Books.Name; 
            existingBook .Description = Books.Description;
            existingBook.Price = Books.Price;
            existingBook.Quantity = Books.Quantity;

            //Retain the existing FIleName value 
            Books.FileName = existingBook.FileName;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BooksExists(Books.BooksId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool BooksExists(int id)
        {
            return _context.Books.Any(e => e.BooksId == id);
        }


    }
}
