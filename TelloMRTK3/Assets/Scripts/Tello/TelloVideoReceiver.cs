using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace Hololux.Tello
{
    class TelloVideoReceiver
    {
        private readonly int LocalPort = 11111;

        private readonly ConcurrentQueue<VideoSample> _samples = new ConcurrentQueue<VideoSample>();
        private TimeSpan _timeIndex = TimeSpan.FromSeconds(0);
        private readonly Stopwatch _watch = new Stopwatch();
        private CancellationTokenSource _cancellationTokenSource = null;
        
        public async void StartListen()
        {
            if (_cancellationTokenSource == null)
            {
                _cancellationTokenSource = new CancellationTokenSource();
                await Task.Run(async () => await Listen(this._cancellationTokenSource.Token));
            }
        }

        public VideoSample GetSample()
        {
            var wait = default(SpinWait);
            VideoSample result;

            while (_samples.Count == 0 || !_samples.TryDequeue(out result))
            {
                wait.SpinOnce();
            }

            return result;
        }
        
        protected async Task Listen(CancellationToken cancellationToken)
        {
            if (cancellationToken == null)
            {
                throw new ArgumentNullException(nameof(cancellationToken));
            }

            var wait = default(SpinWait);

            using (var client = new UdpClient(LocalPort))
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        if (client.Available > 0)
                        {
                            var connect = await client.ReceiveAsync();
                            _timeIndex = _watch.Elapsed;
                            var sample = new VideoSample(connect.Buffer, _timeIndex, _watch.Elapsed - _timeIndex);
                            
                            UnityEngine.Debug.Log("Write line");

                            if (!_watch.IsRunning)
                            {
                                _watch.Start();
                            }

                            _samples.Enqueue(sample);
                        }
                        else
                        {
                            wait.SpinOnce();
                        }
                    }
                    catch (Exception ex)
                    {
                        //Debug.L();
                    }
                }
            }
        }
    }
    
    
    public sealed class VideoSample
    {
        public byte[] Buffer { get; }

        public TimeSpan TimeIndex { get; }

        public TimeSpan Duration { get; }

        public int Length => this.Buffer.Length;

        public VideoSample()
            : this(
                Array.Empty<byte>(),
                TimeSpan.FromSeconds(0),
                TimeSpan.FromSeconds(0))
        {
        }

        public VideoSample(byte[] buffer, TimeSpan timeIndex, TimeSpan duration)
        {
            Buffer = buffer;
            TimeIndex = timeIndex;
            Duration = duration;
        }
    }
}
