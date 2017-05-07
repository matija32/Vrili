using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Vrili.Core.Test.Mocks
{
    public class MockMvxViewDispatcher : MvxMainThreadDispatcher, IMvxViewDispatcher
    {
        public List<IMvxViewModel> CloseRequests = new List<IMvxViewModel>();
        public List<MvxViewModelRequest> NavigateRequests = new List<MvxViewModelRequest>();

        public bool ShowViewModel(MvxViewModelRequest request)
        {
            NavigateRequests.Add(request);
            return true;
        }

        public bool ChangePresentation(MvxPresentationHint hint)
        {
            if (hint.GetType() == typeof(MvxClosePresentationHint))
            {
                CloseRequests.Add((hint as MvxClosePresentationHint).ViewModelToClose);
            }

            return true;
        }

        public bool RequestMainThreadAction(Action action)
        {
            action();
            return true;
        }

        public void AssertNavigatedTo<ViewModelType>()
        {
            Assert.AreEqual(1, NavigateRequests.Count);
            Assert.AreEqual(typeof(ViewModelType), NavigateRequests[0].ViewModelType);
        }

        public void AssertClosed<ViewModelType>()
        {
            Assert.AreEqual(1, CloseRequests.Count);
            Assert.AreEqual(typeof(ViewModelType), CloseRequests[0].GetType());
        }
    }
}