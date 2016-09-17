using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;

namespace Mahjong
{
    public class Houra
    {
        private static readonly int[] Zeros = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

        private static int[] Analyze(List<Pai> pais)
        {
            var n = (int[])Zeros.Clone();

            foreach (var pai in pais)
            {
                var id = pai.Number;

                if (pai is Manzu)
                    id -= 1;
                if (pai is Pinzu)
                    id += 8;
                if (pai is Souzu)
                    id += 17;
                if (pai is Fonpai)
                    id += 26;
                if (pai is Sangenpai)
                    id += 30;

                n[id]++;
            }

            return n;
        }

        private static Pai PaiIdToPai(int id)
        {
            if (id < 9)
                return ManzuList.List.First(pai => pai.Number == id + 1);
            if (id < 18)
                return PinzuList.List.First(pai => pai.Number == id - 8);
            if (id < 27)
                return SouzuList.List.First(pai => pai.Number == id - 17);
            if (id < 31)
                return FonpaiList.List.First(pai => pai.Number == id - 26);
            
            return SangenpaiList.List.First(pai => pai.Number == id - 30);
        }

        private static Mentsu PaiToKotsu(Pai pai)
        {
            return new Mentsu(pai, pai, pai);
        }

        private static Mentsu PaiToShuntsu(Pai pai)
        {
            return new Mentsu(pai, pai.Dora, pai.Dora.Dora);
        }

        public static List<Te> BackTrack(List<Pai> pais)
        {
            var resultTe = new List<Te>();

            var paiCounted = Analyze(pais);

            for (var jan = 0; jan < 34; jan++)
            {
                for (var kotsuFirst = 0; kotsuFirst < 2; kotsuFirst++)
                {
                    var janto = new List<Pai>();
                    var kotsu = new List<Mentsu>();
                    var shuntsu = new List<Mentsu>();

                    var t = (int[])paiCounted.Clone();
                    if (t[jan] < 2)
                        continue;

                    t[jan] -= 2;
                    janto.Add(PaiIdToPai(jan));
                    janto.Add(PaiIdToPai(jan));

                    if (kotsuFirst == 0)
                    {
                        for (var k = 0; k < 34; k++)
                        {
                            if (t[k] < 3)
                                continue;

                            t[k] -= 3;
                            kotsu.Add(PaiToKotsu(PaiIdToPai(k)));
                        }

                        for (var a = 0; a < 3; a++)
                        {
                            for (var b = 0; b < 7;)
                            {
                                if (t[9*a + b] >= 1 &&
                                    t[9*a + b + 1] >= 1 &&
                                    t[9*a + b + 2] >= 1)
                                {
                                    t[9*a + b]--;
                                    t[9*a + b + 1]--;
                                    t[9*a + b + 2]--;
                                    shuntsu.Add(PaiToShuntsu(PaiIdToPai(9*a + b)));
                                }
                                else
                                {
                                    b++;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (var a = 0; a < 3; a++)
                        {
                            for (var b = 0; b < 7;)
                            {
                                if (t[9 * a + b] >= 1 &&
                                    t[9 * a + b + 1] >= 1 &&
                                    t[9 * a + b + 2] >= 1)
                                {
                                    t[9 * a + b]--;
                                    t[9 * a + b + 1]--;
                                    t[9 * a + b + 2]--;
                                    shuntsu.Add(PaiToShuntsu(PaiIdToPai(9 * a + b)));
                                }
                                else
                                {
                                    b++;
                                }
                            }
                        }

                        for (var k = 0; k < 34; k++)
                        {
                            if (t[k] < 3)
                                continue;

                            t[k] -= 3;
                            kotsu.Add(PaiToKotsu(PaiIdToPai(k)));
                        }
                    }

                    if (t.All(count => count == 0))
                    {
                        resultTe.Add(new Te{ Janto = janto, Kotsus = kotsu, Shuntsus = shuntsu });
                    }
                }
            }

            return resultTe;
        }

        public static List<Pai> Sort(List<Pai> pais)
        {
            return pais.OrderBy(pai => pai).ThenBy(pai => pai.Number).ToList();
        }
    }

    public class Te
    {
        public List<Mentsu> Kotsus { get; set; }
        public List<Mentsu> Shuntsus { get; set; }
        public List<Pai> Janto { get; set; }

        public Pai AgariPai { get; set; }
        public bool Tsumo { get; set; }

        public List<Pai> Doras { get; set; } 

        public Fonpai JiFu { get; set; }
        public Fonpai BaFu { get; set; }

        public Te()
        {
            Kotsus = new List<Mentsu>();
            Shuntsus = new List<Mentsu>();
            Janto = new List<Pai>();
            Doras = new List<Pai>();
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

        public void MergeMentsus(Te te)
        {
            if (te == null)
                return;

            Janto = te.Janto;
            Shuntsus.AddRange(te.Shuntsus);
            Kotsus.AddRange(te.Kotsus);
        }

        public bool IsMenzen
        {
            get
            {
                if (Shuntsus.Any(shuntsu => shuntsu.Kui))
                    return false;

                return !Kotsus.Any(kotsu => kotsu.Kui);
            }
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
