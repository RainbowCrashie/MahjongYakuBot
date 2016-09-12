using System.Collections.Generic;
using System.Linq;

namespace Mahjong
{
    public class Houra
    {
        
        public static Te Hora(List<Pai> te)
        {
            te = Sort(te);

            var resultTe = new Te();
            
            for (int jan = 0; jan < 13; jan++)
            {
                for (int kotsuFirst = 0; kotsuFirst < 2; kotsuFirst++)
                {
                    var janto = new List<Pai>();
                    var kotsu = new List<Mentsu>();
                    var shuntsu = new List<Mentsu>();
                    
                    var possibleYaku = new List<Pai>();
                    possibleYaku.AddRange(te);
                    
                    if(!Gates.IsToitsu(possibleYaku[jan], possibleYaku[jan + 1]))
                        break;

                    janto.Add(possibleYaku[jan]);
                    janto.Add(possibleYaku[jan + 1]);

                    possibleYaku.RemoveAt(jan);
                    possibleYaku.RemoveAt(jan);

                    if (kotsuFirst == 0)
                    {
                        SearchKotsu(possibleYaku, kotsu);

                        SearchShuntsu(possibleYaku, shuntsu);
                    }
                    else
                    {
                        SearchShuntsu(possibleYaku, shuntsu);

                        SearchKotsu(possibleYaku, kotsu);
                    }

                    if (possibleYaku.Count != 0)
                        continue;

                    resultTe.Janto = janto;
                    resultTe.Kotsus = kotsu;
                    resultTe.Shuntsus = shuntsu;
                }
            }

            return resultTe;
        }
        
        public static void SearchKotsu(List<Pai> possibleYaku, List<Mentsu> kotsu)
        {
            for (int kou = 0; kou < possibleYaku.Count;)
            {
                if (!Gates.IsKoutsu(possibleYaku[kou], possibleYaku[kou + 1], possibleYaku[kou + 2]))
                {
                    kou += 3;
                    continue;
                }

                kotsu.Add(new Mentsu(possibleYaku[kou], possibleYaku[kou + 1], possibleYaku[kou + 2]));

                possibleYaku.RemoveAt(kou);
                possibleYaku.RemoveAt(kou);
                possibleYaku.RemoveAt(kou);
            }
        }

        public static void SearchShuntsu(List<Pai> possibleYaku, List<Mentsu> shuntsu)
        {
            for (int jun = 0; jun < possibleYaku.Count;)
            {
                if (!Gates.IsJuntsu(possibleYaku[jun], possibleYaku[jun + 1], possibleYaku[jun + 2]))
                {
                    jun += 3;
                    continue;
                }

                shuntsu.Add(new Mentsu(possibleYaku[jun], possibleYaku[jun + 1], possibleYaku[jun + 2]));

                possibleYaku.RemoveAt(jun);
                possibleYaku.RemoveAt(jun);
                possibleYaku.RemoveAt(jun);
            }
        }

        public static List<Pai> Sort(List<Pai> pais)
        {
            return pais.OrderBy(pai => pai).ToList();
        }
        
    }

    public class Te
    {
        public List<Mentsu> Kotsus { get; set; }
        public List<Mentsu> Shuntsus { get; set; }
        public List<Pai> Janto { get; set; }
        public Pai AgariPai { get; set; }
        public bool Tsumo { get; set; }

        public Fonpai JiFu { get; set; }
        public Fonpai BaFu { get; set; }

        public Te()
        {
            Kotsus = new List<Mentsu>();
            Shuntsus = new List<Mentsu>();
            Janto = new List<Pai>();
        }

        public List<Pai> AllPais()
        {
            var pais = new List<Pai>();
            pais.AddRange(Janto);

            foreach (var pailist in Shuntsus)
            {
                pais.AddRange(pailist.Pais);
            }

            foreach (var pailist in Kotsus)
            {
                pais.AddRange(pailist.Pais);
            }

            pais.RemoveAll(pai => pai == null);

            return pais;
        }

        public bool IsMenzen()
        {
            if (Shuntsus.Any(shuntsu => shuntsu.Kui))
                return false;

            return !Kotsus.Any(kotsu => kotsu.Kui);
        }
    }

    public class Mentsu
    {
        public List<Pai> Pais { get; set; }
        public bool Kui { get; set; }

        public Mentsu()
        {
            Pais = new List<Pai>();
            Kui = false;
        }

        public Mentsu(List<Pai> pais, bool kui = false)
        {
            Pais = pais;
            Kui = kui;
        }

        public Mentsu(Pai pai1, Pai pai2, Pai pai3, Pai pai4 = null, bool kui = false)
            : this(new List<Pai> {pai1, pai2, pai3, pai4}, kui)
        {
        }
    }
}
