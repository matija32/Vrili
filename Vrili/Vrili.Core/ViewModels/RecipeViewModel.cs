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
        private Recipe _model;

        private IReactiveCollection<CookingActivityViewModel> _activities;
        public IReactiveCollection<CookingActivityViewModel> Activities
        {
            get { return this._activities; }
            private set { SetProperty(ref _activities, value); }
        }
        
        private readonly ICommand _addActivityCommand;
        public ICommand AddActivityCommand { get { return _addActivityCommand; } }

        private readonly ICommand _resetTimersCommand;
        public ICommand ResetTimersCommand { get { return _resetTimersCommand; } }

        private readonly ICommand _openCommand;
        public ICommand OpenCommand { get { return _openCommand; } }

        private readonly ICommand _saveCommand;
        public ICommand SaveCommand { get { return _saveCommand; } }

        private readonly ICommand _shareCommand;
        public ICommand ShareCommand { get { return _shareCommand; } }
        
        private readonly IRecipeRepo _recipeRepo;
        private readonly IMvxShareTask _shareTask;
        private readonly IMvxMessenger _messenger;

        private MvxSubscriptionToken _token;

        private bool _isCountingDown;
        public bool IsCountingDown
        {
            get { return this._isCountingDown; }
            private set { SetProperty(ref _isCountingDown, value); }
        }

        private string _name;
        public string Name
        {
            get { return this._name; }
            private set
            {
                SetProperty(ref _name, value);
                _model.Name = value;
            }
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
            _addActivityCommand = ReactiveCommand.Create(() => AddActivity());
            _openCommand = ReactiveCommand.Create(() => Open());
            _resetTimersCommand = ReactiveCommand.Create(() => ResetTimers());
            _saveCommand = ReactiveCommand.Create(() => Save());
            _shareCommand = ReactiveCommand.Create(() => Share());

            _token = messenger.SubscribeOnMainThread<LoadRecipeMessage>(OnLoadRecipe);

            int nr = new Random().Next(0, 200);
            LoadRecipe(new Recipe { Name = "Stampot #" + nr  } );
        }

        private void OnLoadRecipe(LoadRecipeMessage message)
        {
            LoadRecipe(_recipeRepo.Get(message.RecipeId));
        }

        private void LoadRecipe(Recipe recipe)
        {
            _model = recipe;

            Name = _model.Name;
            Activities = _model.Activities.CreateDerivedCollection(
                a => new CookingActivityViewModel(a));
        }

        private void Open()
        {
            ShowViewModel<CookbookViewModel>();
        }

        private void ResetTimers()
        {
            throw new NotImplementedException();
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
            _recipeRepo.Save(_model);
        }

        private void AddActivity()
        {
            int r = new Random().Next(1, 10);
            var activity = new CookingActivity
            {
                Name = string.Format("Cook the baboon for {0}s. ", r),
                TotalTime = TimeSpan.FromSeconds(r)
            };

            _model.Activities.Add(activity);
            
        }
    }
}
