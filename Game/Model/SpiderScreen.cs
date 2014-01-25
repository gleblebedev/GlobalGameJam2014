using System;

using OpenTK;

namespace Game.Model
{
	public class SpiderScreen : BaseViewportController, IViewports
	{
		public void Render(int minX, int minY, int maxX, int maxY, Action renderCallback)
		{
			int width = Math.Max(1,maxX - minX);
			int height = Math.Max(1,maxY - minY);

			var forward = this.Forward.Normalized();
			var right = Vector3.Cross(this.Up, forward).Normalized();
			var up = Vector3.Cross(forward,right).Normalized();

			var pi = (float)Math.PI;

			var fovx = pi / 4.0f;

			var step = width / 8;
			var maxheight = step * 1.5f;
			if (maxheight < height)
			{
				var center = (maxY + minY) / 2;
				minY = (int)(center - maxheight / 2);
				height = (int)(maxheight);
			}
			for (int index = 0; index < 8; ++index)
			{
				Vector3 f;
				Vector3 r;
				var k = (1 - 1.0f / 8.0f + index / 4.0f);
				var a = pi * k;
				this.Rotate(forward,right,a,out f,out  r);
				this.SetViewport(minX + index * step, minY,  step, height, fovx, f, up);
				renderCallback();
			}

		}

		void Rotate(Vector3 x, Vector3 y,float a,out Vector3 xres, out Vector3 yres)
		{
			var cos = (float)Math.Cos(a);
			var sin = (float)Math.Sin(a);
			xres = x * cos + y*sin;
			yres = y*cos - x*sin;
		}
	}
}