using Android.App;
using Android.OS;
using MvvmCross.Droid.Views;
using Vrili.Core.ViewModels;

namespace Vrili.Droid.Views
{
    [Activity(Label = "View for MainViewModel")]
    public class RecipeView : MvxActivity<RecipeViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.RecipeView);
        }
    }
}
