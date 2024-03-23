using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KnowledgeNexus.Models;

namespace KnowledgeNexus.Data
{
    public class KnowledgeNexusContext : DbContext
    {
        public KnowledgeNexusContext (DbContextOptions<KnowledgeNexusContext> options)
            : base(options)
        {
        }

        public DbSet<KnowledgeNexus.Models.Books> Books { get; set; } = default!;
    }
}
