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


namespace Vrili.Core.ViewModels
{

    public class RecipeViewModel : MvxViewModel
    {
        public MvxObservableCollection<CookingActivity> Activities { get; private set; }
            = new MvxObservableCollection<CookingActivity>();

        private readonly ICommand _addActivityCommand;
        public ICommand AddActivityCommand { get { return _addActivityCommand; } }

        private readonly ICommand _startCookingCommand;
        public ICommand StartCookingCommand { get { return _startCookingCommand; } }

        private readonly ICommand _openCommand;
        public ICommand OpenCommand { get { return _openCommand; } }

        private readonly ICommand _saveCommand;
        public ICommand SaveCommand { get { return _saveCommand; } }

        private readonly ICommand _shareCommand;
        public ICommand ShareCommand { get { return _shareCommand; } }

        private int baboonCount = 0;

        private IRecipeRepo _recipeRepo;
        private IAlarmBell _alarmBell;
        private IMvxShareTask _shareTask;

        private bool _isCountingDown;
        public bool IsCountingDown
        {
            get { return this._isCountingDown; }
            set { SetProperty(ref _isCountingDown, value); }
        }

        public RecipeViewModel(
              IRecipeRepo recipeRepo
            , IAlarmBell audioPlayer
            , IMvxShareTask shareTask)
        {
            _recipeRepo = recipeRepo;
            _alarmBell = audioPlayer;
            _shareTask = shareTask;

            var isIdle = this.WhenAny(x => x.IsCountingDown, x => !x.Value);
            _addActivityCommand = ReactiveCommand.Create(() => AddActivity(), isIdle);
            _startCookingCommand = ReactiveCommand.Create(() => StartCooking(), isIdle);
            _openCommand = ReactiveCommand.Create(() => Open());
            _saveCommand = ReactiveCommand.Create(() => Save());
            _shareCommand = ReactiveCommand.Create(() => Share());
        }


        public void Init(bool loadRecipe, int recipeId)
        {
            if (loadRecipe)
            {
                var recipe = _recipeRepo.Get(recipeId);
                Activities.AddRange(recipe.Activities);
            }
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
            int r = new Random().Next(0, 200);
            _recipeRepo.Save(new Recipe
            {
                Name = "Baboon cooking " + r,
                Activities = this.Activities.ToList()
            });
        }

        private void AddActivity()
        {
            var activity = new CookingActivity
            {
                Name = string.Format("Cook the baboon for {0}s. Time left: ", baboonCount),
                TotalTime = TimeSpan.FromSeconds(baboonCount)
            };
            baboonCount++;

            activity
                .WhenAny(a => a.IsOngoing, x => x)
                .Subscribe(onNext: _ => IsCountingDown = Activities.Any(a => a.IsOngoing));
            
            Activities.Add(activity);
        }

        private void StartCooking()
        {
            _alarmBell.RingOnce();
            Activities.ToObservable().Subscribe(a => a.CountDown());
        } 
    }
}
