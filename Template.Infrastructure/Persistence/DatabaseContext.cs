using Microsoft.EntityFrameworkCore;

namespace Template.Infrastructure.Persistence;

public class DatabaseContext: DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }
}