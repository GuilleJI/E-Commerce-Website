using KnowledgeNexus.Data;
using KnowledgeNexus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeNexus.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly KnowledgeNexusContext _context;

        public IList<Books> Books { get; set; } = default!; 

        public IndexModel(ILogger<IndexModel> logger, KnowledgeNexusContext context)
        {
            _logger = logger;
            _context = context; 
        }

        public async Task OnGetAsync()
        {
            Books = await _context.Books.ToListAsync(); 
        }
    }
}
