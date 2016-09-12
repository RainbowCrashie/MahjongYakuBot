using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MahjongYakuBot.Tests
{
    [TestClass()]
    public class PaiTests
    {
        [TestMethod]
        public void DoraTest()
        {
            Assert.AreSame(Manzu.One.Dora, Manzu.Two);
            Assert.AreSame(Souzu.One.Dora, Souzu.Two);
            Assert.AreSame(Sangenpai.Haku.Dora, Sangenpai.Hatsu);
            Assert.AreSame(Fonpai.Ton.Dora, Fonpai.Nan);
        }

        [TestMethod]
        public void DoraNineTest()
        {
            Assert.AreSame(Manzu.Nine.Dora, Manzu.One);
            Assert.AreSame(Sangenpai.Chun.Dora, Sangenpai.Haku);
            Assert.AreSame(Fonpai.Pe.Dora, Fonpai.Ton);
        }

        [TestMethod]
        public void 刻子()
        {
            Assert.AreEqual(Gates.IsKoutsu(Manzu.One, Manzu.One, Manzu.One), true);
            Assert.AreNotEqual(Gates.IsKoutsu(Manzu.One, Manzu.Two, Manzu.One), true);
        }

        [TestMethod]
        public void 順子()
        {
            Assert.AreEqual(Gates.IsJuntsu(Manzu.Two, Manzu.Three, Manzu.Four), true);
            Assert.AreEqual(Gates.IsJuntsu(Manzu.Two, Manzu.Four, Manzu.Three), true);
            Assert.AreNotEqual(Gates.IsJuntsu(Manzu.One, Manzu.Three, Manzu.Four), true);
        }

        [TestMethod]
        public void TeIs大四喜()
        {
            var te = new Te();
            te.Janto = new List<Pai>{Sangenpai.Hatsu, Sangenpai.Hatsu};
            te.Kotsus.Add(new Mentsu(Fonpai.Ton, Fonpai.Ton, Fonpai.Ton));
            te.Kotsus.Add(new Mentsu(Fonpai.Nan, Fonpai.Nan, Fonpai.Nan));
            te.Kotsus.Add(new Mentsu(Fonpai.Sha, Fonpai.Sha, Fonpai.Sha));
            te.Kotsus.Add(new Mentsu(Fonpai.Pe, Fonpai.Pe, Fonpai.Pe));

            Assert.AreEqual(new DaiSuShi().Condition(te), true);
        }

        [TestMethod]
        public void TeIs四喜和()
        {
            var te = new Te();
            te.Janto = new List<Pai> { Fonpai.Pe, Fonpai.Pe };
            te.Kotsus.Add(new Mentsu(Fonpai.Ton, Fonpai.Ton, Fonpai.Ton));
            te.Kotsus.Add(new Mentsu(Fonpai.Nan, Fonpai.Nan, Fonpai.Nan));
            te.Kotsus.Add(new Mentsu(Fonpai.Sha, Fonpai.Sha, Fonpai.Sha));
            te.Kotsus.Add(new Mentsu(Sangenpai.Hatsu, Sangenpai.Hatsu, Sangenpai.Hatsu));

            Assert.AreEqual(new SuShiHou().Condition(te), true);
        }

        [TestMethod]
        public void TeIs四暗刻()
        {
            var te = new Te();
            te.Janto = new List<Pai> { Fonpai.Pe, Fonpai.Pe };
            te.Kotsus.Add(new Mentsu(Fonpai.Ton, Fonpai.Ton, Fonpai.Ton));
            te.Kotsus.Add(new Mentsu(Fonpai.Nan, Fonpai.Nan, Fonpai.Nan));
            te.Kotsus.Add(new Mentsu(Fonpai.Sha, Fonpai.Sha, Fonpai.Sha));
            te.Kotsus.Add(new Mentsu(Sangenpai.Hatsu, Sangenpai.Hatsu, Sangenpai.Hatsu));
            te.AgariPai = Fonpai.Ton;
            te.Tsumo = true;

            Assert.AreEqual(new SuAnko().Condition(te), true);

            te.AgariPai = Fonpai.Pe;
            Assert.AreNotEqual(new SuAnko().Condition(te), true);
            Assert.AreEqual(new SuAnkoTanki().Condition(te), true);
        }

        [TestMethod]
        public void TeIs四暗刻単騎待ち()
        {
            var te = new Te();
            te.Janto = new List<Pai> { Fonpai.Pe, Fonpai.Pe };
            te.Kotsus.Add(new Mentsu(Fonpai.Ton, Fonpai.Ton, Fonpai.Ton));
            te.Kotsus.Add(new Mentsu(Fonpai.Nan, Fonpai.Nan, Fonpai.Nan));
            te.Kotsus.Add(new Mentsu(Fonpai.Sha, Fonpai.Sha, Fonpai.Sha));
            te.Kotsus.Add(new Mentsu(Sangenpai.Hatsu, Sangenpai.Hatsu, Sangenpai.Hatsu));
            te.AgariPai = Fonpai.Pe;
            te.Tsumo = true;

            Assert.AreEqual(new SuAnkoTanki().Condition(te), true);
        }

        [TestMethod]
        public void TeIs清一色()
        {
            var te = new Te();
            te.Janto = new List<Pai> { Manzu.One, Manzu.One };
            te.Kotsus.Add(new Mentsu(Manzu.Two, Manzu.Two, Manzu.Two));
            te.Kotsus.Add(new Mentsu(Manzu.Three, Manzu.Three, Manzu.Three));
            te.Kotsus.Add(new Mentsu(Manzu.Four, Manzu.Four, Manzu.Four));
            te.Kotsus.Add(new Mentsu(Manzu.Five, Manzu.Five, Manzu.Five));
            te.AgariPai = Manzu.Five;
            te.Tsumo = true;

            Assert.AreEqual(new ChinItsu().Condition(te), true);
        }

        [TestMethod]
        public void TeIs小三元()
        {
            var te = new Te();
            te.Janto = new List<Pai> { Sangenpai.Chun, Sangenpai.Chun };
            te.Kotsus.Add(new Mentsu(Fonpai.Ton, Fonpai.Ton, Fonpai.Ton));
            te.Kotsus.Add(new Mentsu(Fonpai.Nan, Fonpai.Nan, Fonpai.Nan));
            te.Kotsus.Add(new Mentsu(Sangenpai.Haku, Sangenpai.Haku, Sangenpai.Haku));
            te.Kotsus.Add(new Mentsu(Sangenpai.Hatsu, Sangenpai.Hatsu, Sangenpai.Hatsu));
            te.AgariPai = Fonpai.Ton;
            te.Tsumo = true;

            Assert.AreEqual(new ShouSangen().Condition(te), true);
        }

        [TestMethod]
        public void TeIs三色同刻()
        {
            var te = new Te();
            te.Janto = new List<Pai> { Sangenpai.Chun, Sangenpai.Chun };
            te.Kotsus.Add(new Mentsu(Manzu.Two, Manzu.Two, Manzu.Two));
            te.Kotsus.Add(new Mentsu(Pinzu.Two, Pinzu.Two, Pinzu.Two));
            te.Kotsus.Add(new Mentsu(Souzu.Two, Souzu.Two, Souzu.Two));
            te.Kotsus.Add(new Mentsu(Manzu.Five, Manzu.Five, Manzu.Five));
            te.AgariPai = Manzu.Five;
            te.Tsumo = true;

            Assert.AreEqual(new SanShokuDouKou().Condition(te), true);
        }

        [TestMethod]
        public void TeIs一気通貫()
        {
            var te = new Te();
            te.Janto = new List<Pai> { Manzu.One, Manzu.One };
            te.Shuntsus.Add(new Mentsu(Manzu.One, Manzu.Two, Manzu.Three));
            te.Shuntsus.Add(new Mentsu(Manzu.Four, Manzu.Five, Manzu.Six));
            te.Shuntsus.Add(new Mentsu(Manzu.Seven, Manzu.Eight, Manzu.Nine));
            te.Shuntsus.Add(new Mentsu(Manzu.Five, Manzu.Five, Manzu.Five));
            te.AgariPai = Manzu.One;
            te.Tsumo = true;

            Assert.AreEqual(new Ittsu().Condition(te), true);

            te.Shuntsus[2] = new Mentsu(Souzu.Seven, Souzu.Eight, Souzu.Nine);

            Assert.AreNotEqual(new Ittsu().Condition(te), true);

            te.Shuntsus[3] = new Mentsu(Manzu.Seven, Manzu.Eight, Manzu.Nine);

            Assert.AreEqual(new Ittsu().Condition(te), true);
        }

        [TestMethod]
        public void TeIs二盃口()
        {
            var te = new Te();
            te.Janto = new List<Pai> { Manzu.One, Manzu.One };
            te.Shuntsus.Add(new Mentsu(Manzu.One, Manzu.Two, Manzu.Three));
            te.Shuntsus.Add(new Mentsu(Manzu.One, Manzu.Two, Manzu.Three));
            te.Shuntsus.Add(new Mentsu(Manzu.Seven, Manzu.Eight, Manzu.Nine));
            te.Shuntsus.Add(new Mentsu(Manzu.Seven, Manzu.Eight, Manzu.Nine));
            te.AgariPai = Manzu.One;
            te.Tsumo = true;

            Assert.AreEqual(new RyanPeiKou().Condition(te), true);
        }

        [TestMethod]
        public void TeIs九蓮宝燈()
        {
            var te = new Te();
            te.Janto = new List<Pai> { Manzu.Eight, Manzu.Eight };
            te.Shuntsus.Add(new Mentsu(Manzu.Two, Manzu.Three, Manzu.Four));
            te.Shuntsus.Add(new Mentsu(Manzu.Five, Manzu.Six, Manzu.Seven));
            te.Kotsus.Add(new Mentsu(Manzu.One, Manzu.One, Manzu.One));
            te.Kotsus.Add(new Mentsu(Manzu.Nine, Manzu.Nine, Manzu.Nine));
            te.AgariPai = Manzu.Five;
            te.Tsumo = true;

            Assert.AreEqual(new ChuRen().Condition(te), true);
        }

        [TestMethod]
        public void TeIs純正九蓮宝燈()
        {
            var te = new Te();
            te.Janto = new List<Pai> { Manzu.Eight, Manzu.Eight };
            te.Shuntsus.Add(new Mentsu(Manzu.Two, Manzu.Three, Manzu.Four));
            te.Shuntsus.Add(new Mentsu(Manzu.Five, Manzu.Six, Manzu.Seven));
            te.Kotsus.Add(new Mentsu(Manzu.One, Manzu.One, Manzu.One));
            te.Kotsus.Add(new Mentsu(Manzu.Nine, Manzu.Nine, Manzu.Nine));
            te.AgariPai = Manzu.One;
            te.Tsumo = true;

            Assert.AreEqual(new JunSeiChuRen().Condition(te), true);
        }


    }
}