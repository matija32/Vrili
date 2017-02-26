using MvvmCross.Core.ViewModels;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using Vrili.Core.Models;
using ReactiveUI;

namespace Vrili.Core.ViewModels
{
    public class RecipeViewModel : MvxViewModel
    {
        public MvxObservableCollection<CookingActivity> Activities { get; set; } 
            = new MvxObservableCollection<CookingActivity>();

        private readonly ICommand _addActivityCommand;
        public ICommand AddActivityCommand { get { return _addActivityCommand; } }

        private readonly ICommand _startCookingCommand;
        public ICommand StartCookingCommand { get { return _startCookingCommand; } }

        public RecipeViewModel()
        {
            IsCountingDown = false;

            var isIdle = this.WhenAny(x => x.IsCountingDown, x => !x.Value);

            _addActivityCommand = ReactiveCommand.Create(() => AddActivity(), isIdle);
            _startCookingCommand = ReactiveCommand.Create(() => StartCooking(), isIdle);
        }

        private bool _isCountingDown;
        public bool IsCountingDown
        {
            get { return this._isCountingDown; }
            set { SetProperty(ref _isCountingDown, value); }
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
