﻿using ReactiveUI;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public CookingActivity()
        {
            this
                .WhenAnyValue(x => x.RemainingTime)
                .Select(time => time > TimeSpan.Zero)
                .ToProperty(this, x => x.IsOngoing, out _isOngoing);

            RemainingTime = TimeSpan.Zero;
        }
        
        private TimeSpan _remainingTime;
        public TimeSpan RemainingTime
        {
            get { return this._remainingTime; }
            private set { this.RaiseAndSetIfChanged(ref _remainingTime, value); }
        }

        ObservableAsPropertyHelper<bool> _isOngoing;
        public bool IsOngoing
        {
            get { return _isOngoing.Value; }
        } 

        public void CountDown()
        {
            RemainingTime = TotalTime;
            var period = TimeSpan.FromMilliseconds(100);

            var ticker = Observable
                .Interval(period)
                .Select(_ => RemainingTime - period)
                .TakeWhile(newTime => newTime > TimeSpan.Zero)
                .Subscribe(
                    onNext: newTime => RemainingTime = newTime,
                    onCompleted: () => RemainingTime = TimeSpan.Zero
                );
        }
    }
}
