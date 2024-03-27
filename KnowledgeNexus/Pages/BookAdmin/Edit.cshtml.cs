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
using System.ComponentModel;
using Microsoft.Extensions.Hosting;

namespace KnowledgeNexus.Pages.BookAdmin
{
    /// <summary>
    /// Essential: Use [Authorize] for secure pages in ASP.NET (restricted access except login user)
    /// </summary>
    [Authorize] 
    public class EditModel : PageModel
    {
        private readonly KnowledgeNexusContext _context;

        private readonly ILogger<EditModel> _logger;

        private readonly IHostEnvironment _environment;

        public EditModel(KnowledgeNexusContext context, ILogger<EditModel> logger,
        IHostEnvironment environment)
        {
            _context = context;
            _logger = logger;
            _environment = environment;
        }

        [BindProperty]
        public Books Books { get; set; } = default!;


        /// <summary>
        /// [BindProperty] and [DisplayName(“Change Image”)] link form data to the 
        /// ‘FileUpload’ property and display “Change Image” as the label.
        /// </summary>
        

        [BindProperty]
        [DisplayName("Change Image")]
        public IFormFile? FileUpload { get; set; }

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

            //Change photo 
            if (FileUpload != null)
            {
                // Upload file to server 
                string filename = FileUpload.FileName;
                //Save the file 
                string projectRootPath = _environment.ContentRootPath; 
                string fileSavePath = Path.Combine(projectRootPath,"wwwroot\\uploads", filename);

                // We use a "using" to ensure the filestream is disposed of when we're done with it
                using(FileStream fileStream = new FileStream(fileSavePath, FileMode.Create))
                {
                    FileUpload.CopyTo(fileStream);
                }

                //Updating the fileName property of the existing book
                existingBook.FileName = filename;
            }
          
            //Updating properties of existingBook with values from Books
            existingBook.Name = Books.Name;
            existingBook.Description = Books.Description;
            existingBook.Price = Books.Price;
            

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