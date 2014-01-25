using System;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game.Model
{
	public class SingleScreen : BaseViewportController, IViewports
	{
		public float Pitch { get; set; }

		public Vector3 LookDirection
		{
			get
			{
				var b = this.Position.Clone();
				b.Rotate(b.Y, this.Pitch);
				return b.X;
			}
			
		}

		public void Render(int minX, int minY, int maxX, int maxY, Action renderCallback)
		{
			var basis = Position.Clone();
			basis.Rotate(basis.Y,Pitch);
			this.SetViewport(minX, minY, maxX - minX, maxY - minY, (float)Math.PI / 2.0f, basis.X, basis.Z);

			renderCallback();
		}

	
	}
}