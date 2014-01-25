using System;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game.Model
{
	public class GameScene:IScene
	{
		private readonly World world;

		private IViewports viewport;

		private SpiderScreen viewport2;

		public GameScene(World world)
		{
			this.viewport = new SingleScreen();
			this.viewport2 = new SpiderScreen();
			this.world = world;
		}

		#region Implementation of IScene

		public void Render(int width, int height)
		{
			viewport.Eye = new Vector3(10, 10, 4);
			viewport.Forward = new Vector3(1, 0, 0);
			viewport.Up = new Vector3(0, 0, 1);
			viewport.Render(0,0,width,height/2,RenderImpl);
			viewport2.Eye = viewport.Eye;
			viewport2.Forward = viewport.Forward;
			viewport2.Up = viewport.Up;
			viewport2.Render(0, height / 2, width, height, RenderImpl);
		}

		private void RenderImpl()
		{
			world.Render();
		}

		#endregion
	}
}