using System.Collections.Generic;
using Mahjong;

namespace MahjongYakuBot
{
    public static class Debugging
    {
        public static void Test()
        {
            var tepai = new List<Pai>
            {
                Souzu.One, Souzu.One,
                Manzu.Seven, Manzu.Eight, Manzu.Nine,
                Pinzu.One, Pinzu.One, Pinzu.One,
                Pinzu.Two, Pinzu.Two, Pinzu.Two,
                Pinzu.Three, Pinzu.Three, Pinzu.Three
            };

            var a = WinResult.GetWinResult(tepai);
            ;
        }
    }
}