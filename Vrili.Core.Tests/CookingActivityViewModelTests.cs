using MvvmCross.Test.Core;
using NUnit.Framework;
using Vrili.Core.ViewModels;
using Vrili.Core.Models;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using System;
using Microsoft.Reactive.Testing;

namespace Vrili.Core.Tests
{
    [TestFixture]
    public class CookingActivityViewModelTests : MvxIoCSupportingTest
    {
        private IFixture _fixture;
        
        [SetUp]
        public void SetUp()
        {
            ClearAll();

            _fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        [Test]
        public void UserCanStartActivityIfItsNotStarted()
        {
            var vm = _fixture.Create<CookingActivityViewModel>();

            Assert.IsTrue(vm.StartCommand.CanExecute(null));
        }

        [Test]
        public void UserCannotStartActivityIfItsAlreadyStarted()
        {
            var vm = _fixture.Create<CookingActivityViewModel>();

            vm.StartCommand.Execute(null);

            Assert.IsFalse(vm.StartCommand.CanExecute(null));
        }

        [Test]
        public void ActivityNameIsShown()
        {
            var activity = _fixture.Create<CookingActivity>();

            Assert.AreEqual(activity.Name, new CookingActivityViewModel(activity).Name);
        }

        [Test]
        public void InitialRemainingTimeIsShown()
        {
            var activity = _fixture.Build<CookingActivity>()
                                   .With(a=> a.TotalTime, TimeSpan.FromSeconds(5))
                                   .Create();

            StringAssert.AreEqualIgnoringCase("00:05", new CookingActivityViewModel(activity).RemainingTime);
        }

        [Test]
        public void InitialNegativeRemainingTimeIsShown()
        {
            var activity = _fixture.Build<CookingActivity>()
                                   .With(a => a.TotalTime, TimeSpan.FromSeconds(-5))
                                   .Create();

            StringAssert.AreEqualIgnoringCase("-00:05", new CookingActivityViewModel(activity).RemainingTime);
        }

        [Test]
        public void UserCannotPauseActivityIfItsNotStarted()
        {
            var vm = _fixture.Create<CookingActivityViewModel>();

            Assert.IsFalse(vm.PauseCommand.CanExecute(null));

            vm.StartCommand.Execute(null);

            Assert.IsTrue(vm.PauseCommand.CanExecute(null));

            vm.PauseCommand.Execute(null);

            Assert.IsFalse(vm.PauseCommand.CanExecute(null));

        }

        [Test]
        public void UserCanRestartAfterStopping()
        {
            var vm = _fixture.Create<CookingActivityViewModel>();
            
            vm.StartCommand.Execute(null);
            vm.StopCommand.Execute(null);

            Assert.IsTrue(vm.StartCommand.CanExecute(null));
        }

        [Test]
        public void ActivityOverdue()
        {
            var activity = _fixture.Build<CookingActivity>()
                                   .With(a => a.TotalTime, TimeSpan.FromSeconds(5))
                                   .Create();

            var vm = new CookingActivityViewModel(activity);

            vm.StartCommand.Execute(null);
            var scheduler = new TestScheduler();

            scheduler.AdvanceBy(TimeSpan.FromSeconds(2).Ticks);

            Assert.IsFalse(vm.IsOverdue);
            StringAssert.DoesNotContain("-", vm.RemainingTime);

            scheduler.AdvanceBy(TimeSpan.FromSeconds(5).Ticks);

            Assert.IsTrue(vm.IsOverdue);
            StringAssert.Contains("-", vm.RemainingTime);
        }
    }
}
