using Microsoft.EntityFrameworkCore;
using NDDevicePoller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDDevicePoller.Data
{
    public class DBHelper
    {
        private PollerDBContext _dbContext;

        private DbContextOptions<PollerDBContext> GetAllOptions()
        {
            var optionsBuilder = new DbContextOptionsBuilder<PollerDBContext>();
            optionsBuilder.UseSqlServer(AppSettings.ConnectionString);
            return optionsBuilder.Options;
        }
        public List<NetworkDevice> GetAllDevices()
        {
            using (_dbContext = new PollerDBContext(GetAllOptions()))
            {
                var devices = _dbContext.NetworkDevices.ToList();
                if (devices.Any()) 
                { 
                    return devices;
                }
                else
                {
                    return new List<NetworkDevice>();
                }

            }
        }

        public List<MonitorTypes> GetAllMonitorTypes()
        {
            using (_dbContext = new PollerDBContext(GetAllOptions()))
            {
                var types = _dbContext.MonitorTypes.ToList();
                if (types.Any())
                {
                    return types;
                }
                else
                {
                    return new List<MonitorTypes>();
                }

            }
        }
        
        public async Task UpdateConnectionStatus (NetworkDevice device)
        {
            try
            {
                using (_dbContext = new PollerDBContext(GetAllOptions()))
                {
                    _dbContext.Update(device);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NetworkDeviceExists(device.Id))
                {
                    return;
                }
            }
        }

        public int GetPollInterval()
        {
            using (_dbContext = new PollerDBContext(GetAllOptions()))
            {
                int interval = _dbContext.CrossPlatformSettings.First().PollInterval;
                return interval;
            }
        }
        private bool NetworkDeviceExists(int id)
        {
            using (_dbContext = new PollerDBContext(GetAllOptions()))
            {
                return _dbContext.NetworkDevices.Any(e => e.Id == id);
            }
                
        }
    }
}
