using MvvmCross.Plugins.Sqlite;
using SQLite;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teddy.MvvmCross.Plugins.SimpleAudioPlayer;
using Vrili.Core.Models;

namespace Vrili.Core.Services
{
    public class AlarmBell : IAlarmBell
    {
        private readonly string _soundFilePath = "alarm_clock.mp3";
        private IMvxSimpleAudioPlayer _audioPlayer;

        public AlarmBell(IMvxSimpleAudioPlayer audioPlayer)
        {
            _audioPlayer = audioPlayer;
        }
 
        public void Start()
        {
            _audioPlayer.SetUpLooping();
            _audioPlayer.Open(_soundFilePath);
            _audioPlayer.Volume = 1;
            _audioPlayer.Play();
        }

        public void Stop()
        {
            _audioPlayer.Stop();
            _audioPlayer.TearDownLooping();
        }
    }
}
