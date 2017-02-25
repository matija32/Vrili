using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrili.Core.Models
{
    public class Cookbook
    {
        public IDictionary<string, Recipe> recipesByName { get; } 
            = new Dictionary<string, Recipe>();
    }
}
