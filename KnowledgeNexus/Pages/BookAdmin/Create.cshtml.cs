using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using KnowledgeNexus.Data;
using KnowledgeNexus.Models;
using System.ComponentModel;

namespace KnowledgeNexus.Pages.BookAdmin
{
    public class CreateModel : PageModel
    {
        //Seperating class properties 

        private readonly KnowledgeNexus.Data.KnowledgeNexusContext _context;

        //Injecting IHostEnvironment into constructor properties

        private readonly IHostEnvironment _environment;

        [BindProperty]
        public Books Books { get; set; } = default!;

        [BindProperty]
        [DisplayName("Select Book")]
        public IFormFile FileUpload { get; set; }

        // Injecting IHostEnvironment to plug _environment into constructor
        public CreateModel(KnowledgeNexusContext context, IHostEnvironment environment) 
        {
            _context = context;
            _environment = environment; // Initialized environment 
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

            //
            // Upload File Server
            //

            string filename = FileUpload.FileName;

            // Update Book object to include the book filename
            Books.FileName = filename;

            //Save the file 
            string projectRootPath = _environment.ContentRootPath;
            string fileSavePath = Path.Combine(projectRootPath, "wwwroot", "uploads", filename);

            // Use a "using" to ensure the filestream is disposed of when we're done with it 

            using (FileStream fileStream = new FileStream(fileSavePath, FileMode.Create))
            {
                FileUpload.CopyTo(fileStream);
            }

            // Update the .net context
            _context.Books.Add(Books);

            // Sync .net context with database (execute insert command) 
            _context.Books.Add(Books);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
