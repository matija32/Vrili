using Android.App;
using Android.OS;
using MvvmCross.Droid.Views;
using System;
using Vrili.Core.ViewModels;
using Android.Views;
using Android.Content;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using MvvmCross.Core.ViewModels;
using System.Collections.Generic;
using Android.Content.PM;

namespace Vrili.Droid.Views
{

    [Activity(Label = "Recipe", LaunchMode = LaunchMode.SingleTop)]
    public class RecipeView : MvxActivity<RecipeViewModel>
    {

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            int count = intent.Extras.KeySet().Count;

            foreach(string s in intent.Extras.KeySet())
            {
                var x = intent.Extras.Get(s);
                x.ToString();
            }

            ViewModel.AddActivityCommand.Execute(null);

        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.RecipeView);

            var request = MvxViewModelRequest<RecipeViewModel>.GetDefaultRequest();
            var translator = Mvx.Resolve<IMvxAndroidViewModelRequestTranslator>();

            var intent = translator.GetIntentFor(request);
            intent.PutExtra("life34", "halflife");


            var pending = PendingIntent.GetActivity(
                this, 0, intent, 0);

            Notification.Action alarm_action =
                new Notification.Action(Resource.Drawable.ic_add, "Silence alarm", pending);

            var request2 = MvxViewModelRequest<RecipeViewModel>.GetDefaultRequest();
            var intent2 = translator.GetIntentFor(request2);
            intent2.PutExtra("life-in-another-intent", "counter-strike");


            var pending2 = PendingIntent.GetActivity(
                this, 1, intent2, 0);

            Notification.Action action2 =
                new Notification.Action(Resource.Drawable.ic_save, "Extend", pending2);
            
            Notification.Builder builder =
                new Notification.Builder(this.ApplicationContext)
                .SetSmallIcon(Resource.Drawable.ic_pause)
                .SetContentTitle("My notification")
                .SetContentText("Hello World!")
                .AddAction(alarm_action)
                .AddAction(action2)
//                .SetContentIntent(pending2)
//                .SetAutoCancel(true) // clears the notification on click
                .SetVisibility(NotificationVisibility.Public); // makes the notification visible on lock screen

            Notification notification = builder.Build();
            var notificationManager = (NotificationManager)
                this.ApplicationContext.GetSystemService(Context.NotificationService);
            notificationManager.Notify(0, builder.Build());

            // https://developer.xamarin.com/guides/android/application_fundamentals/notifications/local_notifications_in_android/

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
