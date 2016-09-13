using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using CoreTweet.Streaming;
using Mahjong;
using Twitter;

namespace MahjongYakuBot
{
    class Program
    {
        static void Main(string[] args)
        {
            TwitterStream.Instance.Streamer.OfType<StatusMessage>().Select(s => s.Status).Where(st => st.Text.Contains("@MahjongYaku"))
                .Subscribe(YakuRequestHandler.Compute);

            //七索　七索　四萬　五萬　六萬　二筒　三筒　四筒　六筒　七筒　八筒 二索　三索　四索 ツモ八筒 東家　リーチ一発

            //一索　一索　六萬　七萬　八萬　六萬　七萬　八萬　六筒　六筒　六筒　七筒　八筒　九筒　ツモ八筒　東家　立直

            //var l = new List<Pai>
            //{
            //    Souzu.One,
            //    Souzu.One,
            //    Manzu.Six,
            //    Manzu.Seven,
            //    Manzu.Eight,
            //    Manzu.Six,
            //    Manzu.Seven,
            //    Manzu.Eight,
            //    Pinzu.Six,
            //    Pinzu.Six,
            //    Pinzu.Six,
            //    Pinzu.Seven,
            //    Pinzu.Eight,
            //    Pinzu.Nine
            //};

            //var res = Houra.BackTrack(l);

            //l = Houra.Sort(l);

            //var co = Houra.AllCombinationsOfThree(l);

            //var s = co.Where(me => Gates.IsJuntsu(me.Pais[0], me.Pais[1], me.Pais[2])).ToList();

            Console.ReadLine();
        }
    }
}
