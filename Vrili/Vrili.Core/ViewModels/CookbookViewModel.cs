using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using ReactiveUI;
using System.Windows.Input;
using Vrili.Core.Models;
using Vrili.Core.Services;

namespace Vrili.Core.ViewModels
{
    public class CookbookViewModel : MvxViewModel
    {
        public MvxObservableCollection<Recipe> RecipeNames { get; private set; }
            = new MvxObservableCollection<Recipe>();

        private readonly ICommand _openRecipeCommand;
        public ICommand OpenRecipeCommand { get { return _openRecipeCommand; } }

        private readonly IRecipeRepo _recipeRepo;
        private readonly IMvxMessenger _messenger;

        public CookbookViewModel(
            IRecipeRepo recipeRepo,
            IMvxMessenger messenger)
        {
            _recipeRepo = recipeRepo;
            _messenger = messenger;

            _openRecipeCommand = ReactiveCommand.Create<Recipe>((r) => OpenRecipe(r));

            ShowAllRecipes();
        }

        private void ShowAllRecipes()
        {
            RecipeNames.ReplaceWith(_recipeRepo.GetAllRecipes());
        }

        private void OpenRecipe(Recipe recipe)
        {
            _messenger.Publish(new LoadRecipeMessage(this, recipe.Id));
            Close(this);
        }
    }
}
