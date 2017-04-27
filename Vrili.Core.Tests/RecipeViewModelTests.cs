using MvvmCross.Test.Core;
using Moq;
using NUnit.Framework;

namespace Vrili.Core.Tests
{
    [TestFixture]
    public class RecipeViewModelTests : MvxIoCSupportingTest
    {
        [Test]
        public void TestViewModel()
        {
            base.Setup(); // from MvxIoCSupportingTest
            
            int x = 3 + 2;
        }
    }
}
