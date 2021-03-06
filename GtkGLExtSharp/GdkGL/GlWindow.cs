// This file was generated by the Gtk# code generator.
// Any changes made will be lost if regenerated.

namespace GdkGL {

	using System;
	using System.Collections;
	using System.Runtime.InteropServices;

#region Autogenerated code
	[GlWindow]
	public class GlWindow : Gdk.Drawable {

		[Obsolete]
		protected GlWindow(GLib.GType gtype) : base(gtype) {}
		public GlWindow(IntPtr raw) : base(raw) {}

		[DllImport("libgdkglext-win32-1.0-0.dll", CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr gdk_gl_window_new(IntPtr glconfig, IntPtr window, out int attrib_list);

		public GlWindow (GdkGL.Config glconfig, Gdk.Window window, out int attrib_list) : base (IntPtr.Zero)
		{
			if (GetType () != typeof (GlWindow)) {
				throw new InvalidOperationException ("Can't override this constructor.");
			}
			owned = true;
			Raw = gdk_gl_window_new(glconfig == null ? IntPtr.Zero : glconfig.Handle, window == null ? IntPtr.Zero : window.Handle, out attrib_list);
		}

		[DllImport("libgdkglext-win32-1.0-0.dll", CallingConvention = CallingConvention.Cdecl)]
		static extern void gdk_gl_window_destroy(IntPtr raw);

		public void Destroy() {
			gdk_gl_window_destroy(Handle);
		}

		[DllImport("libgdkglext-win32-1.0-0.dll", CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr gdk_gl_window_get_window(IntPtr raw);

		public Gdk.Window Window { 
			get {
				IntPtr raw_ret = gdk_gl_window_get_window(Handle);
				Gdk.Window ret = GLib.Object.GetObject(raw_ret) as Gdk.Window;
				return ret;
			}
		}

		[DllImport("libgdkglext-win32-1.0-0.dll", CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr gdk_gl_window_get_type();

		static GLib.GType _gtype = new GLib.GType (gdk_gl_window_get_type());
		public static new GLib.GType GType { 
			get {
								return _gtype;
			}
		}


		static GlWindow ()
		{
			GtkSharp.GdkglSharp.ObjectManager.Initialize ();
		}
#endregion
	}

	internal class GlWindowAttribute : GLib.GTypeTypeAttribute {
		[DllImport ("libgdkglext-win32-1.0-0.dll", CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr gdk_gl_window_get_type ();

		private static GLib.GType _gtype = new GLib.GType (gdk_gl_window_get_type ());
		public static GLib.GType GType { get { return _gtype; } }
		public override GLib.GType Type { get { return _gtype; } }

	}
}
