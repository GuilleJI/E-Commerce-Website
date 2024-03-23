using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using KnowledgeNexus.Data;
using KnowledgeNexus.Models;

namespace KnowledgeNexus.Pages.BookAdmin
{
    public class CreateModel : PageModel
    {
        //Seperating class properties 

        private readonly KnowledgeNexus.Data.KnowledgeNexusContext _context;

        [BindProperty]
        public Books Books { get; set; } = default!;

        public CreateModel(KnowledgeNexus.Data.KnowledgeNexusContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Set the Selection Date for the book being selected by the buyer 

            Books.SelectionDate = DateTime.Now;

            // Sync .net context with database (execute insert command) 
            _context.Books.Add(Books);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
