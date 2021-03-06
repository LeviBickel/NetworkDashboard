using Microsoft.EntityFrameworkCore;
using NDDevicePoller.Data;
using NetworkDashboard.Models;

namespace NetworkDashboard.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }

        public DbSet<MonitorTypes> MonitorTypes { get; set; }
        public DbSet<NetworkDevice> NetworkDevices { get; set; }

        public DbSet<CrossServiceSettings> CrossPlatformSettings { get; set; }
    }
}
