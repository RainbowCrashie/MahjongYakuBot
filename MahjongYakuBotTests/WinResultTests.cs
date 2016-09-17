using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mahjong.Tests
{
    [TestClass]
    public class WinResultTests
    {
        [TestMethod]
        public void GetWinResultTest()
        {
            var tepai = new List<Pai>
            {
                Souzu.One, Souzu.One,
                Manzu.Seven, Manzu.Eight, Manzu.Nine,
                Pinzu.One, Pinzu.One, Pinzu.One,
                Pinzu.Two, Pinzu.Two, Pinzu.Two,
                Pinzu.Three, Pinzu.Three, Pinzu.Three
            };
        }
    }
}