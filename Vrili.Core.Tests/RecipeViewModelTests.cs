using MvvmCross.Test.Core;
using Moq;
using NUnit.Framework;
using Vrili.Core.ViewModels;
using Vrili.Core.Services;
using Vrili.Core.Models;
using MvvmCross.Plugins.Share;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using AutoDataConnector;
using Vrili.Core.Test.Mocks;
using MvvmCross.Core.Views;
using MvvmCross.Platform.Core;
using System;
using MvvmCross.Plugins.Messenger;

namespace Vrili.Core.Tests
{
    [TestFixture]
    public class RecipeViewModelTests : MvxIoCSupportingTest
    {

        private Mock<IAlarmBell> alarmBellMock;
        private Mock<IRecipeRepo> recipeRepoMock;
        private Mock<IMvxShareTask> shareTaskMock;
        private Mock<IMvxMessenger> messengerMock;

        private RecipeViewModel recipeViewModel;

        private IFixture _fixture;
        private MockMvxViewDispatcher mockDispatcher;
        private Action<LoadRecipeMessage> _publishActiveRecipe;


        [SetUp]
        public void SetUp()
        {
            ClearAll();

            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            alarmBellMock = _fixture.Freeze<Mock<IAlarmBell>>();
            recipeRepoMock = _fixture.Freeze<Mock<IRecipeRepo>>();
            shareTaskMock = _fixture.Freeze<Mock<IMvxShareTask>>();
            messengerMock = _fixture.Freeze<Mock<IMvxMessenger>>();

            SetupPublisherForActiveRecipeMessage();

            recipeViewModel = _fixture.Create<RecipeViewModel>();

            mockDispatcher = new MockMvxViewDispatcher();
            Ioc.RegisterSingleton<IMvxViewDispatcher>(mockDispatcher);
            Ioc.RegisterSingleton<MvxMainThreadDispatcher>(mockDispatcher);
        }

        private void SetupPublisherForActiveRecipeMessage()
        {
            messengerMock.Setup(n => n.SubscribeOnMainThread(
                It.IsAny<Action<LoadRecipeMessage>>(),
                It.IsAny<MvxReference>(),
                It.IsAny<string>()))
                        .Callback<
                            Action<LoadRecipeMessage>,
                            MvxReference,
                            string>(
                                (action, mvxref, tag) => _publishActiveRecipe = action);
        }

        [Test]
        public void AddingActivitiesToTheRecipe(
            [Random(1, 20, 2)]int numberOfActivities)
        {

            for (int i = 0; i < numberOfActivities; i++)
            {
                recipeViewModel.AddActivityCommand.Execute(null);
            }

            Assert.AreEqual(recipeViewModel.Activities.Count, numberOfActivities);
        }

        [Test]
        public void SavingRecipe()
        {
            recipeViewModel.SaveCommand.Execute(null);
            recipeRepoMock.Verify(r => r.Save(It.IsAny<Recipe>()), Times.Exactly(1));
        }

        [Test]
        public void SharingRecipe()
        {
            recipeViewModel.ShareCommand.Execute(null);

            shareTaskMock.Verify(r => r.ShareLink(
                                            It.IsAny<string>(), 
                                            It.IsAny<string>(), 
                                            It.IsAny<string>()), 
                                        Times.Exactly(1));
        }

        [Test]
        public void OpenGoesToTheCookbook()
        {
            recipeViewModel.OpenCommand.Execute(null);

            mockDispatcher.AssertNavigatedTo<CookbookViewModel>();
        }

        [Test]
        public void LoadingRecipe()
        {
            Recipe recipe = _fixture.Create<Recipe>();
            recipeRepoMock.Setup(r => r.Get(recipe.Id)).Returns(recipe);

            _publishActiveRecipe(new LoadRecipeMessage(this, recipe.Id));

            recipeRepoMock.Verify(r => r.Get(recipe.Id), Times.Exactly(1));
            Assert.AreEqual(recipe.Activities.Count, recipeViewModel.Activities.Count);

        }
    }
}
