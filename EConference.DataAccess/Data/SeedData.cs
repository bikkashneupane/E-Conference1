using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using EConference.DataAccess.Data;

namespace EConference.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            ApplicationDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            if (!context.ConferenceName.Any())
            {
                context.ConferenceName.AddRange(
                    new ConferenceName
                    {
                        Name = "Pending"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}

