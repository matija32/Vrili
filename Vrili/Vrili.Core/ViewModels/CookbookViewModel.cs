using MvvmCross.Core.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Vrili.Core.ViewModels
{
    public class CookbookViewModel : MvxViewModel
    {
        private ICommand _setUpRecipeCommand;
        public ICommand SetUpRecipeCommand { get { return _setUpRecipeCommand; } }

        public override void Start()
        {
            _setUpRecipeCommand = ReactiveCommand.Create(() => SetUpRecipe());

            base.Start();
        }

        private void SetUpRecipe()
        {
            ShowViewModel<RecipeViewModel>();
        }
    }
}
