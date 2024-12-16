using System;
using UnityEngine;

namespace Lottie.Sys
{
    public class TvgAnimation
    {
        private readonly IntPtr _handle;

        public TvgAnimation()
        {
            _handle = TvgLib.tvg_animation_new();
            Picture = new TvgPicture(TvgLib.tvg_animation_get_picture(_handle));
        }

        public TvgPicture Picture { get; }

        public float Frame
        {
            get
            {
                TvgLib.tvg_animation_get_frame(_handle, out var frame);
                return frame;
            }
            set => TvgLib.tvg_animation_set_frame(_handle, value);
        }
        
        public float TotalFrame => TvgLib.tvg_animation_get_total_frame(_handle);
        
        public float Duration => TvgLib.tvg_animation_get_duration(_handle);

        public void Play()
        {
            var frame = Frame;
            frame += Time.deltaTime / Duration * TotalFrame;
            if (frame >= TotalFrame-1)
                frame -= TotalFrame;
            Frame = frame;
        }
    }
}
