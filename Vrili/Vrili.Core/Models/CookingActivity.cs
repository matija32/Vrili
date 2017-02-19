using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrili.Models
{
    public class CookingActivity
    {
        public string Name { get; set; }
        public TimeSpan TotalTime { get; set; }
        public TimeSpan RemainingTime { get; set; }
    }

    public class Cue
    {
        public string Action { get; set; }
        public TimeSpan TimeLeft { get; set; }
    }
}
