using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NDDevicePoller.Data;
using NDDevicePoller.Models;
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
            
            try
            {
                var networkDevices = _dBContext.NetworkDevices.ToList();
                var monitorTypes = _dBContext.MonitorTypes.ToList();

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


        public IActionResult Settings()
        {
            if (_dBContext.CrossPlatformSettings.Any()) {

                var settings = _dBContext.CrossPlatformSettings.First();
                //Utils util = new Utils();
                //settings.SNMPAuth = util.DecryptHASHPassword(settings.SNMPAuth);
                //settings.SNMPPriv = util.DecryptHASHPassword(settings.SNMPPriv);

                CrossServiceSettings newSettings = new CrossServiceSettings
                {
                    
                    ID = settings.ID,
                    PollInterval = settings.PollInterval / 1000,
                    SNMPUsername = settings.SNMPUsername
                    //SNMPAuth = settings.SNMPAuth,
                    //SNMPPriv = settings.SNMPPriv
                };
                return View(settings);
            }
            else
            {
                //Database could be empty:
                NDDevicePoller.Data.CrossServiceSettings settings = new NDDevicePoller.Data.CrossServiceSettings
                {
                    PollInterval = 60000
                };
                _dBContext.CrossPlatformSettings.Add(settings);
                _dBContext.SaveChanges();
                NDDevicePoller.Data.CrossServiceSettings newSettings = new NDDevicePoller.Data.CrossServiceSettings
                {
                    ID= _dBContext.CrossPlatformSettings.First().ID,
                    PollInterval = _dBContext.CrossPlatformSettings.First().PollInterval / 1000,
                    //SNMPUsername = "",
                    //SNMPAuth = "",
                    //SNMPPriv = ""
                };
                return View(newSettings);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Settings(int id, [Bind("ID,PollInterval,SNMPUsername,SNMPAuth,SNMPPriv")] NDDevicePoller.Data.CrossServiceSettings settings)
        {
            if (id != settings.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var storedSettings = _dBContext.CrossPlatformSettings.First();

                    if (settings.PollInterval != storedSettings.PollInterval)
                    {
                        settings.PollInterval = settings.PollInterval * 1000;
                    }


                    Utils util = new Utils();
                    if (settings.SNMPAuth != " ")
                    {
                        string saltedAuthPassword = util.HASHPassword(settings.SNMPAuth);
                        settings.SNMPAuth = saltedAuthPassword;
                    }
                    else
                    {
                        settings.SNMPAuth = storedSettings.SNMPAuth;
                    }
                    if (settings.SNMPPriv != " ")
                    {
                        string saltedPrivPassword = util.HASHPassword(settings.SNMPPriv);
                        settings.SNMPPriv = saltedPrivPassword;
                    }
                    else
                    {
                        settings.SNMPPriv = storedSettings.SNMPPriv;
                    }
                    

                    _dBContext.Update(settings);
                    await _dBContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SettingsExists(settings.ID))
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
            return View(settings);
        }

        private bool SettingsExists(int id)
        {
            return _dBContext.CrossPlatformSettings.Any(e => e.ID == id);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}