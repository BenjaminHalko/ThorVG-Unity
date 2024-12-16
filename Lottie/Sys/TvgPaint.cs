using System;

namespace Lottie.Sys
{
    public abstract class TvgPaint
    {
        private IntPtr _handle;
        private bool _manuallyCreated = true;
        
        public IntPtr Handle
        {
            get => _handle;
            internal set
            {
                _handle = value;
                _manuallyCreated = false;
            }
        }
        
        ~TvgPaint()
        {
            if (!_manuallyCreated && TvgSys.Initialized)
                TvgLib.tvg_paint_del(_handle);
        }
        
        public void Scale(float factor)
        {
            TvgSys.Check(TvgLib.tvg_paint_scale(_handle, factor), "Paint Scale");
        }
        
        public void Bounds(out float x, out float y, out float w, out float h, bool transform = false)
        {
            TvgSys.Check(TvgLib.tvg_paint_get_bounds(_handle, out x, out y, out w, out h, transform), "Paint Bounds");
        }
        
        public void Translate(float x, float y)
        {
            TvgSys.Check(TvgLib.tvg_paint_translate(_handle, x, y), "Paint Translate");
        }
    }
}