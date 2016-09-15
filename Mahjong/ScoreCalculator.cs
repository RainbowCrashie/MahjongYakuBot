using System.Linq;

namespace Mahjong
{
    public static class ScoreCalculator
    {
        public static int CountDoras(Te te)
        {
            return te.AllPais().Select(pai => te.Doras.Count(dora => dora.Dora == pai)).Sum();
        }
    }

    public class Score
    {
        public int FuSu { get; set; }
        public int HanSu { get; set; }
        public Yaku Yakus { get; set; }

        public int WinningPoints { get; set; }

        
    }
}