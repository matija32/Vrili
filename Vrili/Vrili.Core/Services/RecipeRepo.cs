using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vrili.Core.Models;

namespace Vrili.Core.Services
{
    public interface RecipeRepo
    {
        void Save(Recipe recipe);
        Recipe Get();
    }
}
