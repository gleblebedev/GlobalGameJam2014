using System;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game.Model
{
	public class SingleScreen : IViewports
	{
		private Vector3 forward = new Vector3(-1,-1,-1);

		private Vector3 up = new Vector3(0,0,1);

		private Vector3 eye = new Vector3(64,64,64);

		public Vector3 Eye
		{
			get
			{
				return this.eye;
			}
			set
			{
				this.eye = value;
			}
		}

		public Vector3 Forward
		{
			get
			{
				return this.forward;
			}
			set
			{
				this.forward = value;
			}
		}

		public Vector3 Up
		{
			get
			{
				return this.up;
			}
			set
			{
				this.up = value;
			}
		}

		public void Render(int minX, int minY, int maxX, int maxY, Action renderCallback)
		{
			var width = maxX - minX;
			var height = maxY - minY;
			GL.Viewport(minX, minY, width, height);
			var m = Matrix4.LookAt(Eye, Eye+this.Forward, Up);
			var p = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4.0f, width / (float)height, 0.05f, 120.0f);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadMatrix(ref p);
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadMatrix(ref m);

			renderCallback();
		}
	}
}