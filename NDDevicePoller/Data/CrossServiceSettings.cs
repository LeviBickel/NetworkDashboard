using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace NDDevicePoller.Data
{
    public class CrossServiceSettings
    {
        public int ID { get; set; }
        [Display(Name = "Poll Interval")]
        public int PollInterval { get; set; }
    }
}
