using MvvmCross.Core.ViewModels;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using Vrili.Core.Models;
using ReactiveUI;
using Vrili.Core.Services;
using MvvmCross.Platform;

namespace Vrili.Core.ViewModels
{

    public class CookingActivityViewModel : MvxViewModel
    {
        private CookingActivity _model;

        private readonly ICommand _startCommand;
        public ICommand StartCommand { get { return _startCommand; } }

        private bool _isCountingDown;
        public bool IsCountingDown
        {
            get { return this._isCountingDown; }
            set { SetProperty(ref _isCountingDown, value); }
        }

        private TimeSpan _remainingTime;
        public TimeSpan RemainingTime
        {
            get { return this._remainingTime; }
            set { SetProperty(ref _remainingTime, value); }
        }

        public string Name
        {
            get { return _model.Name; }
        }

        public CookingActivityViewModel(
              CookingActivity activity)
        {
            _model = activity;
            
            var isIdle = this.WhenAny(x => x.IsCountingDown, x => !x.Value);
            _model.WhenAnyValue(x => x.RemainingTime)
                  .Subscribe(onNext: t => this.RemainingTime = t);

            _startCommand = ReactiveCommand.Create(() => StartCooking());
        }

        private void StartCooking()
        {
            IsCountingDown = false;
            _model.CountDown();
        }
    }
}
