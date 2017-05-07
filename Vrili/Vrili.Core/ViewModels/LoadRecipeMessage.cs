using MvvmCross.Plugins.Messenger;

namespace Vrili.Core.ViewModels
{
    public class LoadRecipeMessage
        : MvxMessage
    {
        public LoadRecipeMessage(object sender, int recipeId)
            : base(sender)
        {
            RecipeId = recipeId;
        }

        public int RecipeId { get; private set; }
    }
}
