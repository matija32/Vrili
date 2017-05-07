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
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Vrili.Core.Tests
{
    [TestFixture]
    public class CookbookViewModelTests : MvxIoCSupportingTest
    {
        private Mock<IRecipeRepo> recipeRepoMock;
        private Mock<IMvxMessenger> messengerMock;

        private IFixture _fixture;
        private MockMvxViewDispatcher mockDispatcher;

        [SetUp]
        public void SetUp()
        {
            ClearAll();

            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            recipeRepoMock = _fixture.Freeze<Mock<IRecipeRepo>>();
            messengerMock = _fixture.Freeze<Mock<IMvxMessenger>>();

            mockDispatcher = new MockMvxViewDispatcher();
            Ioc.RegisterSingleton<IMvxViewDispatcher>(mockDispatcher);
            Ioc.RegisterSingleton<MvxMainThreadDispatcher>(mockDispatcher);
        }

        [Test]
        public void ShowingAllRecipesOnCreation()
        {
            List<Recipe> recipes = _fixture.Create<List<Recipe>>();
            recipeRepoMock.Setup(r => r.GetAllRecipes()).Returns(recipes);

            var cookbookViewModel = _fixture.Create<CookbookViewModel>();

            recipeRepoMock.Verify(r => r.GetAllRecipes(), Times.Exactly(1));
            Assert.AreEqual(cookbookViewModel.RecipeNames, recipes);
        }

        [Test]
        public void LoadingActivityOnClick()
        {
            Recipe recipe = _fixture.Create<Recipe>();
            List<Recipe> recipes = new List<Recipe> { recipe };
            recipeRepoMock.Setup(r => r.GetAllRecipes()).Returns(recipes);

            var cookbookViewModel = _fixture.Create<CookbookViewModel>();
            cookbookViewModel.OpenRecipeCommand.Execute(recipe);

            messengerMock.Verify(
                messenger => messenger.Publish(It.Is(MessageWith(recipe.Id))), 
                Times.Exactly(1));

            mockDispatcher.AssertClosed<CookbookViewModel>();

        }

        private Expression<Func<LoadRecipeMessage, bool>> MessageWith(int recipeId)
        {
            return m => m.RecipeId == recipeId;
        }
    }
}
