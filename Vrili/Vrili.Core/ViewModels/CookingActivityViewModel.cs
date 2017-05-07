using MvvmCross.Core.ViewModels;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using Vrili.Core.Models;
using ReactiveUI;
using Vrili.Core.Services;
using MvvmCross.Platform;
using System.Reactive;

namespace Vrili.Core.ViewModels
{

    public class CookingActivityViewModel : MvxViewModel
    {
        private CookingActivity _model;

        private readonly ICommand _startCommand;
        public ICommand StartCommand { get { return _startCommand; } }

        private readonly ICommand _pauseCommand;
        public ICommand PauseCommand { get { return _pauseCommand; } }

        private readonly ICommand _stopCommand;
        public ICommand StopCommand { get { return _stopCommand; } }

        private readonly ICommand _resetCommand;
        public ICommand ResetCommand { get { return _resetCommand; } }


        private bool _isCountingDown;
        public bool IsCountingDown
        {
            get { return this._isCountingDown; }
            private set { SetProperty(ref _isCountingDown, value); }
        }

        private bool _isOverdue;
        public bool IsOverdue
        {
            get { return this._isOverdue; }
            private set { SetProperty(ref _isOverdue, value); }
        }

        private string _remainingTime;
        public string RemainingTime
        {
            get { return this._remainingTime; }
            private set { SetProperty(ref _remainingTime, value); }
        }

        public string Name
        {
            get { return _model.Name; }
        }

        public CookingActivityViewModel(
              CookingActivity activity)
        {
            _model = activity;
            
            _model.WhenAnyValue(x => x.RemainingTime)
                  .Subscribe(onNext: t => UpdateRemainingTime(t));

            _model.WhenAnyValue(x => x.RemainingTime)
                  .Subscribe(onNext : t => IsOverdue = t > TimeSpan.Zero);

            var isActive = _model.WhenAnyValue(x => x.IsActive);
            var isNotActive = isActive.Select(x => !x);

            var isTicking = _model.WhenAnyValue(x => x.IsActive);
            var isNotTicking = isTicking.Select(x => !x);

            _startCommand = ReactiveCommand.Create(() => StartCooking(), isNotTicking);
            _pauseCommand = ReactiveCommand.Create(() => PauseCooking(), isTicking);
            _stopCommand = ReactiveCommand.Create(() => FinishCooking());
            _resetCommand = ReactiveCommand.Create(() => FinishCooking());

            isTicking.CombineLatest(isActive, (a, b) => a && b)
                .Subscribe(onNext : value => IsCountingDown = value);

            ResetTimer();

        }

        private void UpdateRemainingTime(TimeSpan t)
        {
            this.RemainingTime = (t < TimeSpan.Zero ? "-" : "") + t.ToString(@"mm\:ss");
        }

        private void StartCooking()
        {
            _model.Start();
        }

        private void PauseCooking()
        {
            _model.Pause();
        }

        private void FinishCooking()
        {
            _model.Finish();
        }

        private void ResetTimer()
        {
            _model.ResetClock();
        }

    }
}
