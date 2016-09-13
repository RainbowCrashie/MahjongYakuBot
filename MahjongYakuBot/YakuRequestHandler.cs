﻿using System;
using System.Linq;
using CoreTweet;
using Mahjong;
using Twitter;

namespace MahjongYakuBot
{
    public class YakuRequestHandler
    {
        public static void Compute(Status tweet)
        {
            try
            {
                var pail = TextPaiAnalizer.Analize(tweet.Text);

                var sorted = Houra.BackTrack(pail.UnsortedPais);

                //sorted contains nothing then noten

                pail.Te.AddMentsus(sorted[0]);

                var yakus = Houra.CountYaku(pail.Te);

                pail.Yakus.AddRange(yakus);

                pail.Yakus.ForEach(yaku => Console.WriteLine(yaku.Name));

                var text = "";

                foreach (var yaku in pail.Yakus)
                {
                    text = text + $"{yaku.Name} ";
                }

                var doraCount = 0;
                foreach (var pai in pail.Te.Doras)
                {
                    doraCount += pail.Te.AllPais().Count(p => p == pai.Dora);
                }

                if (doraCount == 1)
                    text = text + "ドラ ";

                if (doraCount == 2)
                    text = text + "ドラドラ ";

                if (doraCount >= 3)
                    text = text + $"ドラ{doraCount}";

                TwitterStream.Instance.Reply(text, tweet);
            }
            catch (Exception exc)
            {
                
            }
        }
    }
}