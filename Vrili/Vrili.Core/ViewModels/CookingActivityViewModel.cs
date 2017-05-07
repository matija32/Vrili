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
            set { SetProperty(ref _isCountingDown, value); }
        }

        private string _remainingTime;
        public string RemainingTime
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
            
            _model.WhenAnyValue(x => x.RemainingTime)
                  .Subscribe(onNext: t => this.RemainingTime = t.ToString(@"mm\:ss"));

            var isIdle = _model.IsOngoing.Select(x => !x);
            var isTicking = _model.IsOngoing;
            _startCommand = ReactiveCommand.Create(() => StartCooking(), isIdle);
            _pauseCommand = ReactiveCommand.Create(() => PauseCooking());
            _stopCommand = ReactiveCommand.Create(() => StopCooking());
            _resetCommand = ReactiveCommand.Create(() => StopCooking());
        }

        private void StartCooking()
        {
            _model.CountDown();
        }

        private void PauseCooking()
        {
            _model.Pause();
        }

        private void StopCooking()
        {
            _model.Stop();
        }

        private void ResetTimer()
        {
            _model.Reset();
        }

    }
}
