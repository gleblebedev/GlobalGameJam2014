using System;

using OpenTK.Graphics.OpenGL;

namespace Game.Model
{
	public class SingleScreen : BaseViewportController, IViewports
	{
		public float Pitch { get; set; }

		public void Render(int minX, int minY, int maxX, int maxY, Action renderCallback)
		{
			var basis = Position.Clone();
			basis.Rotate(basis.Y,Pitch);
			this.SetViewport(minX, minY, maxX - minX, maxY - minY, (float)Math.PI / 2.0f, basis.X, basis.Z);

			renderCallback();
		}

	
	}
}