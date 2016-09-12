using System.Collections.Generic;
using System.Linq;

namespace Mahjong
{
    public static class Gates
    {
        public static bool IsJuntsu(Pai pai1, Pai pai2, Pai pai3)
        {
            if (!(pai1 is Supai))
                return false;
            if (!(pai2 is Supai))
                return false;
            if (!(pai3 is Supai))
                return false;

            if (pai1.GetType() != pai2.GetType())
                return false;
            if (pai1.GetType() != pai3.GetType())
                return false;

            List<Pai> pais = new List<Pai> {pai1, pai2, pai3};
            pais = pais.OrderBy(pai => pai.Number).ToList();

            if (pais[1].Number != pais[0].Number + 1)
                return false;

            if (pais[1].Number != pais[2].Number - 1)
                return false;
            
            return true;
        }

        public static bool IsKoutsu(Pai pai1, Pai pai2, Pai pai3)
        {
            if (pai1 != pai2)
                return false;

            if (pai1 != pai3)
                return false;

            return true;
        }

        public static bool IsToitsu(Pai pai1, Pai pai2)
        {
            if (pai1 != pai2)
                return false;

            return true;
        }

        public static bool IsKantsu(Pai pai1, Pai pai2, Pai pai3, Pai pai4)
        {
            if (pai1 != pai2)
                return false;

            if (pai1 != pai3)
                return false;

            if (pai1 != pai4)
                return false;

            return true;
        }
        
        /// <summary>
        /// pinzu, manzu, souzu, sangenpai, fonpai
        /// </summary>
        public static bool IsSameType(Pai pai1, Pai pai2)
            => pai1.GetType() == pai2.GetType();
    }
}
