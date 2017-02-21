using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrili.Models
{
    public class CookingActivity : ReactiveObject
    {
        public string Name { get; set; }
        public TimeSpan TotalTime { get; set; }

        private TimeSpan _remainingTime;
        public TimeSpan RemainingTime
        {
            get { return this._remainingTime; }
            set { this.RaiseAndSetIfChanged(ref _remainingTime, value); }
        }

        public void CountDown()
        {
            RemainingTime = TotalTime;
            var period = TimeSpan.FromMilliseconds(100);

            Observable
                .Interval(period)
                .TakeWhile(_ => RemainingTime > period)
                .Subscribe(
                    onNext: _ => RemainingTime -= period,
                    onCompleted: () => RemainingTime = TimeSpan.Zero
                ); ;
        }
    }

    public class Cue
    {
        public string Action { get; set; }
        public TimeSpan TimeLeft { get; set; }
    }
}
