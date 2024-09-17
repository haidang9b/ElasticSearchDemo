using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ESD.SearchService.Data;

public class SearchDbContext : DbContext
{
    public virtual DbSet<Transaction> Transactions { get; set; }

    public SearchDbContext(DbContextOptions<SearchDbContext> options) : base(options)
    {

    }
}
