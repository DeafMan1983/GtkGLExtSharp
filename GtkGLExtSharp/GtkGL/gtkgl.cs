
using GLib;
using Gtk;
using GtkSharp;
using System;
using System.Drawing;
using System.Runtime.InteropServices;


namespace GtkGL {

	public class GlWidget
	{
		private GdkGL.Context	m_shareList = null, m_glContext = null;
		private bool			m_direct;
		private GdkGL.RenderType 	m_renderType;
		private Widget			m_widget = null;
		private GdkGL.GlWindow 	m_glWindow = null;
		
		[DllImport("libgtkglext-win32-1.0-0.dll")]
		static extern bool gtk_widget_set_gl_capability (IntPtr widget,
														 IntPtr glConfig, 
														 IntPtr shareList,
														 bool direct, int render_type);
		[DllImport("libgtkglext-win32-1.0-0.dll")]
		static extern bool gtk_widget_is_gl_capable     (IntPtr widget);
		[DllImport("libgtkglext-win32-1.0-0.dll")]
		static extern IntPtr gtk_widget_get_gl_window (IntPtr raw);
		[DllImport("libgtkglext-win32-1.0-0.dll")]
		static extern IntPtr gtk_widget_get_gl_context (IntPtr raw);
		[DllImport("libgdkglext-win32-1.0-0.dll")]
		static extern bool gdk_gl_drawable_gl_begin (IntPtr drawable, IntPtr context);
		[DllImport("libgdkglext-win32-1.0-0.dll")]
		static extern void gdk_gl_drawable_gl_end (IntPtr drawable);
		[DllImport("libgdkglext-win32-1.0-0.dll")]
		static extern void gdk_gl_drawable_swap_buffers (IntPtr gldrawable);
		[DllImport("libgdkglext-win32-1.0-0.dll")]
		static extern bool gdk_gl_drawable_is_double_buffered (IntPtr gldrawable);
		[DllImport("libgtkglext-win32-1.0-0.dll")]
		static extern IntPtr gtk_widget_get_gl_config (IntPtr o);

		private void SetGlCapability  	(GdkGL.Config glconfig,
										 GdkGL.Context share_list,
										 bool direct,
							             GdkGL.RenderType render_type)
		{
			m_shareList = share_list;
			m_direct = direct;
			m_renderType = render_type;

			if (!gtk_widget_set_gl_capability (m_widget.Handle,
											   glconfig.Handle,
											   share_list == null ? (IntPtr)0 : share_list.Handle, 
			                            		direct,
			                                   (int)render_type))
				throw new Exception ("SetGlCapability");
				
		  	m_widget.AddEvents ((int)Gdk.EventMask.VisibilityNotifyMask);
		  	
			// From GtkGlArea	 
			m_widget.DoubleBuffered = false;
			
			m_widget.Realized += new EventHandler (OnRealize);
		}
		
		private void OnRealize (object o, EventArgs args)
		{
			// GLContext
			IntPtr raw_ret = gtk_widget_get_gl_context (m_widget.Handle);
			if (raw_ret == (IntPtr)0)
				throw new Exception ("gtk_widget_get_gl_context");	
			m_glContext = new GdkGL.Context (raw_ret);
			
	      		IntPtr retvalue = gtk_widget_get_gl_window (m_widget.Handle);
				if (retvalue == (IntPtr)0)
					throw new Exception ("gtk_widget_get_gl_window");
				m_glWindow = new GdkGL.GlWindow (retvalue);

		}

		
		public GlWidget	(Widget widget,
						 GdkGL.Config glconfig,
						 GdkGL.Context share_list,
						 bool direct,
			             GdkGL.RenderType render_type)
		{
			m_widget = widget;
			SetGlCapability (glconfig, share_list, direct, render_type);
		}
		public GlWidget (Widget widget,
						GdkGL.Config glconfig,
                		GdkGL.Context share_list,
						bool direct)
		{
			m_widget = widget;
			SetGlCapability (glconfig, share_list, direct, GdkGL.RenderType.RgbaType);
		}
		public GlWidget (Widget widget,
						GdkGL.Config glconfig,
			            GdkGL.Context share_list)
		{
			m_widget = widget;
			SetGlCapability (glconfig, share_list, true, GdkGL.RenderType.RgbaType);
		}
		public GlWidget (Widget widget, GdkGL.Config glconfig)
		{
			m_widget = widget;
			SetGlCapability (glconfig, null, true, GdkGL.RenderType.RgbaType);
		}


                           
		public bool 		  IsGlCapable ()
		{
			return gtk_widget_is_gl_capable (m_widget.Handle);
		}
 



		public GdkGL.Config  GlConfig
		{
			get {
	      		IntPtr retvalue = gtk_widget_get_gl_config (m_widget.Handle);
				if (retvalue == (IntPtr)0)
					throw new Exception ("gtk_widget_get_gl_config");
				return new GdkGL.Config (retvalue);
			}
    	}

		public 	void	MakeCurrent ()
		{	
			if (!gdk_gl_drawable_gl_begin (m_glWindow.Handle, m_glContext.Handle))
				throw new Exception ("gdk_gl_drawable_gl_begin");
		}
		
		public	void	SwapBuffers ()
		{	
			if (gdk_gl_drawable_is_double_buffered (m_glWindow.Handle))
				gdk_gl_drawable_swap_buffers (m_glWindow.Handle);
			gdk_gl_drawable_gl_end (m_glWindow.Handle);
		}
		
	}
	
	
	public class	Application
	{
		//
		// Disables creation of instances.
		//
		private Application ()
		{
		}
		
		[DllImport("libgtkglext-win32-1.0-0.dll")]
		static extern void gtk_gl_init (ref int argc, ref string[] argv);
		[DllImport("libgtkglext-win32-1.0-0.dll")]
		static extern bool gtk_gl_init_check (ref int argc, ref string[] argv);

		public static void Init (ref string[] args)
		{
			int argc = args.Length;
			gtk_gl_init (ref argc, ref args);
		}

		public static bool InitCheck (ref string[] args)
		{
			int argc = args.Length;
			return gtk_gl_init_check (ref argc, ref args);
		}
	}
	
}





