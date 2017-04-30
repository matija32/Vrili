using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private IRecipeRepo _recipeRepo;


        public CookbookViewModel(
            IRecipeRepo recipeRepo)
        {
            _recipeRepo = recipeRepo;

            _openRecipeCommand = ReactiveCommand.Create<Recipe>((r) => OpenRecipe(r));

            ShowAllRecipes();
        }

        private void ShowAllRecipes()
        {
            RecipeNames.ReplaceWith(_recipeRepo.GetAllRecipes());
        }

        private void OpenRecipe(Recipe recipe)
        {
            //Close(this);
            ShowViewModel<RecipeViewModel>(
                new {
                    loadRecipe = true,
                    recipeId = recipe.Id
                });
        }
    }
}
