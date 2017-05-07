using MvvmCross.Core.ViewModels;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using Vrili.Core.Models;
using ReactiveUI;
using Vrili.Core.Services;
using MvvmCross.Plugins.Share;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using System.Collections.Generic;

namespace Vrili.Core.ViewModels
{

    public class RecipeViewModel : MvxViewModel
    {
        private ReactiveRecipe _recipe;
        public IReactiveCollection<CookingActivityViewModel> Activities { get; private set; }

        private readonly ICommand _addActivityCommand;
        public ICommand AddActivityCommand { get { return _addActivityCommand; } }

        private readonly ICommand _startActivityCommand;
        public ICommand StartActivityCommand { get { return _startActivityCommand; } }

        private readonly ICommand _openCommand;
        public ICommand OpenCommand { get { return _openCommand; } }

        private readonly ICommand _saveCommand;
        public ICommand SaveCommand { get { return _saveCommand; } }

        private readonly ICommand _shareCommand;
        public ICommand ShareCommand { get { return _shareCommand; } }
        
        private readonly IRecipeRepo _recipeRepo;
        private readonly IMvxShareTask _shareTask;
        private readonly IMvxMessenger _messenger;

        private bool _isCountingDown;
        private MvxSubscriptionToken _token;

        public bool IsCountingDown
        {
            get { return this._isCountingDown; }
            set { SetProperty(ref _isCountingDown, value); }
        }

        public RecipeViewModel(
              IRecipeRepo recipeRepo
            , IMvxShareTask shareTask
            , IMvxMessenger messenger)
        {
            _recipeRepo = recipeRepo;
            _shareTask = shareTask;
            _messenger = messenger;

            var isIdle = this.WhenAny(x => x.IsCountingDown, x => !x.Value);
            _addActivityCommand = ReactiveCommand.Create(() => AddActivity(), isIdle);
            _startActivityCommand = ReactiveCommand.Create<CookingActivityViewModel>((a) => ClickOnActivity(a));
            _openCommand = ReactiveCommand.Create(() => Open());
            _saveCommand = ReactiveCommand.Create(() => Save());
            _shareCommand = ReactiveCommand.Create(() => Share());

            _token = messenger.SubscribeOnMainThread<LoadRecipeMessage>(OnLoadRecipe);

            int nr = new Random().Next(0, 200);
            LoadRecipe(new Recipe { Name = "Stampot #" + nr  } );
        }

        private void OnLoadRecipe(LoadRecipeMessage message)
        {
            var recipe = _recipeRepo.Get(message.RecipeId);
            LoadRecipe(recipe);
        }

        private void LoadRecipe(Recipe recipe)
        {
            _recipe = new ReactiveRecipe(recipe);
            Activities = _recipe.Activities.CreateDerivedCollection(
                a => new CookingActivityViewModel(a));
        }

        private void Open()
        {
            ShowViewModel<CookbookViewModel>();
        }

        public override void Start()
        {
            IsCountingDown = false;
            base.Start();
        }

        private void Share()
        {
            _shareTask.ShareLink("Baboon cooking", "Checkout my recipe!", "vrili.com/baboon-cooking");
        }

        private void Save()
        {
            _recipeRepo.Save(_recipe.ExtractRecipe());
        }

        private void AddActivity()
        {
            int r = new Random().Next(1, 10);
            var activity = new CookingActivity
            {
                Name = string.Format("Cook the baboon for {0}s. ", r),
                TotalTime = TimeSpan.FromSeconds(r)
            };

            _recipe.Activities.Add(activity);
            
        }

        private void ClickOnActivity(CookingActivityViewModel activity)
        {
            activity.StartCommand.Execute(null);
        }
    }
}
