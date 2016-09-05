using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace MahjongYakuBot
{
    public abstract class Yaku
    {
        public abstract string Name { get; }

        public abstract bool Condition(Te te);

        public abstract int Hansu { get; }

        public static readonly int Yakuman = 100;

        public static readonly int DoubleYakuman = 200;

        public static readonly int TripleYakuman = 300;
    }

    public class ChinRoutou : Yaku
    {
        public override string Name { get; } = "清老頭";

        public override int Hansu { get; } = Yakuman;

        public override bool Condition(Te te)
        {
            return te.AllPais().Any(Gates.IsRouTouPai);
        }
    }

    public class RyuISou : Yaku
    {
        public override string Name { get; } = "緑一色";

        public override int Hansu { get; } = Yakuman;

        public override bool Condition(Te te)
        {
            return te.AllPais().Any(Midori.Contains);
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
            return FonpaiList.List.Any(te.HazSpecificKotsu);
        }
    }

    public class SuShiHou : Yaku
    {
        public override string Name { get; } = "四喜和";

        public override int Hansu { get; } = Yakuman;

        public override bool Condition(Te te)
        {
            if (te.Janto[0] is Fonpai)
                return false;

            var fonpais = new List<Pai>();
            fonpais.AddRange(fonpais);

            fonpais.Remove(te.Janto[0]);

            return fonpais.Any(te.HazSpecificKotsu);
        }
    }

    public class TsuIiSou : Yaku
    {
        public override string Name { get; } = "字一色";

        public override int Hansu { get; } = Yakuman;

        public override bool Condition(Te te)
        {
            return te.AllPais().Any(pai => pai is Jihai);
        }
    }

    public class DaiSanGen : Yaku
    {
        public override string Name { get; } = "大三元";

        public override int Hansu { get; } = Yakuman;

        public override bool Condition(Te te)
        {
            return SangenpaiList.List.Any(te.HazSpecificKotsu);
        }
    }

    public class SuAnkou : Yaku
    {
        public override string Name { get; } = "四暗刻";

        public override int Hansu { get; } = Yakuman;

        public override bool Condition(Te te)
        {
            if (new SuAnkouTanki().Condition(te))
                return false; //Upward Compatibility

            if (te.Kotsu.Count != 4)
                return false;

            if (te.Tsumo == false)
            {
                if (te.AgariPai != te.Janto[0])
                    return false;
            }

            return true;
        }
    }

    public class SuAnkouTanki : Yaku
    {
        public override string Name { get; } = "四暗刻単騎待ち";

        public override int Hansu { get; } = DoubleYakuman;

        public override bool Condition(Te te)
        {
            if (te.Kotsu.Count != 4)
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
            if (te.AllPais().Any(pai => pai is Jihai))
                return false;
            
            return te.AllPais().Any(pai => Gates.IsSameType(te.AllPais()[0], pai));
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

            return teSupai.Any(pai => Gates.IsSameType(teSupai[0], pai));
        }
    }
}
