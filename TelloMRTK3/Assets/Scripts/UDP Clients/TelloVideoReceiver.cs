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
        private readonly ConcurrentQueue<VideoBuffer> _samples = new ConcurrentQueue<VideoBuffer>();
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
        
        public void StopListen()
        {
            if (_cancellationTokenSource != null
                && !_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel(false);
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
            }
        }
        
        public VideoBuffer GetSample()
        {
            var wait = default(SpinWait);
            VideoBuffer result;

            while (_samples.Count == 0 || !_samples.TryDequeue(out result))
            {
                wait.SpinOnce();
            }

            return result;
        }
        
        private async Task Listen(CancellationToken cancellationToken)
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
                            var buffer = new VideoBuffer(connect.Buffer, _timeIndex, _watch.Elapsed - _timeIndex);
                            
                            if (!_watch.IsRunning)
                            {
                                _watch.Start();
                            }

                            _samples.Enqueue(buffer);
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
    
    
    public sealed class VideoBuffer
    {
        public byte[] Buffer { get; }

        public TimeSpan TimeIndex { get; }

        public TimeSpan Duration { get; }

        public int Length => this.Buffer.Length;

        public VideoBuffer()
            : this(
                Array.Empty<byte>(),
                TimeSpan.FromSeconds(0),
                TimeSpan.FromSeconds(0))
        {
        }

        public VideoBuffer(byte[] buffer, TimeSpan timeIndex, TimeSpan duration)
        {
            Buffer = buffer;
            TimeIndex = timeIndex;
            Duration = duration;
        }
    }
}
