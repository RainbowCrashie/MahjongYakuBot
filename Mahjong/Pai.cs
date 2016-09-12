using System;
using System.Collections.Generic;
using System.Linq;

namespace Mahjong
{
    public abstract class Pai : StringEnum, IComparable
    {
        public int Number { get; }

        protected Pai(string name, int number) : base(name)
        {
            Number = number;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            if (!(obj is Pai))
                throw new ArgumentException();

            return 
                PaiCategoryOrder.List.IndexOf(GetType())*100 + Number
                .CompareTo(
                PaiCategoryOrder.List.IndexOf(obj.GetType())*100 + Number
                );
        }

        protected Pai GetDora(List<Pai> all, int baseNumber)
        {
            var jibun = Number;
            var doranum = baseNumber;

            if (jibun != baseNumber + all.Count - 1)
                doranum += baseNumber - Number + 1;

            return all.Where(p => p.Number == doranum).Select(p => p).First();
        }

        public abstract Pai Dora { get; }

        public bool IsRouTouPai()
            => Number == 1 || Number == 9;

        public bool IsChunChanPai()
            => !IsRouTouPai();
    }

    public abstract class PaiList : List<Pai>
    {
    }

    public class AllPaiList : List<Pai>
    {
        protected AllPaiList()
        {

        }

        public static AllPaiList Create()
        {
            var lst = new AllPaiList();

            foreach (var manzu in ManzuList.List)
            {
                lst.Add(manzu);
            }

            foreach (var souzu in SouzuList.List)
            {
                lst.Add(souzu);
            }

            foreach (var pinzu in PinzuList.List)
            {
                lst.Add(pinzu);
            }

            foreach (var fonpai in FonpaiList.List)
            {
                lst.Add(fonpai);
            }

            foreach (var sangenpai in SangenpaiList.List)
            {
                lst.Add(sangenpai);
            }

            return lst;
        }

        public static AllPaiList List { get; } = Create();
    }

    public abstract class Supai : Pai
    {
        protected Supai(string name, int number) : base(name, number)
        {
        }
    }

    public abstract class Jihai : Pai
    {
        protected Jihai(string name, int number) : base(name, number)
        {
        }
    }

    public class Manzu : Supai
    {
        public static Manzu One { get; } = new Manzu("一萬", 1);
        public static Manzu Two { get; } = new Manzu("二萬", 2);
        public static Manzu Three { get; } = new Manzu("三萬", 3);
        public static Manzu Four { get; } = new Manzu("四萬", 4);
        public static Manzu Five { get; } = new Manzu("五萬", 5);
        public static Manzu Six { get; } = new Manzu("六萬", 6);
        public static Manzu Seven { get; } = new Manzu("七萬", 7);
        public static Manzu Eight { get; } = new Manzu("八萬", 8);
        public static Manzu Nine { get; } = new Manzu("九萬", 9);

        private Manzu(string name, int number) : base(name, number)
        {
        }

        public override Pai Dora
        {
            get { return GetDora(ManzuList.List, 1); }
        }
    }

    public class ManzuList : PaiList
    {
        protected ManzuList()
        {
        }

        public static ManzuList Create()
        {
            var lst = new ManzuList
            {
                Manzu.One,
                Manzu.Two,
                Manzu.Three,
                Manzu.Four,
                Manzu.Five,
                Manzu.Six,
                Manzu.Seven,
                Manzu.Eight,
                Manzu.Nine
            };

            return lst;
        }

        public static ManzuList List { get; } = Create();
    }

    public class Souzu : Supai
    {
        public static Souzu One { get; } = new Souzu("一索", 1);
        public static Souzu Two { get; } = new Souzu("二索", 2);
        public static Souzu Three { get; } = new Souzu("三索", 3);
        public static Souzu Four { get; } = new Souzu("四索", 4);
        public static Souzu Five { get; } = new Souzu("五索", 5);
        public static Souzu Six { get; } = new Souzu("六索", 6);
        public static Souzu Seven { get; } = new Souzu("七索", 7);
        public static Souzu Eight { get; } = new Souzu("八索", 8);
        public static Souzu Nine { get; } = new Souzu("九索", 9);

        private Souzu(string name, int number) : base(name, number)
        {
        }

        public override Pai Dora
        {
            get { return GetDora(SouzuList.List, 1); }
        }
    }

    public class SouzuList : PaiList
    {
        protected SouzuList()
        {
        }

        public static SouzuList Create()
        {
            var lst = new SouzuList
            {
                Souzu.One,
                Souzu.Two,
                Souzu.Three,
                Souzu.Four,
                Souzu.Five,
                Souzu.Six,
                Souzu.Seven,
                Souzu.Eight,
                Souzu.Nine
            };

            return lst;
        }

        public static SouzuList List { get; } = Create();
    }

    public class Pinzu : Supai
    {
        public static Pinzu One { get; } = new Pinzu("一筒", 1);
        public static Pinzu Two { get; } = new Pinzu("二筒", 2);
        public static Pinzu Three { get; } = new Pinzu("三筒", 3);
        public static Pinzu Four { get; } = new Pinzu("四筒", 4);
        public static Pinzu Five { get; } = new Pinzu("五筒", 5);
        public static Pinzu Six { get; } = new Pinzu("六筒", 6);
        public static Pinzu Seven { get; } = new Pinzu("七筒", 7);
        public static Pinzu Eight { get; } = new Pinzu("八筒", 8);
        public static Pinzu Nine { get; } = new Pinzu("九筒", 9);

        private Pinzu(string name, int number) : base(name, number)
        {
        }

        public override Pai Dora
        {
            get { return GetDora(PinzuList.List, 1); }
        }
    }

    public class PinzuList : PaiList
    {
        protected PinzuList()
        {
        }

        public static PinzuList Create()
        {
            var lst = new PinzuList
            {
                Pinzu.One,
                Pinzu.Two,
                Pinzu.Three,
                Pinzu.Four,
                Pinzu.Five,
                Pinzu.Six,
                Pinzu.Seven,
                Pinzu.Eight,
                Pinzu.Nine
            };

            return lst;
        }

        public static PinzuList List { get; } = Create();
    }

    public class Fonpai : Jihai
    {
        public static Fonpai Ton { get; } = new Fonpai("東", 10);
        public static Fonpai Nan { get; } = new Fonpai("南", 11);
        public static Fonpai Sha { get; } = new Fonpai("西", 12);
        public static Fonpai Pe { get; } = new Fonpai("北", 13);

        public Fonpai(string name, int number) : base(name, number)
        {
        }

        public override Pai Dora
        {
            get { return GetDora(FonpaiList.List, 10); }
        }
    }

    public class FonpaiList : PaiList
    {
        protected FonpaiList()
        {
        }

        public static FonpaiList Create()
        {
            var lst = new FonpaiList
            {
                Fonpai.Ton,
                Fonpai.Nan,
                Fonpai.Sha,
                Fonpai.Pe
            };

            return lst;
        }

        public static FonpaiList List { get; } = Create();
    }

    public class Sangenpai : Jihai
    {
        public static Sangenpai Haku { get; } = new Sangenpai("白", 14);
        public static Sangenpai Hatsu { get; } = new Sangenpai("發", 15);
        public static Sangenpai Chun { get; } = new Sangenpai("中", 16);
        
        public Sangenpai(string name, int number) : base(name, number)
        {
        }

        public override Pai Dora
        {
            get { return GetDora(SangenpaiList.List, 14); }
        }
    }

    public class SangenpaiList : PaiList
    {
        protected SangenpaiList()
        {
        }

        public static SangenpaiList Create()
        {
            var lst = new SangenpaiList
            {
                Sangenpai.Haku,
                Sangenpai.Hatsu,
                Sangenpai.Chun
            };

            return lst;
        }

        public static SangenpaiList List { get; } = Create();
    }

    public class PaiCategoryOrder : List<Type>
    {
        protected PaiCategoryOrder()
        {
        }

        public static PaiCategoryOrder Create()
        {
            var lst = new PaiCategoryOrder
            {
                Manzu.One.GetType(),
                Pinzu.One.GetType(),
                Souzu.One.GetType(),
                Fonpai.Ton.GetType(),
                Sangenpai.Haku.GetType()
            };

            return lst;
        }

        public static PaiCategoryOrder List { get; } = Create();
    }

}
