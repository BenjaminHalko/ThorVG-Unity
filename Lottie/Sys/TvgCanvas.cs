using System;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using UnityEngine;

namespace Lottie.Sys
{
    public class TvgCanvas
    {
        private readonly IntPtr _canvas;
        private Texture2D _texture;
        private uint[] _buffer;
        private GCHandle _bufferHandle;

        public uint Width { get; private set; }
        public uint Height { get; private set; }
        public bool Dirty { get; set; }

        // Private
        
        private void FlipBufferVertically()
        {
            for (var y = 0; y < Height / 2; y++)
            {
                var topIndex = y * Width;
                var bottomIndex = (Height - y - 1) * Width;
                for (var x = 0; x < Width; x++)
                {
                    (_buffer[topIndex + x], _buffer[bottomIndex + x]) =
                        (_buffer[bottomIndex + x], _buffer[topIndex + x]);
                }
            }
        }

        private void SetTextureBuffer()
        {
            FlipBufferVertically();
            _texture.SetPixelData(_buffer, 0);
            _texture.Apply();
        }
        
        // Public
        
        public TvgCanvas(uint width, uint height)
        {
            TvgSys.Init();
            _canvas = TvgLib.tvg_swcanvas_create();
            if (width > 0 && height > 0)
                SetSize(width, height);
        }
        
        ~TvgCanvas()
        {
            // TODO: Stop this from crashing
            // TvgSys.Check(TvgLib.tvg_canvas_destroy(_canvas), "Canvas Destroy");

            if (_bufferHandle.IsAllocated)
            {
                _bufferHandle.Free();
            }
        }

        public void AddPaint(TvgPaint paint)
        {
            TvgSys.Check(TvgLib.tvg_canvas_push(_canvas, paint.Handle), "Canvas Push");
        }
        
        public void Update()
        {
            TvgSys.Check(TvgLib.tvg_canvas_update(_canvas), "Canvas Update");
            Dirty = true;
        }
        
        public void Update(TvgPaint paint)
        {
            TvgSys.Check(TvgLib.tvg_canvas_update_paint(_canvas, paint.Handle), "Canvas Update");
            Dirty = true;
        }

        [CanBeNull]
        public Texture2D Texture()
        {
            if (!Dirty) return _texture;
            
            // Draw the canvas
            var result = TvgLib.tvg_canvas_draw(_canvas, true);
            
            if (result != (int)Result.Success)
            {
                return null;
            }
            
            // Sync
            TvgSys.Check(TvgLib.tvg_canvas_sync(_canvas), "Canvas Sync");

            // Get the texture
            SetTextureBuffer();
            
            Dirty = false;

            return _texture;
        }
        
        public void SetSize(uint width, uint height)
        {
            if (Width == width && Height == height) return;
            Width = width;
            Height = height;
            
            // Free the buffer
            if (_bufferHandle.IsAllocated)
                _bufferHandle.Free();
            
            // Create Buffer
            _buffer = new uint[Width * Height];
            _bufferHandle = GCHandle.Alloc(_buffer, GCHandleType.Pinned);
            
            // Create Texture
            _texture = new Texture2D((int)Width, (int)Height, TextureFormat.RGBA32, false);
            
            // Set Target
            TvgSys.Check(TvgLib.tvg_swcanvas_set_target(
                _canvas, _bufferHandle.AddrOfPinnedObject(),
                Width, Width, Height, ColorSpace.Abgr8888), "Canvas Set Target");
            
            // Set Dirty
            Dirty = true;
        }
    }
}
