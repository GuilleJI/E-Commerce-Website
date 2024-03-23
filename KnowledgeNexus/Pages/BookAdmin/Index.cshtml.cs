﻿using System;
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
    public class IndexModel : PageModel
    {
        private readonly KnowledgeNexus.Data.KnowledgeNexusContext _context;
        public IList<Books> Books { get;set; } = default!;


        public IndexModel(KnowledgeNexus.Data.KnowledgeNexusContext context)
        {
            _context = context;
        }
 
        public async Task OnGetAsync()
        {
            Books = await _context.Books.ToListAsync();
        }
    }
}
