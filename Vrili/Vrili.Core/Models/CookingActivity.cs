using ReactiveUI;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace Vrili.Core.Models
{
    [Table("CookingActivity")]
    public class CookingActivity : ReactiveObject
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(Recipe))]
        public int RecipeId { get; set; }
        public string Name { get; set; }
        public TimeSpan TotalTime { get; set; }

        private bool _isActive;
        public bool IsActive
        {
            get { return this._isActive; }
            private set { this.RaiseAndSetIfChanged(ref _isActive, value); }
        }

        private bool _isTicking;
        public bool IsTicking
        {
            get { return this._isTicking; }
            private set { this.RaiseAndSetIfChanged(ref _isTicking, value); }
        }

        
        private TimeSpan _remainingTime;

        [Ignore]
        public TimeSpan RemainingTime
        {
            get { return this._remainingTime; }
            private set { this.RaiseAndSetIfChanged(ref _remainingTime, value); }
        }

        public void Start()
        {
            IsActive = true;
            IsTicking = true;

            var period = TimeSpan.FromMilliseconds(500);
            var ticker = Observable
                .Interval(period)
                .TakeWhile(_ => IsTicking)
                .Select(_ => RemainingTime - period)
                .Subscribe(
                    onNext: newTime => RemainingTime = newTime
                );
        }

        public void Pause()
        {
            IsActive = false;
        }

        public void Finish()
        {
            Pause();
            ResetClock();
        }

        public void ResetClock()
        {
            RemainingTime = TotalTime;
        }
    }
}
