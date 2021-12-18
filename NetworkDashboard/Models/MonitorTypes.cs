using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace NetworkDashboard.Models
{
public class MonitorTypes
{
        public int Id { get; set; }
        [Display(Name = "Type Title")]
        public string TypeTitle { get; set; }
        [Display(Name = "Type Description")]
        public string TypeDescription { get; set; }
        [Display(Name ="Use Ping Check?")]
        public bool ping { get; set; }
        
    }
}
