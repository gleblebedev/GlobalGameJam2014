using System;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Game.Model
{
	public class GameScene:IScene
	{
		private readonly World world;

		private IViewports viewport;

		private SpiderScreen viewport2;

		private Spider spider;

		private float a = 0.0f;

		private IController controller;

		public GameScene(World world)
		{
			this.viewport = new SingleScreen();
			this.viewport2 = new SpiderScreen();
			this.spider = new Spider();
			spider.Position.Origin = new Vector3(16, 16, 16);
			this.controller = new WasdController(this.spider);
			this.world = world;
		}

		#region Implementation of IScene

		public void Render(int width, int height)
		{		

			viewport.Position = spider.Position;
			viewport.Pitch = spider.Pitch;
			viewport.Render(0, 0, width, height / 2, RenderImpl);

			viewport2.Position = spider.Position;
			viewport2.Pitch = spider.Pitch;
			viewport2.Render(0, height / 2, width, height, RenderImpl);
		}

		public void Update(TimeSpan dt)
		{
			controller.Update(dt);
		}

		public void OnKeyDown(KeyEventArgs keyEventArgs)
		{
			controller.OnKeyDown(keyEventArgs);
		}

		public void OnKeyUp(KeyEventArgs keyEventArgs)
		{
			controller.OnKeyUp(keyEventArgs);
		}

		public void OnMouseMove(MouseEventArgs mouseEventArgs)
		{
			controller.OnMouseMove(mouseEventArgs);
		}

		private void RenderImpl()
		{
			world.Render();

			GL.DepthMask(false);
			GL.Disable(EnableCap.DepthTest);
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
			GL.DepthMask(true);
			GL.Enable(EnableCap.DepthTest);
		}

		#endregion
	}
}