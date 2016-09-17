using System;
using System.Reactive.Linq;
using CoreTweet.Streaming;
using Twitter;

namespace MahjongYakuBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            TwitterStream.Instance.Streamer.OfType<StatusMessage>().Select(s => s.Status).Where(st => st.Text.Contains("@MahjongYaku"))
                .Subscribe(YakuRequestHandler.Compute);

#if DEBUG
            Debugging.Test();
#endif

            Console.ReadLine();
        }
    }
}
