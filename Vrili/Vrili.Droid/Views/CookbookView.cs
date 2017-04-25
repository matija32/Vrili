using Android.App;
using Android.OS;
using Android.Views;
using MvvmCross.Droid.Views;
using System;
using Vrili.Core.ViewModels;

namespace Vrili.Droid.Views
{
    [Activity(Label = "CookbookView")]
    public class CookbookView : MvxActivity<CookbookViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.CookbookView);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.RecipeMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }
    }
}
