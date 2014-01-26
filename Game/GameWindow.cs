using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using Game.Model;

using Newtonsoft.Json;

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Game
{
	public partial class GameWindow : Form
	{
		#region Constants and Fields

		private readonly MaterialMap materialMap;

		private readonly Random rnd = new Random();

		private IScene scene;

		private int h;

		private bool loaded;

		private int w;

		#endregion

		#region Constructors and Destructors
		public GameWindow():this(new GameOptions())
		{
			
		}

		protected override void OnDeactivate(EventArgs e)
		{
			this.scene.DeactivateControls();
			base.OnDeactivate(e);
		}
		protected override void OnActivated(EventArgs e)
		{
			this.scene.ActivateControls();
			base.OnDeactivate(e);
		}
		public GameWindow(GameOptions options)
		{
			this.InitializeComponent();

			this.materialMap = new MaterialMap();

			this.materialMap[1] = new WorldMaterial { Color = Color4.White, Texture = new Texture(LoadTexture("001")) };
			this.materialMap[2] = new WorldMaterial { Color = Color4.White, Texture = new Texture(LoadTexture("grass")) };
			this.materialMap[3] = new WorldMaterial { Color = Color4.White, Texture = new Texture(LoadTexture("wood")) };
			this.materialMap[4] = new WorldMaterial { Color = Color4.Blue };

			this.spider = LoadModel("spider");
			this.fly = LoadModel("Fly");

			var world = new World(options.VoxelArray, this.materialMap);
			this.gameScene = this.scene = new GameScene(world, options) {Spider = spider, Fly = fly};
			
			this.editorScene = new EditorScene(world,options.VoxelArray);
			this.BringToFront();
			this.Focus();
			this.KeyPreview = true;
			glControl.MouseMove += OnGlControlMouseMove;
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


		static Model3D LoadModel(string s1)
		{
			using (var s = typeof(GameWindow).Assembly.GetManifestResourceStream("Game.Models." + s1 + ".json"))
			{
				var ms = new MemoryStream();
				s.CopyTo(ms);
				ms.Flush();
				var value = Encoding.UTF8.GetString(ms.ToArray()).Trim((char)65279);
				return (Model3D)JsonConvert.DeserializeObject(value, typeof(Model3D));
			}
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

		private DateTime? previousTime;

		private IScene gameScene;

		private IScene editorScene;

		private Model3D fly;

		private Model3D spider;

		private void RenderScene(object sender, PaintEventArgs e)
		{
			DateTime now = DateTime.Now;
			if (previousTime != null)
			{
				var dt = now.Subtract(previousTime.Value);
				this.scene.Update(dt);
			}
			previousTime = now;
			if (!this.loaded) // Play nice
			{
				return;
			}
			try
			{
				this.glControl.MakeCurrent();
				GL.ClearColor(new Color4(0, 0x20, 0x40, 0));
				GL.ClearDepth(1.0f);
				GL.DepthMask(true);
				GL.DepthFunc(DepthFunction.Less);
				GL.Viewport(0,0,w,h);
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

		protected override void OnKeyDown(KeyEventArgs e)
		{
	
			scene.OnKeyDown(e);
			if (!e.Handled)
				base.OnKeyDown(e);
		}
		protected override void OnKeyUp(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F5)
			{
				if (this.scene == this.gameScene)
				{
					this.scene = this.editorScene;
				}
				else
				{
					this.scene = this.gameScene;
				}
				e.Handled = true;
				return;
			}
			scene.OnKeyUp(e);
			if (!e.Handled)
				base.OnKeyDown(e);
		}
	
		private void OnGlControlMouseMove(object sender, MouseEventArgs e)
		{
			scene.OnMouseMove(e);
		}

		#endregion
	}
}