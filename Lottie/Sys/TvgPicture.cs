using System;

namespace Lottie.Sys
{
    public class TvgPicture : TvgPaint
    {
        public TvgPicture()
        {
            Handle = TvgLib.tvg_picture_create();
        }
        
        public TvgPicture(IntPtr handle)
        {
            Handle = handle;
        }
        
        public void Load(string data)
        {
            TvgSys.Check(TvgLib.tvg_picture_load(Handle, data), "Picture Load");
        }

        public void LoadData(string data, string type)
        {
            TvgSys.Check(TvgLib.tvg_picture_load_data(Handle, data, (uint)data.Length, type, "", true), "Picture Load");
        }
        
        public void SetSize(float width, float height)
        {
            TvgSys.Check(TvgLib.tvg_picture_set_size(Handle, width, height), "Picture Set Size");
        }
        
        public void GetSize(out float width, out float height)
        {
            TvgSys.Check(TvgLib.tvg_picture_get_size(Handle, out width, out height), "Picture Get Size");
        }
    }
}
