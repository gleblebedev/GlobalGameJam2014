using System;
using System.Drawing;
using System.Windows.Forms;

using Game.Model;

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Game
{
	public partial class GameWindow : Form
	{
		#region Constants and Fields

		private readonly MaterialMap materialMap;

		private readonly Random rnd = new Random();

		private readonly IScene scene;

		private int h;

		private bool loaded;

		private Spider spider;

		private int w;

		#endregion

		#region Constructors and Destructors

		public GameWindow()
		{
			this.InitializeComponent();
			this.spider = new Spider();
			this.materialMap = new MaterialMap();

			this.materialMap[1] = new WorldMaterial { Color = Color4.White, Texture = new Texture(LoadTexture("001")) };
			this.materialMap[2] = new WorldMaterial { Color = Color4.Red };
			this.materialMap[3] = new WorldMaterial { Color = Color4.Green };
			this.materialMap[4] = new WorldMaterial { Color = Color4.Blue };
			var voxelArray = new VoxelArray(32, 32, 32);
			voxelArray.OutlineBox(1, 0, 0, 0, 31, 31, 31);
			for (int i = 0; i < 1000; ++i)
			{
				var index = this.rnd.Next(voxelArray.SizeX * voxelArray.SizeY * voxelArray.SizeZ);
				var x = index % voxelArray.SizeX;
				index /= voxelArray.SizeX;
				var y = index % voxelArray.SizeY;
				index /= voxelArray.SizeY;
				var z = index % voxelArray.SizeZ;
				voxelArray.FillBox((byte)(this.rnd.Next(3) + 2), x, y, z, x, y, z);
			}
			this.scene = new GameScene(new World(voxelArray, this.materialMap));
			this.BringToFront();
			this.Focus();
			this.KeyPreview = true;
		}

		#endregion

		#region Public Methods and Operators

		public void UpdateGame()
		{
			//if (glControl.IsIdle)
			{
				this.glControl.Invalidate();
				//this.RenderScene(this, null);
			}
		}

		#endregion

		#region Methods

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			switch (keyData)
			{
				case Keys.Left:
				case Keys.A:
					this.TurnLeft();
					return true;
					break;
				case Keys.Right:
				case Keys.D:
					this.TurnRight();
					return true;
					break;
				case Keys.Up:
				case Keys.W:
					this.GoForward();
					return true;
					break;
				case Keys.Down:
				case Keys.S:
					this.GoBack();
					return true;
					break;
				default:
					return base.ProcessCmdKey(ref msg, keyData);
					break;
			}
			// etc..
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private static Bitmap LoadTexture(string s1)
		{
			using (var s = typeof(GameWindow).Assembly.GetManifestResourceStream("Game.Textures." + s1 + ".jpg"))
			{
				return new Bitmap(s);
			}
		}

		private void ControlLoaded(object sender, EventArgs e)
		{
			this.loaded = true;
			this.glControl.MakeCurrent();
			this.SetupViewport();
		}

		private void GoBack()
		{
			//
		}

		private void GoForward()
		{
			//
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
				GL.ClearColor(new Color4(0, 0x20, 0x40, 0));
				GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

				this.scene.Render(this.w, this.h);

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

		private void TurnLeft()
		{
			//
		}

		private void TurnRight()
		{
			//
		}

		#endregion
	}
}