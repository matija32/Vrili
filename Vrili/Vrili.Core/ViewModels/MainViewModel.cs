using MvvmCross.Core.ViewModels;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using Vrili.Models;

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
            _addActivity = ReactiveCommand.Create(() => AddActivity());
            _startCooking = ReactiveCommand.Create(() => StartCooking());
            base.Start();
        }

        private int count = 0;

        private void AddActivity()
        {
            Activities.Add(new CookingActivity
            {
                Name = string.Format("Cook the baboon for {0}s. Time left: ", count),
                TotalTime = TimeSpan.FromSeconds(count),
                RemainingTime = TimeSpan.FromSeconds(0)
            });
            count++;
        }

        private void StartCooking()
        {
            Activities.ToObservable().Subscribe(a => a.CountDown());
        }
    }
}
