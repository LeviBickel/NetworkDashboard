using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace NDDevicePoller.Data
{
    public class CrossServiceSettings
    {
        public int ID { get; set; }
        [Display(Name = "Poll Interval")]
        public int PollInterval { get; set; }
        [Display(Name = "SNMP Username")]
        public string SNMPUsername { get; set; }
        [Display(Name = "SNMP Authentication Password")]
        public string SNMPAuth { get; set; }
        [Display(Name = "SNMP Privacy Password")]
        public string SNMPPriv { get; set; }

    }
}
