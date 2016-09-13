using System.Collections.Generic;
using System.Linq;

namespace Mahjong
{
    public abstract class Yaku
    {
        public abstract string Name { get; }

        public abstract bool Condition(Te te);

        public bool Result { get; set; }

        public abstract int Hansu { get; }

        public bool KuiSagari { get; } = false;

        public static readonly int Yakuman = 100;

        public static readonly int DoubleYakuman = 200;

        public static readonly int TripleYakuman = 300;

        protected bool ContainsJihai(Te te)
        {
            return (te.AllPais().Any(pai => pai is Jihai));
        }

        protected bool HazSpecificKotsu(Te te, Pai pai)
        {
            return te.Kotsus.Any(kotsu => kotsu.Pais[0] == pai);
        }
    }
    
    public class ChinRouTou : Yaku
    {
        public override string Name { get; } = "清老頭";

        public override int Hansu { get; } = Yakuman;

        public override bool Condition(Te te)
        {
            return te.AllPais().All(pai => pai.IsRouTouPai());
        }
    }

    public abstract class ChuRenBase : Yaku
    {
        protected bool CheckTe(Te te)
        {
            var pais = te.AllPais();

            if (!pais.Any(pai => Gates.IsSameType(pais[0], pai)))
                return false;

            if (pais.Count(pai => pai.Number == 1) < 3)
                return false;

            if (pais.Count(pai => pai.Number == 9) < 3)
                return false;

            return AllChunChanNumber.All(n => pais.Select(p => p.Number).Contains(n));
        }

        protected static List<int> AllChunChanNumber { get; } = new List<int> {2,3,4,5,6,7,8};
    }

    public class ChuRen : ChuRenBase
    {
        public override string Name { get; } = "九蓮宝燈";

        public override int Hansu { get; } = Yakuman;

        public override bool Condition(Te te)
        {
            if (new JunSeiChuRen().Condition(te))
                return false;

            return CheckTe(te);
        }
    }

    public class JunSeiChuRen : ChuRenBase
    {
        public override string Name { get; } = "純正九蓮宝燈";

        public override int Hansu { get; } = Yakuman;

        public override bool Condition(Te te)
        {
            if (!CheckTe(te))
                return false;

            var hand = new List<Pai>();
            hand.AddRange(te.AllPais());
            hand.Remove(te.AgariPai);

            return AllChunChanNumber.All(n => hand.Select(p => p.Number).Contains(n));
        }
    }

    public class RyuISou : Yaku
    {
        public override string Name { get; } = "緑一色";

        public override int Hansu { get; } = Yakuman;

        public override bool Condition(Te te)
        {
            return te.AllPais().All(Midori.Contains);
        }

        private List<Pai> Midori { get; } = new List<Pai>
        {
            Souzu.Two,
            Souzu.Three,
            Souzu.Four,
            Souzu.Six,
            Souzu.Eight,
            Sangenpai.Hatsu
        };
    }

    public class DaiSuShi : Yaku
    {
        public override string Name { get; } = "大四喜";

        public override int Hansu { get; } = DoubleYakuman;

        public override bool Condition(Te te)
        {
            return FonpaiList.List.All(pai => HazSpecificKotsu(te, pai));
        }
    }

    public class SuShiHou : Yaku
    {
        public override string Name { get; } = "四喜和";

        public override int Hansu { get; } = Yakuman;

        public override bool Condition(Te te)
        {
            if (!(te.Janto[0] is Fonpai))
                return false;

            var fonpais = new List<Pai>();
            fonpais.AddRange(FonpaiList.List);

            fonpais.Remove(te.Janto[0]);

            return fonpais.All(pai => HazSpecificKotsu(te, pai));
        }
    }

    public class TsuIiSou : Yaku
    {
        public override string Name { get; } = "字一色";

        public override int Hansu { get; } = Yakuman;

        public override bool Condition(Te te)
        {
            return te.AllPais().All(pai => pai is Jihai);
        }
    }

    public class DaiSanGen : Yaku
    {
        public override string Name { get; } = "大三元";

        public override int Hansu { get; } = Yakuman;

        public override bool Condition(Te te)
        {
            return SangenpaiList.List.All(pai => HazSpecificKotsu(te, pai));
        }
    }

    public class SuAnko : Yaku
    {
        public override string Name { get; } = "四暗刻";

        public override int Hansu { get; } = Yakuman;

        public override bool Condition(Te te)
        {
            if (new SuAnkoTanki().Condition(te))
                return false;

            if (!te.IsMenzen())
                return false;

            if (te.Kotsus.Count != 4)
                return false;

            if (te.Tsumo == false)
            {
                if (te.AgariPai != te.Janto[0])
                    return false;
            }

            return true;
        }
    }

    public class SuAnkoTanki : Yaku
    {
        public override string Name { get; } = "四暗刻単騎待ち";

        public override int Hansu { get; } = DoubleYakuman;

        public override bool Condition(Te te)
        {
            if (!te.IsMenzen())
                return false;

            if (te.Kotsus.Count != 4)
                return false;

            if (te.AgariPai != te.Janto[0])
                return false;

            return true;
        }
    }

    public class ChinItsu : Yaku
    {
        public override string Name { get; } = "清一色";

        public override int Hansu { get; } = 6;

        public override bool Condition(Te te)
        {
            if (ContainsJihai(te))
                return false;

            return te.AllPais().All(pai => Gates.IsSameType(te.AllPais()[0], pai));
        }
    }

    public class HonItsu : Yaku
    {
        public override string Name { get; } = "混一色";

        public override int Hansu { get; } = 3;

        public override bool Condition(Te te)
        {
            if (new ChinItsu().Condition(te))
                return false;

            var teSupai = te.AllPais().Where(pai => pai is Supai).ToList();

            return teSupai.All(pai => Gates.IsSameType(teSupai[0], pai));
        }
    }

    public abstract class ChantaBase : Yaku
    {
        protected bool CheckSupai(Te te)
        {
            //Tolerating Jihais are valid even for JunChan as far as JunChan rejects te contains Jihai.
            if (!te.Janto[0].IsRouTouPai() || !(te.Janto[0] is Jihai))
                return false;

            if (te.Kotsus.Select(k => k.Pais[0]).All(pai => pai.IsRouTouPai() || pai is Jihai))
                return false;

            if (te.Shuntsus.All(shuntsu => !shuntsu.Pais.Any(pai => pai.IsRouTouPai())))
                return false;

            return true;
        }
    }

    public class JunChan : ChantaBase
    {
        public override string Name { get; } = "純全帯么九";

        public override int Hansu { get; } = 3;

        public override bool Condition(Te te)
        {
            if (ContainsJihai(te))
                return false;
            
            return CheckSupai(te);
        }
    }

    public class HonChan : ChantaBase
    {
        public override string Name { get; } = "混全帯么九";

        public override int Hansu { get; } = 2;

        public override bool Condition(Te te)
        {
            if (new JunChan().Condition(te))
                return false;

            if (new HonRouTou().Condition(te))
                return false;

            return CheckSupai(te);
        }
    }

    public class ShouSangen : Yaku
    {
        public override string Name { get; } = "小三元";

        public override int Hansu { get; } = 2;

        public override bool Condition(Te te)
        {
            if (!(te.Janto[0] is Sangenpai))
                return false;

            return te.Kotsus.Count(kotsu => kotsu.Pais[0] is Sangenpai) == 2;
        }
    }

    public class SanAnko : Yaku
    {
        public override string Name { get; } = "三暗刻";

        public override int Hansu { get; } = 2;

        public override bool Condition(Te te)
        {
            if (new SuAnko().Condition(te))
                return false;

            if (new SuAnkoTanki().Condition(te))
                return false;

            if (te.Kotsus.Count(kotsu => kotsu.Kui == false) < 3)
                return false;
            
            //At this point .Count() == 4 is rejected by condition: !SuAnko
            if (!te.Tsumo)
                if (!(te.Janto.Contains(te.AgariPai) ||
                    (te.Shuntsus[0].Kui && te.Shuntsus[0].Pais.Contains(te.AgariPai))))
                    return false;

            return true;
        }
    }

    public class ToiToi : Yaku
    {
        public override string Name { get; } = "対々和";

        public override int Hansu { get; } = 2;

        public override bool Condition(Te te)
        {
            return (te.Kotsus.Count == 4);
        }
    }

    public class SanShokuDouKou : Yaku
    {
        public override string Name { get; } = "三色同刻";

        public override int Hansu { get; } = 2;

        public override bool Condition(Te te)
        {
            if (te.Kotsus.Count < 3)
                return false;

            return te.Kotsus.Select(k => k.Pais[0]).Any(sampleKotsu =>
                sampleKotsu is Supai &&
                te.Kotsus.Select(k => k.Pais[0]).Where(kotsu => kotsu.Number == sampleKotsu.Number).ToList().Count == 3
                );
        }
    }

    public class SanShokuDouJun : Yaku
    {
        public override string Name { get; } = "三色同順";

        public override int Hansu { get; } = 2;

        public new bool KuiSagari { get; } = true;

        public override bool Condition(Te te)
        {
            if (te.Shuntsus.Count < 3)
                return false;

            foreach (var sampleKotsu in te.Shuntsus)
            {
                var douShuns =
                    te.Shuntsus.Where(shuntsu => shuntsu.Pais.Min().Number == sampleKotsu.Pais.Min().Number).ToList();

                if (!douShuns.Any(shuntsu => shuntsu.Pais[0] is Manzu))
                    return false;

                if (!douShuns.Any(shuntsu => shuntsu.Pais[0] is Pinzu))
                    return false;

                if (!douShuns.Any(shuntsu => shuntsu.Pais[0] is Souzu))
                    return false;
            }

            return true;
        }
    }

    public class Ittsu : Yaku
    {
        public override string Name { get; } = "一気通貫";

        public override int Hansu { get; } = 2;

        public new bool KuiSagari { get; } = true;

        public override bool Condition(Te te)
        {
            if (te.Shuntsus.Count < 3)
                return false;

            var shuntsus =
                te.Shuntsus.Select(s => s.Pais.Min())
                    .Where(shuntsu => shuntsu.Number == 1 || shuntsu.Number == 4 || shuntsu.Number == 7)
                    .ToList();

            return shuntsus.Any(sampleShuntsu =>
                shuntsus.Where(shuntsu => shuntsu.GetType() == sampleShuntsu.GetType()).ToList().Count == 3
                );
        }
    }

    public class HonRouTou : Yaku
    {
        public override string Name { get; } = "混老頭";

        public override int Hansu { get; } = 2;

        public override bool Condition(Te te)
        {
            return te.AllPais().All(pai => pai.IsRouTouPai() || pai is Jihai);
        }
    }

    public class MenFonPai : Yaku
    {
        public override string Name { get; } = "門風牌";

        public override int Hansu { get; } = 1;

        public override bool Condition(Te te)
        {
            return HazSpecificKotsu(te, te.JiFu);
        }
    }

    public class ChanFonPai : Yaku
    {
        public override string Name { get; } = "荘風牌";

        public override int Hansu { get; } = 1;

        public override bool Condition(Te te)
        {
            return HazSpecificKotsu(te, te.BaFu);
        }
    }

    public class YakuHai : Yaku
    {
        public override string Name { get; } = "三元牌";

        public override int Hansu { get; } = 1;

        public override bool Condition(Te te)
        {
            if (new ShouSangen().Condition(te))
                return false;
            
            return te.Kotsus.Select(kotsu => kotsu.Pais[0]).Any(pai => pai is Sangenpai);
        }
    }

    public class TanYao : Yaku
    {
        public override string Name { get; } = "断么九";

        public override int Hansu { get; } = 1;

        public override bool Condition(Te te)
        {
            return te.AllPais().All(pai => pai.IsChunChanPai());
        }
    }

    public class PinFu : Yaku
    {
        public override string Name { get; } = "平和";

        public override int Hansu { get; } = 1;

        public override bool Condition(Te te)
        {
            if (!te.IsMenzen())
                return false;

            if (te.Janto[0] is Sangenpai)
                return false;

            if (te.Janto[0] == te.BaFu)
                return false;

            if (te.Janto[0] == te.JiFu)
                return false;

            if (te.Shuntsus.Count < 4)
                return false;

            if (!te.Shuntsus.Select(s => s.Pais.Min()).ToList().Any(pai => pai == te.AgariPai))
                return false;

            return !te.Shuntsus.Select(s => s.Pais.Max()).ToList().Any(pai => pai == te.AgariPai);
        }
    }

    public class MenzenTsumo : Yaku
    {
        public override string Name { get; } = "門前清自摸和";

        public override int Hansu { get; } = 1;

        public override bool Condition(Te te)
        {
            if (!te.IsMenzen())
                return false;

            return te.Tsumo;
        }
    }

    public class RyanPeiKou : Yaku
    {
        public override string Name { get; } = "二盃口";

        public override int Hansu { get; } = 3;

        public override bool Condition(Te te)
        {
            if (!te.IsMenzen())
                return false;

            if (te.Shuntsus.Count != 4)
                return false;

            var shuntsuPool = te.Shuntsus.Select(s => s.Pais.Min()).ToList();

            var sampleShuntsu = shuntsuPool[0];
            shuntsuPool.RemoveAt(0);

            shuntsuPool.RemoveAll(shuntsu => shuntsu == sampleShuntsu);

            if (shuntsuPool.Count != 2)
                return false;

            return shuntsuPool[0] == shuntsuPool[1];
        }
    }

    public class IiPeiKou : Yaku
    {
        public override string Name { get; } = "一盃口";

        public override int Hansu { get; } = 3;

        public override bool Condition(Te te)
        {
            if (!te.IsMenzen())
                return false;

            if (te.Shuntsus.Count < 2)
                return false;

            if (new RyanPeiKou().Condition(te))
                return false;

            var shuntsuPool = te.Shuntsus.Select(s => s.Pais.Min()).ToList();

            var sampleShuntsu = shuntsuPool[0];
            shuntsuPool.RemoveAt(0);

            return shuntsuPool.Contains(sampleShuntsu);
        }
    }

    public class YakumanList : List<Yaku>
    {
        protected YakumanList()
        {
        }

        public static YakumanList Create()
        {
            var lst = new YakumanList
            {
                new ChinRouTou(),
                new ChuRen(),
                new JunSeiChuRen(),
                new RyuISou(),
                new DaiSuShi(),
                new SuShiHou(),
                new TsuIiSou(),
                new DaiSanGen(),
                new SuAnko(),
                new SuAnkoTanki(),
            };

            return lst;
        }

        public static YakumanList List { get; } = Create();
    }

    public class YakuList : List<Yaku>
    {
        protected YakuList()
        {
        }

        public static YakuList Create()
        {
            var lst = new YakuList
            {
                new ChinItsu(),
                new HonItsu(),
                new JunChan(),
                new HonChan(),
                new ShouSangen(),
                new SanAnko(),
                new ToiToi(),
                new SanShokuDouJun(),
                new SanShokuDouKou(),
                new Ittsu(),
                new HonRouTou(),
                new MenFonPai(),
                new YakuHai(),
                new TanYao(),
                new PinFu(),
                new MenzenTsumo(),
                new RyanPeiKou(),
                new IiPeiKou()
            };

            return lst;
        }

        public static YakuList List { get; } = Create();
    }

    public abstract class DeclaredYaku : Yaku
    {
        //リーチ　一発　ダブルリーチ2　嶺上開花　海底撈月　河底撈魚
        public abstract string[] Aliases { get; }

        public override bool Condition(Te te)
        {
            return true;
        }
    }

    public class Riichi : DeclaredYaku
    {
        public override string Name { get; } = "立直";

        public override int Hansu { get; } = 1;

        public override string[] Aliases { get; } = {"リーチ", "立直"};
    }

    public class Ippatsu : DeclaredYaku
    {
        public override string Name { get; } = "一発";

        public override int Hansu { get; } = 1;

        public override string[] Aliases { get; } = { "一発", "イッパツ" };
    }

    public class RinShanKaiHou : DeclaredYaku
    {
        public override string Name { get; } = "嶺上開花";

        public override int Hansu { get; } = 1;

        public override string[] Aliases { get; } = { "嶺上開花" };
    }

    public class KaiTei : DeclaredYaku
    {
        public override string Name { get; } = "海底撈月";

        public override int Hansu { get; } = 1;

        public override string[] Aliases { get; } = { "海底撈月", "カイテイ" };
    }

    public class HouTei : DeclaredYaku
    {
        public override string Name { get; } = "河底撈魚";

        public override int Hansu { get; } = 1;

        public override string[] Aliases { get; } = { "河底撈魚", "ホウテイ" };
    }

    public class DoubleRiichi : DeclaredYaku
    {
        public override string Name { get; } = "ダブル立直";

        public override int Hansu { get; } = 2;

        public override string[] Aliases { get; } = { "ダブルリーチ", "ダブル立直" };
    }

}
