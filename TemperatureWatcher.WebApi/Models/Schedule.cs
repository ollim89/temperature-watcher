using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureWatcher.WebApi.Models
{
    public class Schedule
    {
        public int Hour { get; set; }
        public int Minute { get; set; }
        public bool ActiveState { get; set; }
    }
}
