using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrili.Core.Models
{
    public class Recipe
    {
        public string Name { get; set; }
        public IList<CookingActivity> Activities { get; } = new List<CookingActivity>();
    }
}
