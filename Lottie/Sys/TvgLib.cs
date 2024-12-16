using System;
using System.Runtime.InteropServices;

namespace Lottie.Sys
{
    internal static class TvgLib
    {
        
/************************************************************************/
/* Engine API                                                           */
/************************************************************************/
        
        [DllImport("thorvg", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tvg_engine_init(TvgEngine engine, int threads);
        
        [DllImport("thorvg", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tvg_engine_term(TvgEngine engine);

        [DllImport("thorvg", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tvg_engine_version(ref uint major, ref uint minor, ref uint micro, ref string version);
        
/************************************************************************/
/* Canvas API                                                         */
/************************************************************************/

        [DllImport("thorvg", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr tvg_swcanvas_create();
        
        [DllImport("thorvg", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tvg_canvas_destroy(IntPtr handle);

        [DllImport("thorvg", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tvg_swcanvas_set_target(IntPtr handle, IntPtr buffer, uint stride, uint w, uint h, ColorSpace colorspace);

        [DllImport("thorvg", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tvg_canvas_push(IntPtr handle, IntPtr paint);
        
        [DllImport("thorvg", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tvg_canvas_update(IntPtr handle);
        
        [DllImport("thorvg", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tvg_canvas_update_paint(IntPtr handle, IntPtr paint);

        [DllImport("thorvg", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tvg_canvas_draw(IntPtr handle, bool clear);
        
        [DllImport("thorvg", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tvg_canvas_sync(IntPtr handle);
        
/************************************************************************/
/* Paint API                                                            */
/************************************************************************/

        [DllImport("thorvg", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tvg_paint_del(IntPtr handle);
        
        [DllImport("thorvg", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tvg_paint_scale(IntPtr handle, float factor);
        
        [DllImport("thorvg", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tvg_paint_get_bounds(IntPtr handle, out float x, out float y, out float w, out float h, bool transform);
        
        [DllImport("thorvg", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tvg_paint_translate(IntPtr handle, float x, float y);
        
/************************************************************************/
/* Picture API                                                          */
/************************************************************************/

        [DllImport("thorvg", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr tvg_picture_create();
        
        [DllImport("thorvg", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tvg_picture_load(IntPtr handle, string path);
        
        [DllImport("thorvg", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tvg_picture_load_data(IntPtr handle, string data, uint size, string mimetype, string rpath, bool copy);
        
        [DllImport("thorvg", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tvg_picture_set_size(IntPtr handle, float w, float h);
        
        [DllImport("thorvg", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tvg_picture_get_size(IntPtr handle, out float w, out float h);
        
/************************************************************************/
/* Animation API                                                        */
/************************************************************************/

        [DllImport("thorvg", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr tvg_animation_new();
        
        [DllImport("thorvg", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tvg_animation_set_frame(IntPtr handle, float frame);
        
        [DllImport("thorvg", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tvg_animation_get_frame(IntPtr handle, out float frame);
        
        [DllImport("thorvg", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tvg_animation_get_total_frame(IntPtr handle, out float totalFrame);
        
        [DllImport("thorvg", CallingConvention = CallingConvention.Cdecl)]
        public static extern int tvg_animation_get_duration(IntPtr handle, out float duration);

        [DllImport("thorvg", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr tvg_animation_get_picture(IntPtr handle);
    }
}