using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MahjongYakuBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var hai = new List<Pai>
            {
                Manzu.One,
                Manzu.One,
                Manzu.One,
                Manzu.Two,
                Manzu.Three,
                Manzu.Four,
                Manzu.Six,
                Manzu.Seven,
                Manzu.Eight,
                Fonpai.Ton,
                Fonpai.Ton,
                Fonpai.Ton,
                Fonpai.Sha,
                Fonpai.Sha
            };

            var yaku = Houra.Hora(hai);

            Console.WriteLine("Janto: ");
            foreach (var pai in yaku.Janto)
            {
                Console.WriteLine(pai.Number);
            }

            Console.WriteLine("Kotsu: ");
            foreach (var pais in yaku.Kotsu)
            {
                foreach (var pai in pais)
                {
                    Console.WriteLine(pai.Number);
                }
            }

            Console.WriteLine("Shuntsu: ");
            foreach (var pais in yaku.Shuntsu)
            {
                foreach (var pai in pais)
                {
                    Console.WriteLine(pai.Number);
                }
            }

            Console.ReadLine();
        }


    }
}
