using Android.App;
using Android.OS;
using MvvmCross.Droid.Views;
using System;
using Vrili.Core.ViewModels;
using Android.Views;

namespace Vrili.Droid.Views
{
    [Activity(Label = "Recipe")]
    public class RecipeView : MvxActivity<RecipeViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.RecipeView);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.RecipeMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.recipe_menu_open:

                    ViewModel.OpenCommand.Execute(null);
                    return true;

                case Resource.Id.recipe_menu_save:

                    ViewModel.SaveCommand.Execute(null);
                    return true;

                case Resource.Id.recipe_menu_share:

                    ViewModel.ShareCommand.Execute(null);
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}
