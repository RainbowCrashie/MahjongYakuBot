using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.Remoting.Messaging;

namespace Mahjong
{
    public class WinResult : IComparable
    {
        #region fields
        private const int FuTei = 20;
        private const int KotsuFuBase = 2;
        private const int JantoFuBonus = 2;
        private const int WaitingBonus = 2;

        private const int FuCeiling = 10;

        private const int ChildBonus = 4;
        private const int DealerBonus = 6;
        private const int YakuPointBase = 2;
        private const int Bazoro = 2;

        private const int PointCeiling = 100;

        //public class Scoring
        //{
        //    public enum ManganPoints
        //    {
        //        満貫 = 8000,
        //        跳満 = 12000,
        //        倍満 = 16000,
        //        三倍満 = 24000,
        //        数え役満 = 32000,
        //        役満 = 32000,
        //        ダブル役満 = 64000,
        //        トリプル役満 = 96000
        //    }

        //    private readonly int _notManganPoints;
        //    public ManganPoints Mangan { get; }

        //    public Scoring(int points)
        //    {
        //        _notManganPoints = points;
        //        Mangan = 0;
        //    }

        //    public Scoring(ManganPoints mangan)
        //    {
        //        Mangan = mangan;
        //    }

        //    public int Points(bool isDealer = false)
        //    {
        //        if (Mangan == 0)
        //            return _notManganPoints;

        //        return (int)((int)Mangan * (isDealer ? ManganPointDealerModifier : 1));
        //    }
        //}

        public class Scoring
        {
            public enum Manganese
            {
                満貫 = 8000,
                跳満 = 12000,
                倍満 = 16000,
                三倍満 = 24000,
                数え役満 = 32000,
                役満 = 32000,
                ダブル役満 = 64000,
                トリプル役満 = 96000
            }

            private readonly int _notManganPoints;
            public Manganese Mangan { get; }
            
            public Scoring(int points)
            {
                _notManganPoints = points;
                Mangan = 0;
            }

            public Scoring(Manganese mangan)
            {
                Mangan = mangan;
            }

            public int Points(bool isDealer = false)
            {
                if (Mangan == 0)
                    return _notManganPoints;

                return (int)((int)Mangan * (isDealer ? ManganPointDealerModifier : 1));
            }

            public string NameIfMangan
            {
                get
                {
                    if (Mangan == 0)
                        return "";

                    return $"{Mangan}";
                }
            }
        }

        private const double ManganPointDealerModifier = 1.5;
        #endregion

        #region properties
        public Te Te { get; }

        public bool IsDealer
        {
            get
            {
                return Te.JiFu == Fonpai.Ton;
            }
        }

        private List<Yaku> _yakus;
        public List<Yaku> Yakus
        {
            get { return _yakus = _yakus ?? new List<Yaku>(); }
        }

        public int DoraCount { get; }

        public int HanSu { get; }

        public int FuSu { get; }
        
        public Scoring PointWithManganData { get; }

        public int WinningPoints
        {
            get { return PointWithManganData.Points(IsDealer); }
        }
        
        #endregion

        #region ctors
        public WinResult(Te te, IEnumerable<DeclaredYaku> preDeclaredYakus = null)
        {
            Te = te;

            if (preDeclaredYakus != null)
                Yakus.AddRange(preDeclaredYakus);
            Yakus.AddRange(CheckYakus(te));

            DoraCount = CountDoras(te);

            HanSu = CountHansu(Yakus, te.IsMenzen);

            if (HanSu != 0)
                HanSu += DoraCount;

            FuSu = CalculateFuSu(
                te, te.AgariPai,
                Yakus.Any(yaku => yaku is PinFu),
                Yakus.Any(yaku => yaku is ChiToiTsu));
            
            PointWithManganData = CalculatePoints(FuSu, HanSu, IsDealer);
        }

        public static WinResult GetWinResult(Te te, IEnumerable<DeclaredYaku> preDeclaredYakus = null)
        {
            return new WinResult(te, preDeclaredYakus);
        }

        /// <summary>
        /// Returns 
        /// </summary>
        public static WinResult GetWinResult(List<Pai> unsortedPais, Te preAnalizedMentsus = null,
            IEnumerable<DeclaredYaku> preDeclaredYakus = null)
        {
            var backTracked = Houra.BackTrack(unsortedPais);

            //chitoi

            //kokushi

            var winResults = new List<WinResult>();
            foreach (var te in backTracked)
            {
                var tenten = new Te();

                tenten.MergeMentsus(preAnalizedMentsus);
                tenten.MergeMentsus(te);

                tenten.Tsumo = true;
                tenten.AgariPai = tenten.AllPais().Last();
                if (preAnalizedMentsus != null)
                {
                    tenten.Tsumo = preAnalizedMentsus.Tsumo;
                    tenten.JiFu = preAnalizedMentsus.JiFu;
                    tenten.BaFu = preAnalizedMentsus.BaFu;
                    tenten.Doras = preAnalizedMentsus.Doras;
                    tenten.AgariPai = preAnalizedMentsus.AgariPai;
                }
                
                winResults.Add(GetWinResult(tenten, preDeclaredYakus));
            }
            return winResults.Max();
        }
        #endregion

        #region methods

        public static int CountDoras(Te te)
        {
            return te.AllPais().Select(pai => te.Doras.Count(dora => dora.Dora == pai)).Sum();
        }

        public static int CountHansu(List<Yaku> yakus, bool isMenzen)
        {
            return isMenzen
                ? yakus.Select(yaku => yaku.Hansu).Sum()
                : yakus.Where(y => y.KuiSagari).Select(yaku => yaku.Hansu - 1).Sum() +
                    yakus.Where(y => !y.KuiSagari).Select(yaku => yaku.Hansu).Sum();
        }

        public static List<Yaku> CheckYakus(Te te)
        {
            var yakumans = new List<Yaku>();
            yakumans.AddRange(YakumanList.List);

            yakumans.RemoveAll(yaku => !yaku.Condition(te));

            if (yakumans.Count > 0)
                return yakumans;

            var yakus = new List<Yaku>();
            yakus.AddRange(YakuList.List);

            yakus.RemoveAll(yaku => !yaku.Condition(te));

            return yakus;
        }

        public static int CalculateFuSu(Te te, Pai agariPai, bool isPinfu, bool isChiToiTsu)
        {
            if (isChiToiTsu)
                return 25;

            var fu = FuTei;


            if (te.Tsumo && !isPinfu)
                fu += 2;
            if (te.IsMenzen && !te.Tsumo)
                fu += 10;


            Func<Mentsu, int> kotsuFu = kotsu =>
            {
                var power = 1;

                if (Gates.IsKantsu(kotsu.Pais))
                    power += 2;

                if (!kotsu.Kui)
                    power += 1;

                var yaochupower = kotsu.Pais[0].IsChunChanPai() ? 1 : 2;

                return (int) Math.Pow(KotsuFuBase, power)*yaochupower;
            };

            fu += te.Kotsus.Select(kotsu => kotsuFu(kotsu)).Sum();


            //Official RenFonPai bonus: 4.
            //Tes unspecified jifu and bafu will not be bonused, not considerating them as Ton   
            if (te.Janto[0] == te.JiFu)
                fu += JantoFuBonus;

            if (te.Janto[0] == te.BaFu)
                fu += JantoFuBonus;

            if (te.Janto[0] is Sangenpai)
                fu += JantoFuBonus;


            if (agariPai == te.Janto[0])
            {
                fu += WaitingBonus;
            }
            else if(agariPai is Supai)
            {
                if (agariPai.Number == 3 &&
                    te.Shuntsus.Select(s => s.Pais.Min()).Any(shuntsu => shuntsu.Number ==1))
                {
                    fu += WaitingBonus;
                }
                else if(agariPai.Number == 7 &&
                    te.Shuntsus.Select(s => s.Pais.Min()).Any(shuntsu => shuntsu.Number == 9))
                {
                    fu += WaitingBonus;
                }
                else if(te.Shuntsus.Select(s => s.Pais[1]).Any(shuntsu => shuntsu == agariPai))
                {
                    fu += WaitingBonus;
                }
            }

            return Ceiling(fu, FuCeiling);
        }

        public static Scoring CalculatePoints(int fu, int han, bool isDealer)
        {
            if (han >= 300)
                return new Scoring(Scoring.Manganese.トリプル役満);

            if (han >= 200)
                return new Scoring(Scoring.Manganese.ダブル役満);

            if (han >= 100)
                return new Scoring(Scoring.Manganese.役満);

            if (han >= 13)
                return new Scoring(Scoring.Manganese.数え役満);

            if (han >= 11)
                return new Scoring(Scoring.Manganese.三倍満);

            if (han >= 8)
                return new Scoring(Scoring.Manganese.倍満);

            if (han >= 6)
                return new Scoring(Scoring.Manganese.跳満);

            if (han == 5)
                return new Scoring(Scoring.Manganese.満貫);

            if (han == 4 && fu >= 32)
                return new Scoring(Scoring.Manganese.満貫);

            if (han == 3 && fu >= 62)
                return new Scoring(Scoring.Manganese.満貫);

            return new Scoring(Ceiling(
                fu*
                (int) Math.Pow(YakuPointBase, han + Bazoro)*
                (isDealer ? DealerBonus : ChildBonus),
                PointCeiling));
        }

        public static int Ceiling(int number, int ceilTo)
        {
            return ((int)(number / ceilTo)) * ceilTo;
        }

        public override string ToString()
        {
            if (HanSu == 0)
                return "役なし";

            var doraFormat = "";

            if (DoraCount == 1)
                doraFormat = "ドラ ";

            if (DoraCount == 2)
                doraFormat = "ドラドラ ";

            if (DoraCount >= 3)
                doraFormat = $"ドラ{DoraCount}";

            return $"{string.Join(" ", Yakus.Select(y => y.Name))} {doraFormat} " + Environment.NewLine +
                   $"{(HanSu < 100 ? $"{FuSu}符{HanSu}翻" : "")} {PointWithManganData.NameIfMangan} {WinningPoints}点";
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            if (!(obj is WinResult))
                throw new ArgumentException();

            return
                WinningPoints
                .CompareTo(
                ((WinResult)obj).WinningPoints
                );
        }
        #endregion
    }
}