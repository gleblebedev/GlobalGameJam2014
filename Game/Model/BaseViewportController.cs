using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game.Model
{
	public class BaseViewportController
	{
		private Basis position = new Basis(new Vector3(64, 64, 64), new Vector3(-1, -1, -1),new Vector3(0, 0, 1));
		
		protected void SetViewport(int minX, int minY, int width, int height, float fovx, Vector3 forward, Vector3 up)
		{
			GL.Viewport(minX, minY, width, height);
			var m = Matrix4.LookAt(this.position.Origin, this.position.Origin + forward, up);
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
	

		public Basis Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.position = value;
			}
		}
	}
}