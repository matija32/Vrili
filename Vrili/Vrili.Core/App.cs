using MvvmCross.Platform.IoC;

namespace Vrili.Core
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .InNamespace("Vrili.Core.Services")
                .AsInterfaces()
                .RegisterAsLazySingleton();
            
            RegisterAppStart<ViewModels.RecipeViewModel>();
        }
    }
}
