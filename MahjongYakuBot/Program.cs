using System;
using System.Collections.Generic;

namespace MahjongYakuBot
{
    class Program
    {
        static void Main(string[] args)
        {
            //    var hai = new List<Pai>
            //    {
            //        Manzu.One,
            //        Manzu.One,
            //        Manzu.One,
            //        Manzu.Two,
            //        Manzu.Three,
            //        Manzu.Four,
            //        Manzu.Six,
            //        Manzu.Seven,
            //        Manzu.Eight,
            //        Fonpai.Ton,
            //        Fonpai.Ton,
            //        Fonpai.Ton,
            //        Fonpai.Sha,
            //        Fonpai.Sha
            //    };

            //    var yaku = Houra.Hora(hai);

            //    Console.WriteLine("Janto: ");
            //    foreach (var pai in yaku.Janto)
            //    {
            //        Console.WriteLine(pai.Number);
            //    }

            //    Console.WriteLine("Kotsu: ");
            //    foreach (var pais in yaku.Kotsus)
            //    {
            //        foreach (var pai in pais.Pais)
            //        {
            //            Console.WriteLine(pai.Number);
            //        }
            //    }

            //    Console.WriteLine("Shuntsu: ");
            //    foreach (var pais in yaku.Shuntsus)
            //    {
            //        foreach (var pai in pais.Pais)
            //        {
            //            Console.WriteLine(pai.Number);
            //        }
            //    }

            var te = new Te();
            te.Janto = new List<Pai> { Manzu.Eight, Manzu.Eight };
            te.Shuntsus.Add(new Mentsu(Manzu.Two, Manzu.Three, Manzu.Four));
            te.Shuntsus.Add(new Mentsu(Manzu.Five, Manzu.Six, Manzu.Seven));
            te.Kotsus.Add(new Mentsu(Manzu.One, Manzu.One, Manzu.One));
            te.Kotsus.Add(new Mentsu(Manzu.Nine, Manzu.Nine, Manzu.Nine));
            te.AgariPai = Manzu.One;
            te.Tsumo = true;

            Console.WriteLine(new JunSeiChuRen().Condition(te));
            

            Console.ReadLine();

        }


    }
}
