using MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrili.Core.ViewModels
{
    class ActiveRecipeMessage
        : MvxMessage
    {
        public ActiveRecipeMessage(object sender, int recipeId)
            : base(sender)
        {
            RecipeId = recipeId;
        }

        public int RecipeId { get; private set; }
    }
}
