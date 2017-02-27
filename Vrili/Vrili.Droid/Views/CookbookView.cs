using Android.App;
using Android.OS;
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
    }
}
