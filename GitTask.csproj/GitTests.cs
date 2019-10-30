using NUnit.Framework;

namespace GitTask
{
    [TestFixture]
    public class GitTests
    {
        private const int DefaultFilesCount = 5;
        private Git sut;

        [SetUp]
        public void SetUp()
        {
            sut = new Git(DefaultFilesCount);
        }

        [Test]
        public void YouTried()
        {
            Assert.True(true, "Никто не написал этот тест...");
        }
   }
}