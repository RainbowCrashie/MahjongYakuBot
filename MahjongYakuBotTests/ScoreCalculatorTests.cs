using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Mahjong.Tests
{
    [TestClass()]
    public class ScoreCalculatorTests
    {
        [TestMethod()]
        public void CountDorasTest()
        {
            var te = new Te
            {
                Janto = new List<Pai> { Manzu.Two, Manzu.Two },
                Doras = new List<Pai> {Manzu.One, Manzu.One, Pinzu.Two}
            };
            te.Kotsus.Add(new Mentsu(Pinzu.Three, Pinzu.Three, Pinzu.Three));
            te.Kotsus.Add(new Mentsu(Pinzu.Four, Pinzu.Four, Pinzu.Four));

            Assert.AreEqual(ScoreCalculator.CountDoras(te), 7);
        }
    }
}