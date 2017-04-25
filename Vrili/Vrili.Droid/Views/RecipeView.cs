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
    }
}
