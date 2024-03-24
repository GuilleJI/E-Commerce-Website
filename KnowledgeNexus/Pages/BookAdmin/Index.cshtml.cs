using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KnowledgeNexus.Data;
using KnowledgeNexus.Models;
using Microsoft.AspNetCore.Authorization;

namespace KnowledgeNexus.Pages.BookAdmin
{
    [Authorize]
    public class IndexModel : PageModel
    {
        // Properties should be at the top od the class

        private readonly KnowledgeNexusContext _context;
        public IList<Books> Books { get;set; } = default!;


        public IndexModel(KnowledgeNexusContext context)
        {
            _context = context;
        }
 
        public async Task OnGetAsync()
        {
            Books = await _context.Books.ToListAsync();
        }
    }
}
