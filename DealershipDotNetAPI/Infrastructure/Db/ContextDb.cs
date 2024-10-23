using DealershipDotNetAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DealershipDotNetAPI.Infrastructure.Db
{
    public class ContextDb : DbContext
    {
        public DbSet<Administrator> Administrators { get; set; } = default!;
    }
}
