using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Vrili.Core.Services;

namespace Vrili.Core.ViewModels
{
    public class CookbookViewModel : MvxViewModel
    {
        private readonly ICommand _openRecipeCommand;
        public ICommand OpenRecipeCommand { get { return _openRecipeCommand; } }

        public CookbookViewModel()
        {
            _openRecipeCommand = ReactiveCommand.Create(() => OpenRecipe());
        }

        private void SetUpRecipe()
        {
            ShowViewModel<RecipeViewModel>();
        }

        private void OpenRecipe()
        {
            ShowViewModel<RecipeViewModel>(new { loadRecipe = true });
        }
    }
}
