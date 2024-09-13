using Microsoft.EntityFrameworkCore;
using SearchService.Models;

namespace SearchService.Data;

public class SearchDbContext : DbContext
{
    public virtual DbSet<Transaction> Transactions { get; set; }

    public SearchDbContext(DbContextOptions<SearchDbContext> options) : base(options)
    {

    }
}
