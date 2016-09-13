using Mahjong;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MahjongYakuBot.Tests
{
    [TestClass]
    public class TextPaiAnalizerTests
    {
        [TestMethod]
        public void TextToPai()
        {
            Assert.AreSame(TextPaiAnalizer.DeterminePai("一筒"), Pinzu.One);
            Assert.AreSame(TextPaiAnalizer.DeterminePai("リャンワン"), Manzu.Two);
        }
    }
}