using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CoreTweet;
using CoreTweet.Streaming;

namespace Twitter
{
    public sealed class TwitterStream
    {
        #region Singleton stuff
        private static readonly Lazy<TwitterStream> lazy = new Lazy<TwitterStream>(() => new TwitterStream());
        public static TwitterStream Instance { get; } = lazy.Value;
        #endregion 

        private Tokens Token { get; } = ApiKey.Api();

        public IConnectableObservable<StreamingMessage> Streamer { get; }

        private IDisposable Connection { get; set; }

        private TwitterStream()
        {
            Streamer = Token.Streaming.UserAsObservable().Publish();
            Connect();
        }

        public void Connect()
        {
            Connection = Streamer.Connect();
        }

        public void Disconnect()
        {
            Connection.Dispose();
        }
    }
}