using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Game
{
    public partial class GameWindow : Form
    {
        private bool loaded;

        public GameWindow()
        {
            InitializeComponent();
            
        }

        private void RenderScene(object sender, PaintEventArgs e)
        {
            if (!this.loaded) // Play nice
            {
                return;
            }
             try
                        {
            this.glControl.MakeCurrent();
            GL.ClearColor(new Color4(0,0x20,0x40,0));
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            GL.Flush();
            this.glControl.SwapBuffers();
                        }
             catch (Exception ex)
             {
                 this.glControl.Visible = false;
                 MessageBox.Show(ex.ToString(), ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
             }
        }

        private void SceneResize(object sender, EventArgs e)
        {
            if (!this.loaded)
            {
                return;
            }
            this.glControl.MakeCurrent();
            this.SetupViewport();
        }

        private void ControlLoaded(object sender, EventArgs e)
        {
            this.loaded = true;
            this.glControl.MakeCurrent();
            this.SetupViewport();
        }

        private void SetupViewport()
        {
            if (!this.loaded)
            {
                return;
            }
            if (GraphicsContext.CurrentContext == null)
            {
                return;
            }
            int w = Math.Max(1, this.glControl.Width);
            int h = Math.Max(1, this.glControl.Height);
            // Use all of the glControl painting area
            //this.graphicsContext.SetViewport(0, 0, w, h);

            //this.Camera.AspectRatio = w / (float)h;
        }
    }
}
