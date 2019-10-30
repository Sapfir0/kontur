using NUnit.Framework;

namespace ImageParser
{
    [TestFixture]
    public class ImageParserTests
    {
        private ImageParser parser;

        [SetUp]
        public void SetUp()
        {
            parser = new ImageParser();
        }

        [Test]
        public void YouTried()
        {
            Assert.True(true, "Никто не написал этот тест...");
        }
   }
}