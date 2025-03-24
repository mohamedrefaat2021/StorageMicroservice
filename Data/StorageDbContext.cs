using Microsoft.EntityFrameworkCore;
using StorageMicroservice.Models;

namespace StorageMicroservice.Data
{
    public class StorageDbContext : DbContext
    {
        public StorageDbContext(DbContextOptions<StorageDbContext> options) : base(options) { }

        public DbSet<FileMetadata> FileMetadata { get; set; }
    }
}
