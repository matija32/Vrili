using Android.Content;
using MvvmCross.Droid.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using System.Reflection;
using System.Collections.Generic;

namespace Vrili.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override IEnumerable<Assembly> AndroidViewAssemblies
        {
            get
            {
                var assemblies = base.AndroidViewAssemblies;
                List<Assembly> extendedAssemblies = new List<Assembly>(assemblies);
                extendedAssemblies.Add(typeof(Android.Support.Design.Widget.FloatingActionButton).Assembly);
                extendedAssemblies.Add(typeof(Android.Support.Design.Widget.CoordinatorLayout).Assembly);
                return extendedAssemblies;
            }
        }
    }
}
