using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game.Model
{
	public class BaseViewportController 
	{
		private Vector3 forward = new Vector3(-1, -1, -1);

		private Vector3 up = new Vector3(0, 0, 1);

		private Vector3 eye = new Vector3(64, 64, 64);

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
		protected void SetViewport(int minX, int minY, int width, int height, float fovx, Vector3 forward, Vector3 up)
		{
			GL.Viewport(minX, minY, width, height);
			var m = Matrix4.LookAt(this.Eye, this.Eye + forward, up);
			var aspect = width / (float)height;
			var fovy = fovx / aspect;
			var maxfovy = (float)MathHelper.Pi / 2 - 0.1f;
			if (fovy > maxfovy)
			{
				fovy = maxfovy;
			}
			var p = Matrix4.CreatePerspectiveFieldOfView(fovy, aspect, 0.05f, 120.0f);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadMatrix(ref p);
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadMatrix(ref m);
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
	}
}