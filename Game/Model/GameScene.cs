using System;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game.Model
{
	public class GameScene:IScene
	{
		private readonly World world;

		public GameScene(World world)
		{
			this.world = world;
		}

		#region Implementation of IScene

		public void Render(int width, int height)
		{
			var m = Matrix4.LookAt(new Vector3(64, 64, 64), new Vector3(0, 0, 0), new Vector3(0, 0, 1));
			var p = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI/4.0f, width/(float)height,0.05f,120.0f);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadMatrix(ref p);
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadMatrix(ref m);

			world.Render();
		}

		

		#endregion
	}
}