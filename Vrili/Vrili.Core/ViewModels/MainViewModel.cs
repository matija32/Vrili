using MvvmCross.Core.ViewModels;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using Vrili.Models;
using ReactiveUI;

namespace Vrili.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        public MvxObservableCollection<CookingActivity> Activities { get; set; } 
            = new MvxObservableCollection<CookingActivity>();

        private ICommand _addActivity;
        public ICommand AddActivityCommand { get { return _addActivity; } }

        private ICommand _startCooking;
        public ICommand StartCookingCommand { get { return _startCooking; } }

        public override void Start()
        {
            IsCountingDown = false;
            var canAdd = this.WhenAny(x => x.IsCountingDown, x => !x.Value);

            _addActivity = ReactiveCommand.Create(() => AddActivity(), canAdd);
            _startCooking = ReactiveCommand.Create(() => StartCooking());
            
            base.Start();
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
