using BackBetha.Models;
using Microsoft.EntityFrameworkCore;

namespace BackBetha.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }
        public DbSet<Client> Clients { get; set; }

        public DbSet<ClientSendingRecord> ClientSendingRecords { get; set;}       
    }
}
