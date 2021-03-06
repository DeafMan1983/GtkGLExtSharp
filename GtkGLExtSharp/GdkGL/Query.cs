// This file was generated by the Gtk# code generator.
// Any changes made will be lost if regenerated.

namespace GdkGL {

	using System;
	using System.Runtime.InteropServices;

#region Autogenerated code
	public class Query {

		[DllImport("libgdkglext-win32-1.0-0.dll", CallingConvention = CallingConvention.Cdecl)]
		static extern bool gdk_gl_query_version_for_display(IntPtr display, out int major, out int minor);

		public static bool VersionForDisplay(Gdk.Display display, out int major, out int minor) {
			bool raw_ret = gdk_gl_query_version_for_display(display == null ? IntPtr.Zero : display.Handle, out major, out minor);
			bool ret = raw_ret;
			return ret;
		}

		[DllImport("libgdkglext-win32-1.0-0.dll", CallingConvention = CallingConvention.Cdecl)]
		static extern bool gdk_gl_query_version(out int major, out int minor);

		public static bool Version(out int major, out int minor) {
			bool raw_ret = gdk_gl_query_version(out major, out minor);
			bool ret = raw_ret;
			return ret;
		}

		[DllImport("libgdkglext-win32-1.0-0.dll", CallingConvention = CallingConvention.Cdecl)]
		static extern bool gdk_gl_query_gl_extension(IntPtr extension);

		public static bool GlExtension(string extension) {
			IntPtr native_extension = GLib.Marshaller.StringToPtrGStrdup (extension);
			bool raw_ret = gdk_gl_query_gl_extension(native_extension);
			bool ret = raw_ret;
			GLib.Marshaller.Free (native_extension);
			return ret;
		}

		[DllImport("libgdkglext-win32-1.0-0.dll", CallingConvention = CallingConvention.Cdecl)]
		static extern bool gdk_gl_query_extension_for_display(IntPtr display);

		public static bool ExtensionForDisplay(Gdk.Display display) {
			bool raw_ret = gdk_gl_query_extension_for_display(display == null ? IntPtr.Zero : display.Handle);
			bool ret = raw_ret;
			return ret;
		}

		[DllImport("libgdkglext-win32-1.0-0.dll", CallingConvention = CallingConvention.Cdecl)]
		static extern bool gdk_gl_query_extension();

		public static bool Extension() {
			bool raw_ret = gdk_gl_query_extension();
			bool ret = raw_ret;
			return ret;
		}

#endregion
	}
}
