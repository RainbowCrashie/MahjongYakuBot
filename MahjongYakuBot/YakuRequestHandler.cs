using System;
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

                var scoring = WinResult.GetWinResult(pail.UnsortedPais, pail.Te, pail.PredeclaredYakus);

                TwitterStream.Instance.Reply(scoring.ToString(), tweet);
            }
            catch (Exception exc)
            {
            }
        }
    }
}