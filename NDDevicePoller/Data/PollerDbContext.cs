using Microsoft.EntityFrameworkCore;
using NDDevicePoller.Models;

namespace NDDevicePoller.Data
{
    public class PollerDBContext : DbContext
    {
        public PollerDBContext(DbContextOptions<PollerDBContext> options) : base(options)
        {

        }

        public DbSet<MonitorTypes> MonitorTypes { get; set; }
        public DbSet<NetworkDevice> NetworkDevices { get; set; }
        public DbSet<CrossServiceSettings> CrossPlatformSettings { get; set; }
    }

    
}
