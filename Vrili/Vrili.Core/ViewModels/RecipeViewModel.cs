using MvvmCross.Core.ViewModels;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using Vrili.Core.Models;
using ReactiveUI;
using Vrili.Core.Services;
using System.Collections.Generic;

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

        private RecipeRepo _recipeRepo;

        private bool _isCountingDown;
        public bool IsCountingDown
        {
            get { return this._isCountingDown; }
            set { SetProperty(ref _isCountingDown, value); }
        }

        public RecipeViewModel(RecipeRepo recipeRepo)
        {
            _recipeRepo = recipeRepo;

            var isIdle = this.WhenAny(x => x.IsCountingDown, x => !x.Value);
            _addActivityCommand = ReactiveCommand.Create(() => AddActivity(), isIdle);
            _startCookingCommand = ReactiveCommand.Create(() => StartCooking(), isIdle);
            _saveCommand = ReactiveCommand.Create(() => Save());
        }

        public void Init(int recipeId)
        {
            var recipe = _recipeRepo.Get(recipeId);
            Activities.AddRange(recipe.Activities);
        }

        public override void Start()
        {
            IsCountingDown = false;
            base.Start();
        }

        private void Save()
        {
            _recipeRepo.Save(new Recipe
            {
                Name = "Baboon cooking",
                Activities = this.Activities.ToList()
            });
        }

        private int baboonCount = 0;
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
            Activities.ToObservable().Subscribe(a => a.CountDown());
        }
    }
}
