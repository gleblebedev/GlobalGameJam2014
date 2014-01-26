using System;
using System.Drawing;
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

		private readonly VoxelArray voxelArray;

		private bool hasBlockPos;

		private Vector3 newBlockNormal;

		private Vector3 newBlockPoint;

		private bool left;

		private bool right;

		private bool trunLeft;

		private bool trunRight;

		private bool forward;

		private bool backward;

		#endregion

		#region Constructors and Destructors

		public EditorScene(World world, VoxelArray voxelArray)
		{
			this.world = world;
			this.voxelArray = voxelArray;
			this.viewport = new SingleScreen() { };
			this.viewport.Position.Origin = new Vector3(this.world.SizeX * 0.5f, this.world.SizeY * 0.5f, this.world.SizeZ * 0.5f);
			this.viewport.Position.ResetRotation();
		}

		#endregion

		#region Public Methods and Operators

		public void OnKeyDown(KeyEventArgs keyEventArgs)
		{
			switch (keyEventArgs.KeyCode)
			{
				case Keys.A:
					this.left = true;
					keyEventArgs.Handled = true;
					break;
				case Keys.D:
					this.right = true;
					keyEventArgs.Handled = true;
					break;
				case Keys.C:
					this.down = true;
					keyEventArgs.Handled = true;
					break;
				case Keys.E:
					this.up = true;
					keyEventArgs.Handled = true;
					break;
				case Keys.W:
					this.forward = true;
					keyEventArgs.Handled = true;
					break;
				case Keys.S:
					this.backward = true;
					keyEventArgs.Handled = true;
					break;
			}
		}

		public void OnKeyUp(KeyEventArgs keyEventArgs)
		{
			switch (keyEventArgs.KeyCode)
			{
				case Keys.A:
					this.left = false;
					keyEventArgs.Handled = true;
					break;
				case Keys.D:
					this.right = false;
					keyEventArgs.Handled = true;
					break;
				case Keys.C:
					this.down = false;
					keyEventArgs.Handled = true;
					break;
				case Keys.E:
					this.up = false;
					keyEventArgs.Handled = true;
					break;
				case Keys.W:
					this.forward = false;
					keyEventArgs.Handled = true;
					break;
				case Keys.S:
					this.backward = false;
					keyEventArgs.Handled = true;
					break;
				case Keys.Space:
					RemoveElement();
					keyEventArgs.Handled = true;
					break;
                case Keys.U:
				case Keys.NumPad1:
					SetElement(1);
					keyEventArgs.Handled = true;
					break;
                case Keys.I:
				case Keys.NumPad2:
					SetElement(2);
					keyEventArgs.Handled = true;
					break;
                case Keys.O:
				case Keys.NumPad3:
					SetElement(3);
					keyEventArgs.Handled = true;
					break;
                case Keys.P:
				case Keys.NumPad4:
					SetElement(4);
					keyEventArgs.Handled = true;
					break;
			}
		}

		private void SetElement(byte i)
		{
			if (!this.hasBlockPos)
				return;
			var center = newBlockPoint + newBlockNormal * 0.5f;
			var x = (int)Math.Floor(center.X);
			var y = (int)Math.Floor(center.Y);
			var z = (int)Math.Floor(center.Z);
			voxelArray.FillBox(i, x, y, z, x, y, z);
		}

		private void RemoveElement()
		{
			if (!this.hasBlockPos)
				return;
			var center = newBlockPoint - newBlockNormal * 0.5f;
			var x = (int)Math.Floor(center.X);
			var y = (int)Math.Floor(center.Y);
			var z = (int)Math.Floor(center.Z);
			voxelArray.FillBox(0,x,y,z,x,y,z);
		}

		private Point? mouseLocation;

		private bool down;

		private bool up;

		public void OnMouseMove(MouseEventArgs mouseEventArgs)
		{
			if (mouseLocation != null)
			{
				if ((mouseEventArgs.Button & MouseButtons.Left) == MouseButtons.Left)
				{
					var dy = mouseEventArgs.Location.Y - mouseLocation.Value.Y;
					var dx = mouseEventArgs.Location.X - mouseLocation.Value.X;
					if (dy != 0)
						viewport.Pitch -= dy * 0.002f;
					if (dx != 0)
					{
						viewport.Position.Rotate(viewport.Position.Z,dx * 0.002f);
					}
				}
			}
			this.mouseLocation = mouseEventArgs.Location;
		}
		public void Render(int width, int height)
		{
			this.hasBlockPos = this.world.TraceRay(
				this.viewport.Position.Origin,
				this.viewport.Position.Origin + this.viewport.LookDirection * 20.0f,
				out this.newBlockPoint,
				out this.newBlockNormal);
			this.viewport.Render(0, 0, width, height, this.RenderImpl);
		}

		public void Update(TimeSpan dt)
		{
			var scale = (float)dt.TotalSeconds;
			if (this.trunLeft && !this.trunRight)
			{
				this.viewport.Position.Rotate(this.viewport.Position.Z, scale);
			}
			if (!this.trunLeft && this.trunRight)
			{
				this.viewport.Position.Rotate(this.viewport.Position.Z, -scale);
			}
			if (this.up && !this.down)
			{
				this.viewport.Position.Origin += this.viewport.Position.Z * scale * 5.0f;
			}
			if (!this.up && this.down)
			{
				this.viewport.Position.Origin -= this.viewport.Position.Z * scale * 5.0f;
			}
			if (this.left && !this.right)
			{
				this.viewport.Position.Origin += this.viewport.Position.Y* scale*5.0f;
			}
			if (!this.left && this.right)
			{
				this.viewport.Position.Origin -= this.viewport.Position.Y * scale * 5.0f;
			}
			if (this.forward && !this.backward)
			{
				this.viewport.Position.Origin += this.viewport.Position.X * scale * 5.0f;
			}
			if (!this.forward && this.backward)
			{
				this.viewport.Position.Origin -= this.viewport.Position.X * scale * 5.0f;
			}
		}

		#endregion

		#region Methods

		private void RenderImpl()
		{
			this.world.Render(0);

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