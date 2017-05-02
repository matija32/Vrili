using MvvmCross.Plugins.Messenger;

namespace Vrili.Core.ViewModels
{
    public class ActiveRecipeMessage
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
