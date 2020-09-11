using System;
using System.Collections.Generic;
using System.Text;
using EConference.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EConference.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ConferenceName> ConferenceName { get; set; }
        public DbSet<RegisterPaper> PapersRegistered { get; set; }
    }
}
