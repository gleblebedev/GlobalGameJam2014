﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Game.Model;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Game
{
    public partial class GameWindow : Form
    {
        private bool loaded;

	    private IScene scene;

	    private MaterialMap materialMap;

	    private int h;

	    private int w;

	    private Spider spider;

	    public GameWindow()
        {
            InitializeComponent();
			spider = new Spider();
		    this.materialMap = new MaterialMap();

			materialMap[1] = new WorldMaterial() { Color = Color4.White, Texture = new Texture(LoadTexture("001")) };
			materialMap[2] = new WorldMaterial() { Color = Color4.Red };
			materialMap[3] = new WorldMaterial() { Color = Color4.Green };
			materialMap[4] = new WorldMaterial() { Color = Color4.Blue };
			var voxelArray = new VoxelArray(32, 32, 32);
			voxelArray.OutlineBox(1,0,0,0,31,31,31);
			for (int i = 0; i < 220;++i )
			{
				var x = rnd.Next(voxelArray.SizeX);
				var y = rnd.Next(voxelArray.SizeY);
				var z = rnd.Next(voxelArray.SizeZ);
				voxelArray.FillBox((byte)(rnd.Next(3) + 2), x, y,z,x,y,z);
			}
		    this.scene = new GameScene(new World(voxelArray, materialMap));

        }
		Random rnd = new Random();
		private static Bitmap LoadTexture(string s1)
	    {
			using (var s = typeof(GameWindow).Assembly.GetManifestResourceStream("Game.Textures." + s1 + ".jpg"))
		    {
			    return new Bitmap(s);
		    }
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
				 
	                        scene.Render(w,h);


				 GL.DepthMask(false);
							GL.Begin(PrimitiveType.Lines);

							GL.Color4(Color4.Red);
							GL.Vertex3(0, 0, 0);
							GL.Vertex3(100, 0, 0);

							GL.Color4(Color4.Green);
							GL.Vertex3(0, 0, 0);
							GL.Vertex3(0, 100, 0);

							GL.Color4(Color4.Blue);
							GL.Vertex3(0, 0, 0);
							GL.Vertex3(0, 0, 100);

							GL.End();

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
            this.w = Math.Max(1, this.glControl.Width);
            this.h = Math.Max(1, this.glControl.Height);
            // Use all of the glControl painting area
            //this.graphicsContext.SetViewport(0, 0, w, h);

            //this.Camera.AspectRatio = w / (float)h;
        }
    }
}
