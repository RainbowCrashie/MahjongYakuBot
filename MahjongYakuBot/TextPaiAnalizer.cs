﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Mahjong;

namespace MahjongYakuBot
{
    public class TextPaiAnalizer
    {
        private static readonly char[] PaiSplitterChars = {' ', '　'};
        private static readonly string BaFuPattern = "(?<bafu>.)場";
        private static readonly string JiFuPattern = "(?<jifu>.)家";
        private static readonly string AgariPaiPattern = "((?<agari>ロン|ツモ)(?<agaripai>.+))";
        private static readonly string DoraPattern = "ドラ(?<dora>.+)";
        
        private static readonly DeclaredYaku[] DeclaredYakus =
        {
            new Riichi(), new Ippatsu(), new RinShanKaiHou(),
            new KaiTei(), new HouTei(), new RinShanKaiHou()
        };

        public static AnalizingReturningType Analize(string tweet)
        {
            //宣言系(削除)→場風(削除)→自風(削除)→上がり牌→ドラ→(2つ削除)

            tweet = tweet.Replace("@MahjongYaku", "");

            var ret = new AnalizingReturningType();

            ret.Yakus = DeclaredYakus.Where(yaku => yaku.Aliases.Any(tweet.Contains)).Select(y => y as Yaku).ToList();
            DeclaredYakus.SelectMany(yaku => yaku.Aliases).ToList().ForEach(alias => tweet = tweet.Replace(alias, ""));
            
            ret.Te.BaFu = DeterminePai(Regex.Match(tweet, BaFuPattern).Groups["bafu"].Value) as Fonpai;
            tweet = Regex.Replace(tweet, BaFuPattern, "");

            ret.Te.JiFu = DeterminePai(Regex.Match(tweet, JiFuPattern).Groups["jifu"].Value) as Fonpai;
            tweet = Regex.Replace(tweet, JiFuPattern, "");

            var machedAgari = Regex.Match(tweet, AgariPaiPattern);
            ret.Te.Tsumo = machedAgari.Groups["agari"].Value != "ロン";
            ret.Te.AgariPai = DeterminePai(machedAgari.Groups["agaripai"].Value);

            ret.Te.Doras = Regex.Match(tweet, DoraPattern).Groups["dora"].Value
                .Split(PaiSplitterChars)
                .Select(DeterminePai).ToList();

            tweet = Regex.Replace(tweet, AgariPaiPattern, "");
            tweet = Regex.Replace(tweet, DoraPattern, "");

            //鳴き抽出処理(ぽん明カンちー)[暗カン]

            ret.UnsortedPais = tweet.Split(PaiSplitterChars).Select(DeterminePai).ToList();

            ret.UnsortedPais.RemoveAll(pai => pai == null);

            ret.Te.AgariPai = ret.Te.AgariPai ?? ret.UnsortedPais.Last();

            return ret;
        }

        public static Pai DeterminePai(string rawpai)
        {
            if (rawpai == "")
                return null;

            PaiAliasList.List.ForEach(als => als.Aliases.ToList().ForEach(al => rawpai = rawpai.Replace(al, als.Ideal)));

            return AllPaiList.List.First(pai => rawpai.Contains(pai.Text));
        }
    }

    public class PaiAlias
    {
        public string Ideal { get; }
        public string[] Aliases { get; }

        protected PaiAlias(string ideal, string[] aliases)
        {
            Ideal = ideal;
            Aliases = aliases;
        }

        public static PaiAlias One { get; } = new PaiAlias("一", new[] {"イー", "1"});
        public static PaiAlias Two { get; } = new PaiAlias("二", new[] { "リャン", "2" });
        public static PaiAlias Three { get; } = new PaiAlias("三", new[] { "サン", "3" });
        public static PaiAlias Four { get; } = new PaiAlias("四", new[] { "スー", "4" });
        public static PaiAlias Five { get; } = new PaiAlias("五", new[] { "ウー", "5", "伍" });
        public static PaiAlias Six { get; } = new PaiAlias("六", new[] { "ロー", "6" });
        public static PaiAlias Seven { get; } = new PaiAlias("七", new[] { "チー", "7" });
        public static PaiAlias Eight { get; } = new PaiAlias("八", new[] { "パー", "8" });
        public static PaiAlias Nine { get; } = new PaiAlias("九", new[] { "チュー", "9" });

        public static PaiAlias Sou { get; } = new PaiAlias("索", new[] { "ソウ" });
        public static PaiAlias Pin { get; } = new PaiAlias("筒", new[] { "ピン" });
        public static PaiAlias Man { get; } = new PaiAlias("萬", new[] { "ワン", "マン" });

        public static PaiAlias Haku { get; } = new PaiAlias("白", new[] { "ハク" });
        public static PaiAlias Hatsu { get; } = new PaiAlias("發", new[] { "ハツ" });
        public static PaiAlias Chun { get; } = new PaiAlias("中", new[] { "チュン" });
    }

    public class PaiAliasList : List<PaiAlias>
    {
        protected PaiAliasList()
        {
        }

        public static PaiAliasList Create()
        {
            var lst = new PaiAliasList
            {
                PaiAlias.One,
                PaiAlias.Two,
                PaiAlias.Three,
                PaiAlias.Four,
                PaiAlias.Five,
                PaiAlias.Six,
                PaiAlias.Seven,
                PaiAlias.Eight,
                PaiAlias.Nine,
                PaiAlias.Sou,
                PaiAlias.Pin,
                PaiAlias.Man,
                PaiAlias.Haku,
                PaiAlias.Hatsu,
                PaiAlias.Chun
            };

            return lst;
        }

        public static PaiAliasList List { get; } = Create();
    }

    public class AnalizingReturningType
    {
        public Te Te { get; set; } = new Te();
        public List<Yaku> Yakus { get; set; } = new List<Yaku>();
        public List<Pai> UnsortedPais { get; set; }   = new List<Pai>();
    }
}