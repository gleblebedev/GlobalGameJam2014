using System;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game.Model
{
	public class GameScene:IScene
	{
		private readonly World world;

		private IViewports viewport;

		public GameScene(World world)
		{
			this.viewport = new SingleScreen();
			this.world = world;
		}

		#region Implementation of IScene

		public void Render(int width, int height)
		{
			viewport.Render(0,0,width,height,RenderImpl);
		}

		private void RenderImpl()
		{
			world.Render();
		}

		#endregion
	}
}