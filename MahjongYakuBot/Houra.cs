using System.Collections.Generic;
using System.Linq;

namespace MahjongYakuBot
{
    public class Houra
    {
        public static Te Hora(List<Pai> te)
        {
            var resultTe = new Te();
            te = Sort(te);
            
            for (int jan = 0; jan < 13; jan++)
            {
                for (int kotsuFirst = 0; kotsuFirst < 2; kotsuFirst++)
                {
                    var janto = new List<Pai>();
                    var kotsu = new List<List<Pai>>();
                    var shuntsu = new List<List<Pai>>();

                    //Copy List
                    var possibleYaku = new List<Pai>();
                    foreach (var pai in te)
                    {
                        possibleYaku.Add(
                            AllPaiList.List.Where(p => p.Number == pai.Number).First());
                    }
                    
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
                    resultTe.Kotsu = kotsu;
                    resultTe.Shuntsu = shuntsu;
                }
            }

            return resultTe;
        }

        public static void SearchKotsu(List<Pai> possibleYaku, List<List<Pai>> kotsu)
        {
            for (int kou = 0; kou < possibleYaku.Count;)
            {
                if (!Gates.IsKoutsu(possibleYaku[kou], possibleYaku[kou + 1], possibleYaku[kou + 2]))
                {
                    kou += 3;
                    continue;
                }

                kotsu.Add(new List<Pai> { possibleYaku[kou], possibleYaku[kou + 1], possibleYaku[kou + 2] });

                possibleYaku.RemoveAt(kou);
                possibleYaku.RemoveAt(kou);
                possibleYaku.RemoveAt(kou);
            }
        }

        public static void SearchShuntsu(List<Pai> possibleYaku, List<List<Pai>> shuntsu)
        {
            for (int jun = 0; jun < possibleYaku.Count;)
            {
                if (!Gates.IsJuntsu(possibleYaku[jun], possibleYaku[jun + 1], possibleYaku[jun + 2]))
                {
                    jun += 3;
                    continue;
                }

                shuntsu.Add(new List<Pai> { possibleYaku[jun], possibleYaku[jun + 1], possibleYaku[jun + 2] });

                possibleYaku.RemoveAt(jun);
                possibleYaku.RemoveAt(jun);
                possibleYaku.RemoveAt(jun);
            }
        }

        public static List<Pai> Sort(List<Pai> pais)
        {
            return pais.OrderBy(pai => pai).ThenBy(pai => pai.Number).ToList();
        }
        
    }

    public class Te
    {
        public List<List<Pai>> Kotsu { get; set; }
        public List<List<Pai>> Shuntsu { get; set; }
        public List<Pai> Janto { get; set; }
        public Pai AgariPai { get; set; }
        public bool Tsumo { get; set; }

        public Te()
        {
            Kotsu = new List<List<Pai>>();
            Shuntsu = new List<List<Pai>>();
            Janto = new List<Pai>();
        }

        public List<Pai> AllPais()
        {
            var pais = new List<Pai>();
            pais.AddRange(Janto);

            foreach (var pailist in Shuntsu)
            {
                pais.AddRange(pailist);
            }

            foreach (var pailist in Kotsu)
            {
                pais.AddRange(pailist);
            }

            return pais;
        }

        public bool HazSpecificKotsu(Pai pai)
        {
            foreach (var pais in Kotsu)
            {
                if(pais[0] == pai)
                    return true;
            }

            return false;
        }
    }
}
