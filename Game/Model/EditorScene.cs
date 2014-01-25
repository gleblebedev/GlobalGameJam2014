using System;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Game.Model
{
	public class EditorScene : IScene
	{
		#region Constants and Fields

		private readonly SingleScreen viewport;

		private readonly World world;

		private bool hasBlockPos;

		private Vector3 newBlockNormal;

		private Vector3 newBlockPoint;

		#endregion

		#region Constructors and Destructors

		public EditorScene(World world)
		{
			this.world = world;
			this.viewport = new SingleScreen();
		}

		#endregion

		#region Public Methods and Operators

		public void OnKeyDown(KeyEventArgs keyEventArgs)
		{
		}

		public void OnKeyUp(KeyEventArgs keyEventArgs)
		{
		}

		public void OnMouseMove(MouseEventArgs mouseEventArgs)
		{
		}

		public void Render(int width, int height)
		{
			this.hasBlockPos = this.world.TraceRay(
				this.viewport.Position.Origin,
				this.viewport.Position.Origin + this.viewport.LookDirection * 10.0f,
				out this.newBlockPoint,
				out this.newBlockNormal);
			this.viewport.Render(0, 0, width, height, this.RenderImpl);
		}

		public void Update(TimeSpan dt)
		{
		}

		#endregion

		#region Methods

		private void RenderImpl()
		{
			this.world.Render();

			GL.DepthMask(false);
			GL.Disable(EnableCap.DepthTest);
			GL.Disable(EnableCap.Texture2D);

			if (this.hasBlockPos)
			{
				var p = this.newBlockPoint + this.newBlockNormal * 0.5f;
				int x = (int)Math.Floor(p.X);
				int y = (int)Math.Floor(p.Y);
				int z = (int)Math.Floor(p.Z);

				GL.Begin(PrimitiveType.Lines);

				GL.Color4(Color4.Red);
				GL.Vertex3(this.newBlockPoint - Vector3.UnitX);
				GL.Vertex3(this.newBlockPoint + Vector3.UnitX);
				GL.Color4(Color4.Green);
				GL.Vertex3(this.newBlockPoint - Vector3.UnitY);
				GL.Vertex3(this.newBlockPoint + Vector3.UnitY);
				GL.Color4(Color4.Blue);
				GL.Vertex3(this.newBlockPoint - Vector3.UnitZ);
				GL.Vertex3(this.newBlockPoint + Vector3.UnitZ);

				GL.Color4(Color4.White);
				GL.Vertex3(x, y, z);
				GL.Vertex3(x + 1, y, z);
				GL.Vertex3(x, y + 1, z);
				GL.Vertex3(x + 1, y + 1, z);
				GL.Vertex3(x, y + 1, z + 1);
				GL.Vertex3(x + 1, y + 1, z + 1);
				GL.Vertex3(x, y, z + 1);
				GL.Vertex3(x + 1, y, z + 1);

				GL.Vertex3(x, y, z);
				GL.Vertex3(x, y, z + 1);
				GL.Vertex3(x + 1, y, z);
				GL.Vertex3(x + 1, y, z + 1);
				GL.Vertex3(x + 1, y + 1, z);
				GL.Vertex3(x + 1, y + 1, z + 1);
				GL.Vertex3(x, y + 1, z);
				GL.Vertex3(x, y + 1, z + 1);

				GL.Vertex3(x, y, z);
				GL.Vertex3(x, y + 1, z);
				GL.Vertex3(x + 1, y, z);
				GL.Vertex3(x + 1, y + 1, z);
				GL.Vertex3(x + 1, y, z + 1);
				GL.Vertex3(x + 1, y + 1, z + 1);
				GL.Vertex3(x, y, z + 1);
				GL.Vertex3(x, y + 1, z + 1);
				GL.End();
			}

			GL.Color4(Color4.Red);
			GL.Vertex3(0, 0, 0);
			GL.Vertex3(100, 0, 0);

			GL.Color4(Color4.Green);
			GL.Vertex3(0, 0, 0);
			GL.Vertex3(0, 100, 0);

			GL.Color4(Color4.Blue);
			GL.Vertex3(0, 0, 0);
			GL.Vertex3(0, 0, 100);

			GL.End();
			GL.DepthMask(true);
			GL.Enable(EnableCap.DepthTest);
		}

		#endregion
	}
}