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

namespace Vrili.Core.Tests
{
    [TestFixture]
    public class RecipeViewModelTests : MvxIoCSupportingTest
    {

        private Mock<IAlarmBell> alarmBellMock;
        private Mock<IRecipeRepo> recipeRepoMock;
        private Mock<IMvxShareTask> shareTaskMock;
        private RecipeViewModel recipeViewModel;

        private readonly IFixture _fixture;
        private MockMvxViewDispatcher mockDispatcher;

        public RecipeViewModelTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        [SetUp]
        public void SetUp()
        {
            ClearAll();

            alarmBellMock = _fixture.Freeze<Mock<IAlarmBell>>();
            recipeRepoMock = _fixture.Freeze<Mock<IRecipeRepo>>();
            shareTaskMock = _fixture.Freeze<Mock<IMvxShareTask>>();
            recipeViewModel = _fixture.Create<RecipeViewModel>();

            mockDispatcher = new MockMvxViewDispatcher();
            Ioc.RegisterSingleton<IMvxViewDispatcher>(mockDispatcher);
            Ioc.RegisterSingleton<MvxMainThreadDispatcher>(mockDispatcher);
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
        public void ShareRecipe()
        {
            recipeViewModel.ShareCommand.Execute(null);

            shareTaskMock.Verify(r => r.ShareLink(
                                            It.IsAny<string>(), 
                                            It.IsAny<string>(), 
                                            It.IsAny<string>()), 
                                        Times.Exactly(1));
        }

        [Test]
        public void OpenRecipeFromACookbook()
        {
            recipeViewModel.OpenCommand.Execute(null);

            mockDispatcher.AssertNavigatedTo<CookbookViewModel>();
        }
    }
}
