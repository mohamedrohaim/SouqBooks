using Microsoft.EntityFrameworkCore;
using SouqBooks.Models;

namespace SouqBooks.Data
{
    public class ApplicationDbContext:DbContext
    {

        public ApplicationDbContext(DbContextOptions options):base(options) 
        {

        }



        DbSet<Category> categories { get; set; }
    }
}
