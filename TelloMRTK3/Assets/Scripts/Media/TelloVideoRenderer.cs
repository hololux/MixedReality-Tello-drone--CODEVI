using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices.WindowsRuntime;

#if WINDOWS_UWP
using Windows.Media.Playback;
using Windows.Media.Core;
using Windows.Graphics.DirectX.Direct3D11;
using Windows.Graphics.Imaging;
using Microsoft.Graphics.Canvas;
using Windows.Media.MediaProperties;
#endif

namespace Hololux.Tello
{
    public class TelloVideoRenderer : MonoBehaviour
    {
        public RawImage rawImageScreen;
        
        #if WINDOWS_UWP
        private MediaPlayer mediaPlayer;
        private static SoftwareBitmap frameServerDest;
        private Texture2D tex;
        private CanvasDevice canvasDevice;
        private  CanvasBitmap canvasBitmap;
        #endif
        
        private TelloVideoReceiver _telloVideoReceiver;
        private Queue<byte[]> _frames;
        private bool _frameArrivedOnce;
        
        private void Awake()
        {
            _frames = new Queue<byte[]>();
            
            #if WINDOWS_UWP
            frameServerDest = new SoftwareBitmap(BitmapPixelFormat.Rgba8, 720,480,BitmapAlphaMode.Premultiplied );
            tex =  new Texture2D(frameServerDest.PixelWidth, frameServerDest.PixelHeight, UnityEngine.TextureFormat.RGBA32, false);
            canvasDevice = CanvasDevice.GetSharedDevice();
            canvasBitmap = CanvasBitmap.CreateFromSoftwareBitmap(canvasDevice, frameServerDest);
            
            
            mediaPlayer = new MediaPlayer();
            mediaPlayer.IsVideoFrameServerEnabled = true;
            mediaPlayer.VideoFrameAvailable += MediaPlayer_VideoFrameAvailable;
            mediaPlayer.MediaFailed += mediaPlayer_Failed;
            mediaPlayer.MediaOpened += mediaPlayer_Opened;
            //mediaPlayer.RealTimePlayback = true;
            #endif
        }

        private void Start()
        {
            _telloVideoReceiver = new TelloVideoReceiver();
            
            InitializeVideo();
        }
        
        private void Update()
        {
            if (_frames.Count!=0)
            {
                #if WINDOWS_UWP
                tex.LoadRawTextureData(_frames.Dequeue());
                tex.Apply();
                rawImageScreen.texture= tex;
                #endif 
            }
        }
        
        public void StartVideo()
        {
            _telloVideoReceiver.StartListen();
            
            #if WINDOWS_UWP
            mediaPlayer.Play();
            #endif 
            
        }

        public void StopVideo()
        {
            
        }
        
        #if WINDOWS_UWP
        private void MediaPlayer_VideoFrameAvailable(MediaPlayer sender, object args)
        {
            sender.CopyFrameToVideoSurface(canvasBitmap);
            byte[] bytes = canvasBitmap.GetPixelBytes();
            _frames.Enqueue(bytes);

            if(!_frameArrivedOnce)
            {    
                _frameArrivedOnce = true;
            }
         }
        
        private void mediaPlayer_Failed(MediaPlayer sender,MediaPlayerFailedEventArgs args)
        {
             mediaPlayer.Source = null;
             Debug.Log("media faild");
             Debug.Log(args.Error.ToString() );
             Debug.Log(args.ErrorMessage);
        }
        
        private void mediaPlayer_Opened(MediaPlayer sender, object args)
        {
             Debug.Log("frame opened");
        }
        #endif
        
        private void InitializeVideo()
        {
            #if WINDOWS_UWP
            var videoEncodingProperties = VideoEncodingProperties.CreateH264();
            videoEncodingProperties.Height = 720;
            videoEncodingProperties.Width = 960;

            var mediaStreamSource = new MediaStreamSource(new VideoStreamDescriptor(videoEncodingProperties))
            {
                BufferTime = TimeSpan.FromSeconds(0.0),
            };

            mediaStreamSource.SampleRequested += MediaStreamSource_SampleRequested;
            mediaPlayer.Source = MediaSource.CreateFromMediaStreamSource(mediaStreamSource);
            #endif
        }
        
        #if WINDOWS_UWP
        private void MediaStreamSource_SampleRequested(MediaStreamSource sender, MediaStreamSourceSampleRequestedEventArgs args)
        {
            var sample = _telloVideoReceiver.GetSample();

            if (sample != null)
            {
                args.Request.Sample = MediaStreamSample.CreateFromBuffer(sample.Buffer.AsBuffer(), sample.TimeIndex);
                args.Request.Sample.Duration = sample.Duration; //MediaStreamSample.UnknownDuration
            }
        }
        #endif
    }
}
