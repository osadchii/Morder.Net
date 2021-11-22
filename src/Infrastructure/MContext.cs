using Infrastructure.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class MContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    public MContext(DbContextOptions<MContext> options) : base(options)
    {

    }
}