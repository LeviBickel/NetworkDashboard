using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetworkDashboard.Data;
using NetworkDashboard.Models;

namespace NetworkDashboard.Controllers
{
    public class NetworkDevicesController : Controller
    {
        private readonly DBContext _context;
        
        public NetworkDevicesController(DBContext context)
        {
            _context = context;
        }

        // GET: NetworkDevices
        public async Task<IActionResult> Index()
        {
            return View(await _context.NetworkDevices.ToListAsync());
        }

        // GET: NetworkDevices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var networkDevice = await _context.NetworkDevices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (networkDevice == null)
            {
                return NotFound();
            }

            return View(networkDevice);
        }


        public IActionResult Create()
        {
            var monitorTypes = _context.MonitorTypes.ToList();
            if(monitorTypes.Count < 1)
            {
                ViewBag.data = null;
            }
            else
            {
                ViewBag.data = monitorTypes;
            }
            
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DisplayName,DeviceName,DeviceType,DeviceDescription,IPAddress,IsConnected")] NetworkDevice networkDevice)
        {
            //Utils utils = new Utils();
            if (ModelState.IsValid)
            {
                //var nonPingCategories = _context.MonitorTypes.Where(m => m.ping == false).Where(m=> m.TypeTitle == networkDevice.DeviceType).ToList();

                //if (nonPingCategories != null && nonPingCategories.Count >= 1)
                //{
                //    networkDevice.IsConnected = utils.HTTPCheckHostAsync(networkDevice).Result;
                //}
                //else
                //{
                //    if (!utils.PingHost(networkDevice.IPAddress))
                //    {
                //        networkDevice.IsConnected = utils.PingHost(networkDevice.DeviceName);
                //    }
                //    else
                //    {
                //        networkDevice.IsConnected = true;
                //    }
                //}
                _context.Add(networkDevice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(networkDevice);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.data = _context.MonitorTypes.ToList();
            var networkDevice = await _context.NetworkDevices.FindAsync(id);
            if (networkDevice == null)
            {
                return NotFound();
            }
            return View(networkDevice);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DisplayName,DeviceName,DeviceType,DeviceDescription,IPAddress,IsConnected")] NetworkDevice networkDevice)
        {
            if (id != networkDevice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(networkDevice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NetworkDeviceExists(networkDevice.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(networkDevice);
        }

        // GET: NetworkDevices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var networkDevice = await _context.NetworkDevices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (networkDevice == null)
            {
                return NotFound();
            }

            return View(networkDevice);
        }

        // POST: NetworkDevices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var networkDevice = await _context.NetworkDevices.FindAsync(id);
            _context.NetworkDevices.Remove(networkDevice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NetworkDeviceExists(int id)
        {
            return _context.NetworkDevices.Any(e => e.Id == id);
        }
    }
}
