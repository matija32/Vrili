using MvvmCross.Core.ViewModels;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using Vrili.Core.Models;
using ReactiveUI;
using Vrili.Core.Services;
using System.Collections.Generic;
using MvvmCross.Plugins.Share;
using MvvmCross.Platform;
using Teddy.MvvmCross.Plugins.SimpleAudioPlayer;


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

        private readonly ICommand _saveCommand;
        public ICommand SaveCommand { get { return _saveCommand; } }

        private readonly ICommand _shareCommand;
        public ICommand ShareCommand { get { return _shareCommand; } }

        private int baboonCount = 0;
        private IRecipeRepo _recipeRepo;
        private IMvxSimpleAudioPlayer _audioPlayer;

        private bool _isCountingDown;
        public bool IsCountingDown
        {
            get { return this._isCountingDown; }
            set { SetProperty(ref _isCountingDown, value); }
        }

        public RecipeViewModel(
              IRecipeRepo recipeRepo
            , IMvxSimpleAudioPlayer audioPlayer)
        {
            _recipeRepo = recipeRepo;
            _audioPlayer = audioPlayer;

            var isIdle = this.WhenAny(x => x.IsCountingDown, x => !x.Value);
            _addActivityCommand = ReactiveCommand.Create(() => AddActivity(), isIdle);
            _startCookingCommand = ReactiveCommand.Create(() => StartCooking(), isIdle);
            _saveCommand = ReactiveCommand.Create(() => Save());
            _shareCommand = ReactiveCommand.Create(() => Share());
        }

        public void Init(bool loadRecipe)
        {
            if (loadRecipe)
            {
                var recipeId = _recipeRepo.FindRecipeWithActivities();
                var recipe = _recipeRepo.Get(recipeId);
                Activities.AddRange(recipe.Activities);
            }
        }

        public override void Start()
        {
            IsCountingDown = false;
            base.Start();
        }

        private void Share()
        {
            var service = Mvx.Resolve<IMvxShareTask>();
            service.ShareLink("Baboon cooking", "Checkout my recipe!", "vrili.com/baboon-cooking");
        }

        private void Save()
        {
            _recipeRepo.Save(new Recipe
            {
                Name = "Baboon cooking",
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
            _audioPlayer.Open("alarm_clock.mp3");
            _audioPlayer.Volume = 1;
            _audioPlayer.Play();
            Activities.ToObservable().Subscribe(a => a.CountDown());
        } 
    }
}
