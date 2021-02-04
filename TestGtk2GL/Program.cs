
/*
 * button.c:
 * Simple toggle button example.
 *
 * written by Naofumi Yasufuku  <naofumi@users.sourceforge.net>
*/

/*
 * button.cs:
 * Conversion for GTK# - gtkgl-sharp.
 *
 * converted by Luciano Martorella  <mad_lux_it@users.sourceforge.net>
*/


using Gtk;
using GtkGL;
using System;
using System.Runtime.InteropServices;
using Tao.OpenGl; // Do not forget to install libtao*-cil libtao*-cil-dev for Debian
// exanple: sudo apt install -fy libtao*-cil libtao*-cil-dev
// Make sure install sudo apt install -fy libgtkglext* then you will able GtkSharp 2 Enjoy your development with gaming or drawing :)
// Do not need use OpenTK! PS Shut up, OpenTK! I love original GtkGLExt.

namespace Gtk
{
    class GlToggleButton : ToggleButton
    {
        private GlWidget m_gl = null;
        private float m_angle = 0.0f;
        private float m_pos_y = 0.0f;
        private uint m_timeout_id = 0;

        const uint TIMEOUT_INTERVAL = 10;
        private bool m_animate = true;

        private bool realized = false;

        private const string libGL = "libGL.so.1";

        [DllImport(libGL)]
        private extern static void glEnable(uint enable_flag);

        [DllImport(libGL)]
        private extern static void glViewport(int x, int y, int width, int height);

        [DllImport(libGL)]
        private extern static void glClearColor(float r, float g, float b, float a);

        [DllImport(libGL)]
        private extern static void glClear(uint clear_mask);

        [DllImport(libGL)]
        private extern static void glBegin(uint primitive_mode);

        [DllImport(libGL)]
        private extern static void glEnd();

        [DllImport(libGL)]
        private extern static void glFlush();

        [DllImport(libGL)]
        private extern static void glColor3f(float r, float g, float b);

        [DllImport(libGL)]
        private extern static void glVertex3f(float x, float y, float z);

        // Modern OpenGL
        [DllImport(libGL)]
        private unsafe extern static void glGenVertexArrays(int size, uint* vaos);

        [DllImport(libGL)]
        private unsafe extern static void glGenBuffers(int size, uint* vbos);

        [DllImport(libGL)]
        private extern static void glBindVertexArray(uint array);

        [DllImport(libGL)]
        private unsafe extern static void glBufferData(uint target, int size, void* data, uint usage);

        [DllImport(libGL)]
        private unsafe extern static void glVertexAttribPointer(uint index, int size, uint type, bool normalized, int stride, void* pointer);

        [DllImport(libGL)]
        private extern static void glEnableVertexAttribArray(uint obj);

        [DllImport(libGL)]
        private extern static void glBindBuffer(uint target, uint vbo);

        [DllImport(libGL)]
        private extern static uint glCreateProgram();

        [DllImport(libGL)]
        private extern static uint glCreateShader(uint shader_type);

        [DllImport(libGL)]
        private unsafe extern static void glShaderSource(uint shader_source, int count, IntPtr shader_string, int* length);

        [DllImport(libGL)]
        private extern static void glCompileShader(uint shader_source);

        [DllImport(libGL)]
        private extern static void glAttachShader(uint program, uint shader);

        [DllImport(libGL)]
        private extern static void glLinkProgram(uint program_source);

        [DllImport(libGL)]
        private extern static void glUseProgram(uint program_source);

        [DllImport(libGL)]
        private extern static void glDeleteShader(uint shader_source);

        [DllImport(libGL)]
        private extern static void glDrawArrays(uint primitive_mode, int first, int count);

        public GlToggleButton(GdkGL.Config config)
        {
            // VBox.
            VBox vbox = new VBox(false, 0);
            vbox.BorderWidth = 10;

            // Drawing area for drawing OpenGL scene.
            DrawingArea drawing_area = new DrawingArea();
            drawing_area.SetSizeRequest(200, 200);

            // Set OpenGL-capability to the widget. 
            m_gl = new GlWidget(drawing_area, config);
            drawing_area.Realized += new EventHandler(Realize);
            drawing_area.ConfigureEvent += new ConfigureEventHandler(Configure);
            drawing_area.ExposeEvent += new ExposeEventHandler(Expose);
            drawing_area.Unrealized += new EventHandler(Unrealize);
            drawing_area.VisibilityNotifyEvent += new VisibilityNotifyEventHandler(VisibilityNotify);

            vbox.PackStart(drawing_area, true, true, 0);
            drawing_area.Show();

            // Label.
            Label label = new Label("Toggle Animation");
            vbox.PackStart(label, false, false, 10);
            label.Show();

            Toggled += new EventHandler(ToggleAnimation);

            // Add VBox. 
            vbox.Show();
            Add(vbox);
        }

        private void ToggleAnimation(object o, EventArgs args)
        {
            m_animate = !m_animate;

            if (m_animate)
                TimeoutAdd();
            else
            {
                TimeoutRemove();
                QueueDraw();
            }
        }

        private void VisibilityNotify(object o, VisibilityNotifyEventArgs args)
        {
            if (m_animate)
                if (args.Event.State == Gdk.VisibilityState.FullyObscured)
                    TimeoutRemove();
                else
                    TimeoutAdd();
        }

        private void Realize(object o, EventArgs args)
        {
            float[] ambient = { 0.0f, 0.0f, 0.0f, 1.0f };
            float[] diffuse = { 1.0f, 1.0f, 1.0f, 1.0f };
            float[] position = { 1.0f, 1.0f, 1.0f, 0.0f };
            float[] lmodel_ambient = { 0.2f, 0.2f, 0.2f, 1.0f };
            float[] local_view = { 0.0f };

            // OpenGL BEGIN 
            m_gl.MakeCurrent();

            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_AMBIENT, ambient);
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_DIFFUSE, diffuse);
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, position);
            Gl.glLightModelfv(Gl.GL_LIGHT_MODEL_AMBIENT, lmodel_ambient);
            Gl.glLightModelfv(Gl.GL_LIGHT_MODEL_LOCAL_VIEWER, local_view);
            Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glEnable(Gl.GL_LIGHT0);
            Gl.glEnable(Gl.GL_DEPTH_TEST);

            Gl.glClearColor(1.0f, 0.33f, 0f, 1.0f);
            Gl.glClearDepth(1.0);

            // OpenGL END 
            realized = true;
            ((Widget)o).QueueResize();
        }

        private void Unrealize(object o, EventArgs args)
        {
            TimeoutRemove();
        }

        private void Configure(object o, ConfigureEventArgs args)
        {
            if (!realized)
                return;
            float w = ((Widget)o).Allocation.Width;
            float h = ((Widget)o).Allocation.Height;
            float aspect;

            // OpenGL BEGIN 
            m_gl.MakeCurrent();

            Gl.glViewport(0, 0, (int)w, (int)h);

            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            if (w > h)
            {
                aspect = w / h;
                Gl.glFrustum(-aspect, aspect, -1.0, 1.0, 5.0, 60.0);
            }
            else
            {
                aspect = h / w;
                Gl.glFrustum(-1.0, 1.0, -aspect, aspect, 5.0, 60.0);
            }

            Gl.glMatrixMode(Gl.GL_MODELVIEW);

            // OpenGL END **
        }

        private unsafe void ModernOpenGL()
        {
            const string vertexShaderSource = @"#version 300 es
                precision highp float;
                layout (location = 0) in vec4 Position0;
                layout (location = 1) in vec4 Color0;
                out vec4 color;
                void main()
                {    
                    gl_Position = Position0;    
                    color = Color0;
                }";

            const string fragmentShaderSource = @"#version 300 es
                precision highp float;
                in vec4 color;
                out vec4 fragColor;
                void main() 
                {               
                    fragColor = color;      
                }";

            // Shader
            uint vertexshader = glCreateShader(0x8B31);
            IntPtr* textPtr = stackalloc IntPtr[1];
            int* lengthArray = stackalloc int[1];
            lengthArray[0] = vertexShaderSource.Length;
            textPtr[0] = Marshal.StringToHGlobalAnsi(vertexShaderSource);
            glShaderSource(vertexshader, 1, (IntPtr)textPtr, lengthArray);
            glCompileShader(vertexshader);

            uint fragmentshader = glCreateShader(0x8B30);
            lengthArray[0] = fragmentShaderSource.Length;
            textPtr[0] = Marshal.StringToHGlobalAnsi(fragmentShaderSource);
            glShaderSource(fragmentshader, 1, (IntPtr)textPtr, lengthArray);
            glCompileShader(fragmentshader);

            uint shaderprogram = glCreateProgram();
            glAttachShader(shaderprogram, vertexshader);
            glAttachShader(shaderprogram, fragmentshader);
            glLinkProgram(shaderprogram);

            glDeleteShader(vertexshader);
            glDeleteShader(fragmentshader);

            float[] vertices = new float[]
            {
                // vertices             colors
                0f, 0.5f, 0.0f, 1.0f, 1.0f, 0.0f, 0.0f, 1.0f,
                0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f, 1.0f,
                -0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f
            };

            // VertexArrayObject
            uint vao = 0;
            uint vbo = 0;
            glGenVertexArrays(1, &vao);
            glGenBuffers(1, &vbo);

            glBindVertexArray(vao);
            glBindBuffer(0x8892, vao);

            fixed (float* verticesPtr = &vertices[0])
            {
                glBufferData(0x8892, vertices.Length * sizeof(float), verticesPtr, 0x88E4);
            }

            int stride = 8 * sizeof(float);
            glVertexAttribPointer(0, 4, 0x1406, false, stride, (void*)null);
            glEnableVertexAttribArray(0);

            glVertexAttribPointer(1, 4, 0x1406, false, stride, (void*)16);
            glEnableVertexAttribArray(1);

            glBindBuffer(0x8892, 0);
            glBindVertexArray(vao);

            glUseProgram(shaderprogram);
            glBindVertexArray(vao);
            glDrawArrays(0x0004, 0, 3);
        }

        private void Expose(object o, ExposeEventArgs args)
        {
            // brass 
            float[] ambient = { 0.329412f, 0.223529f, 0.027451f, 1.0f };
            float[] diffuse = { 0.780392f, 0.568627f, 0.113725f, 1.0f };
            float[] specular = { 0.992157f, 0.941176f, 0.807843f, 1.0f };
            float shininess = 0.21794872f * 128.0f;

            // OpenGL BEGIN 
            m_gl.MakeCurrent();

            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

            Gl.glLoadIdentity();
            Gl.glTranslatef(0.0f, 0.0f, -10.0f);

            Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, m_pos_y, 0.0f);
            Gl.glRotatef(m_angle, 0.0f, 1.0f, 0.0f);
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT, ambient);
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_DIFFUSE, diffuse);
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_SPECULAR, specular);
            Gl.glMaterialf(Gl.GL_FRONT, Gl.GL_SHININESS, shininess);

            // Disable because I try to ModernOpenGL that is why. 
            //GdkGL.Draw.Torus(true, 0.5, 1.0, 30, 30);
            Gl.glPopMatrix();

            // Modern OpenGL If you disable ModernOpenGL() than you need remove "//" of GdkGL.Draw....
            ModernOpenGL();

            m_gl.SwapBuffers();

            // OpenGL END 
        }


        private void TimeoutAdd()
        {
            if (m_timeout_id == 0)
            {
                m_timeout_id = Timeout.Add(TIMEOUT_INTERVAL, new Function(timeout));
            }
        }

        private void TimeoutRemove()
        {
            if (m_timeout_id != 0)
            {
                Timeout.Remove(m_timeout_id);
                m_timeout_id = 0;
            }
        }

        private bool timeout()
        {
            m_angle += 3.0f;
            while (m_angle >= 360.0f)
                m_angle -= 360.0f;

            double t = m_angle * Math.PI / 180.0;
            if (t > Math.PI)
                t = 2.0 * Math.PI - t;

            m_pos_y = (float)(2.0 * (Math.Sin(t) + 0.4 * Math.Sin(3.0 * t)) - 1.0);

            QueueDraw();
            return true;
        }
    }
}



// The application class
class MainApp
{
    static int Main(string[] args)
    {
        // Init GTK.
        Gtk.Application.Init();

        // Init GtkGLExt.
        GtkGL.Application.Init(ref args);

        // Configure OpenGL-capable visual.

        // Try double-buffered visual       
        GdkGL.Config glconfig = new GdkGL.Config(GdkGL.ConfigMode.Rgb | GdkGL.ConfigMode.Depth | GdkGL.ConfigMode.Double);

        if (glconfig == null)
        {
            Console.WriteLine("*** Cannot find the double-buffered visual.");
            Console.WriteLine("*** Trying single-buffered visual.");

            // Try single-buffered visual
            glconfig = new GdkGL.Config(GdkGL.ConfigMode.Rgb | GdkGL.ConfigMode.Depth);
            if (glconfig == null)
            {
                Console.WriteLine("*** No appropriate OpenGL-capable visual found.");
                return 1;
            }
        }

        // Top-level window.
        Window window = new Window(WindowType.Toplevel);
        window.Title = "Test GtkGL";

        // Perform the resizes immediately
        window.ResizeMode = ResizeMode.Immediate;

        // Get automatically redrawn if any of their children changed allocation.
        window.ReallocateRedraws = true;

        // Set border width. 
        window.BorderWidth = 10;

        window.DeleteEvent += new DeleteEventHandler(Window_Delete);

        // Toggle button which contains an OpenGL scene.
        GlToggleButton button = new GlToggleButton(glconfig);
        button.Show();

        window.Add(button);

        // Show window.
        window.Show();

        // Main Loop
        Gtk.Application.Run();
        return 0;
    }

    static void Window_Delete(object obj, DeleteEventArgs args)
    {
        Gtk.Application.Quit();
    }
}