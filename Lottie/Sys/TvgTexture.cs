using System;
using UnityEngine;

namespace Lottie.Sys
{
    public enum TvgTextureFillType
    {
        Fill,
        Fit,
        Stretch
    }

    public class TvgTexture
    {
        private readonly TvgCanvas _canvas;
        private readonly TvgAnimation _animation;

        public TvgTexture(string data, uint width, uint height, bool isFile=true)
        {
            
            // Load the canvas
            _canvas = new TvgCanvas(width, height);
            
            // Load the animation
            _animation = new TvgAnimation();
            if (isFile)
                _animation.Picture.Load(data);
            else
                _animation.Picture.LoadData(data, "lottie");
            
            // Add the paint
            _canvas.AddPaint(_animation.Picture);
            
            // Resize the picture
            SetSize(width, height);
        }
        
        public void SetSize(uint width, uint height)
        {
            _canvas.SetSize(width, height);
            _animation.Picture.GetSize(out var w, out var h);
            var scale = Math.Min(width / w, height / h);
            w *= scale;
            h *= scale;
            _animation.Picture.SetSize(w, h);
            _animation.Picture.Translate((width - w) / 2, (height - h) / 2);
            _canvas.Update();
        }

        public void Play()
        {
            _animation.Play();
            _canvas.Update();
        }
        
        public Texture2D Texture() => _canvas.Texture();

        public float Frame
        {
            get => _animation.Frame;
            set => _animation.Frame = value;
        }
    }
}