using System;

using OpenTK.Graphics.OpenGL;

namespace Game.Model
{
	public class SingleScreen : BaseViewportController, IViewports
	{

		public void Render(int minX, int minY, int maxX, int maxY, Action renderCallback)
		{
			this.SetViewport(minX, minY, maxX - minX, maxY - minY, (float)Math.PI / 4.0f, this.Forward, this.Up);

			renderCallback();
		}

	
	}
}