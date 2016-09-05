using Microsoft.VisualStudio.TestTools.UnitTesting;
using MahjongYakuBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            te.Kotsu.Add(new List<Pai> { Fonpai.Ton, Fonpai.Ton, Fonpai.Ton });
            te.Kotsu.Add(new List<Pai> { Fonpai.Nan, Fonpai.Nan, Fonpai.Nan });
            te.Kotsu.Add(new List<Pai> { Fonpai.Sha, Fonpai.Sha, Fonpai.Sha });
            te.Kotsu.Add(new List<Pai> { Fonpai.Pe, Fonpai.Pe, Fonpai.Pe });

            Assert.AreEqual(new DaiSuShi().Condition(te), true);
        }

        [TestMethod]
        public void TeIs四喜和()
        {
            var te = new Te();
            te.Janto = new List<Pai> { Fonpai.Pe, Fonpai.Pe };
            te.Kotsu.Add(new List<Pai> { Fonpai.Ton, Fonpai.Ton, Fonpai.Ton });
            te.Kotsu.Add(new List<Pai> { Fonpai.Nan, Fonpai.Nan, Fonpai.Nan });
            te.Kotsu.Add(new List<Pai> { Fonpai.Sha, Fonpai.Sha, Fonpai.Sha });
            te.Kotsu.Add(new List<Pai> { Sangenpai.Hatsu, Sangenpai.Hatsu, Sangenpai.Hatsu });

            Assert.AreEqual(new DaiSuShi().Condition(te), true);
        }

        [TestMethod]
        public void TeIs四暗刻()
        {
            var te = new Te();
            te.Janto = new List<Pai> { Fonpai.Pe, Fonpai.Pe };
            te.Kotsu.Add(new List<Pai> { Fonpai.Ton, Fonpai.Ton, Fonpai.Ton });
            te.Kotsu.Add(new List<Pai> { Fonpai.Nan, Fonpai.Nan, Fonpai.Nan });
            te.Kotsu.Add(new List<Pai> { Fonpai.Sha, Fonpai.Sha, Fonpai.Sha });
            te.Kotsu.Add(new List<Pai> { Sangenpai.Hatsu, Sangenpai.Hatsu, Sangenpai.Hatsu });
            te.AgariPai = Fonpai.Ton;
            te.Tsumo = true;

            Assert.AreEqual(new SuAnkou().Condition(te), true);

            te.AgariPai = Fonpai.Pe;
            Assert.AreNotEqual(new SuAnkou().Condition(te), true);
            Assert.AreEqual(new SuAnkouTanki().Condition(te), true);
        }

        [TestMethod]
        public void TeIs四暗刻単騎待ち()
        {
            var te = new Te();
            te.Janto = new List<Pai> { Fonpai.Pe, Fonpai.Pe };
            te.Kotsu.Add(new List<Pai> { Fonpai.Ton, Fonpai.Ton, Fonpai.Ton });
            te.Kotsu.Add(new List<Pai> { Fonpai.Nan, Fonpai.Nan, Fonpai.Nan });
            te.Kotsu.Add(new List<Pai> { Fonpai.Sha, Fonpai.Sha, Fonpai.Sha });
            te.Kotsu.Add(new List<Pai> { Sangenpai.Hatsu, Sangenpai.Hatsu, Sangenpai.Hatsu });
            te.AgariPai = Fonpai.Pe;
            te.Tsumo = true;

            Assert.AreEqual(new SuAnkouTanki().Condition(te), true);
        }

        [TestMethod]
        public void TeIs清一色()
        {
            var te = new Te();
            te.Janto = new List<Pai> { Manzu.One, Manzu.One };
            te.Kotsu.Add(new List<Pai> { Manzu.Two, Manzu.Two, Manzu.Two });
            te.Kotsu.Add(new List<Pai> { Manzu.Three, Manzu.Three, Manzu.Three });
            te.Kotsu.Add(new List<Pai> { Manzu.Four, Manzu.Four, Manzu.Four });
            te.Kotsu.Add(new List<Pai> { Manzu.Five, Manzu.Five, Manzu.Five });
            te.AgariPai = Manzu.Five;
            te.Tsumo = true;

            Assert.AreEqual(new ChinItsu().Condition(te), true);
        }
    }
}