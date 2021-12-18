using Microsoft.AspNetCore.Mvc;
using NetworkDashboard.Data;
using NetworkDashboard.Models;
using System.Diagnostics;

namespace NetworkDashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DBContext _dBContext;

        public HomeController(ILogger<HomeController> logger, DBContext dBContext)
        {
            _logger = logger;
            _dBContext = dBContext;
        }

        public IActionResult Index()
        {
            Utils utils = new Utils();
            try
            {
                var networkDevices = _dBContext.NetworkDevices.ToList();
                var monitorTypes = _dBContext.MonitorTypes.ToList();

                foreach (var networkDevice in networkDevices)
                {
                    if (monitorTypes.Where(m => m.ping == false).Where(m => m.TypeTitle == networkDevice.DeviceType).Count() >= 1)//nonPing.Contains(monitorTypes.Find(m=>m.TypeTitle == networkDevice.DeviceType)))
                    {
                        networkDevice.IsConnected = utils.HTTPCheckHostAsync(networkDevice).Result;
                    }
                    else
                    {
                        if (!utils.PingHost(networkDevice.IPAddress))
                        {
                            networkDevice.IsConnected = utils.PingHost(networkDevice.DeviceName);
                        }
                        else
                        {
                            networkDevice.IsConnected = true;
                        }
                    }

                }
                _dBContext.SaveChanges();

                DashboardDisplay dash = new DashboardDisplay()
                {
                    TypesOfDevices = monitorTypes.ToList(),
                    Devices = networkDevices.ToList()
                };
                return View(dash);
            }
            catch (Exception ex)
            {
                //spit out the error to the event log?
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        public IActionResult Refresh()
        {
            Utils utils = new Utils();
            var networkDevices = _dBContext.NetworkDevices.ToList();
            foreach (var item in networkDevices)
            {
                if(utils.PingHost(item.IPAddress) == false)
                {
                    item.IsConnected = utils.PingHost(item.DeviceName);
                }
                else
                {
                    item.IsConnected = true;
                }
                _dBContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}