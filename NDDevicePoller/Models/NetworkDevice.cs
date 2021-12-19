using System.ComponentModel.DataAnnotations;

namespace NDDevicePoller.Models
{
    public class NetworkDevice
    {
        public int Id { get; set; }
        [Display(Name = "Hostname")]
        public string DeviceName { get; set; }
        [Display(Name ="Display Name")]
        public string DisplayName { get; set; }
        [Display(Name = "Device Type")]
        public string DeviceType { get; set; }
        [Display(Name = "Device Description")]
        public string DeviceDescription { get; set; }
        [Display(Name = "IP Address")]
        public string IPAddress { get; set; }
        [Display(Name = "Is Connected?")]
        public bool IsConnected { get; set; }
    }
}
