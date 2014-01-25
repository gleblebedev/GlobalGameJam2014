using System;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Game.Model
{
	public class GameScene:IScene
	{
		private readonly World world;

		private IViewports viewport;

		private SpiderScreen viewport2;

		private IControlledCreature spider;

		private float a = 0.0f;

		private IController controller;

		public GameScene(World world)
		{
			this.world = world;

			this.viewport = new SingleScreen();
			this.viewport2 = new SpiderScreen();
			Vector3 contactPoint, contactPointNormal,point;
			do
			{
				point = GetSpawnPoint();
			}
			while (!world.TraceRay(point, point - new Vector3(0,0,1) * 10.0f, out contactPoint, out contactPointNormal));

			this.spider = new Spider(this.world, contactPoint + contactPointNormal*0.5f);
			
			this.controller = new WasdController(this.spider);
		}

		private Vector3 GetSpawnPoint()
		{
			retry:
			var x = rnd.Next(world.SizeX);
			var y = rnd.Next(world.SizeY);
			var z = 1;// rnd.Next(world.SizeZ);
			if (!world.IsEmpty(x, y, z)) goto retry;
			while (z > 0 && world.IsEmpty(x,y,z-1))
			{
				--z;
			}
			if (z == 0)
				goto retry;
			return new Vector3(x+0.5f, y+0.5f, z+0.5f);
		}
		Random rnd = new Random();

		private bool hasBlockPos;
		Vector3 newBlockPoint;
		Vector3 newBlockNormal;
		#region Implementation of IScene

		public void Render(int width, int height)
		{
			
			this.hasBlockPos = world.TraceRay(this.spider.Position.Origin, this.spider.Position.Origin + this.spider.LookDirection * 10.0f, out newBlockPoint, out newBlockNormal);

			viewport.Position = spider.Position;
			viewport.Pitch = spider.Pitch;
			viewport.Render(0, 0, width, height / 2, RenderImpl);

			viewport2.Position = spider.Position;
			viewport2.Pitch = spider.Pitch;
			viewport2.Render(0, height / 2, width, height, RenderImpl);
		}

		public void Update(TimeSpan dt)
		{
			spider.Update(dt);
			controller.Update(dt);
		}

		public void OnKeyDown(KeyEventArgs keyEventArgs)
		{
			controller.OnKeyDown(keyEventArgs);
		}

		public void OnKeyUp(KeyEventArgs keyEventArgs)
		{
			controller.OnKeyUp(keyEventArgs);
		}

		public void OnMouseMove(MouseEventArgs mouseEventArgs)
		{
			controller.OnMouseMove(mouseEventArgs);
		}

		private void RenderImpl()
		{
			world.Render();

			GL.DepthMask(false);
			GL.Disable(EnableCap.DepthTest);
			GL.Disable(EnableCap.Texture2D);

			if (hasBlockPos)
			{
				var p = newBlockPoint + newBlockNormal * 0.5f;
				int x = (int)Math.Floor(p.X);
				int y = (int)Math.Floor(p.Y);
				int z = (int)Math.Floor(p.Z);

				GL.Begin(PrimitiveType.Lines);

				GL.Color4(Color4.Red);
				GL.Vertex3(newBlockPoint-Vector3.UnitX);
				GL.Vertex3(newBlockPoint + Vector3.UnitX);
				GL.Color4(Color4.Green);
				GL.Vertex3(newBlockPoint - Vector3.UnitY);
				GL.Vertex3(newBlockPoint + Vector3.UnitY);
				GL.Color4(Color4.Blue);
				GL.Vertex3(newBlockPoint - Vector3.UnitZ);
				GL.Vertex3(newBlockPoint + Vector3.UnitZ);

				GL.Color4(Color4.White);
				GL.Vertex3(x, y, z);
				GL.Vertex3(x+1, y, z);
				GL.Vertex3(x, y+1, z);
				GL.Vertex3(x + 1, y+1, z);
				GL.Vertex3(x, y + 1, z+1);
				GL.Vertex3(x + 1, y + 1, z+1);
				GL.Vertex3(x, y , z + 1);
				GL.Vertex3(x + 1, y, z + 1);

				GL.Vertex3(x, y, z);
				GL.Vertex3(x, y, z+1);
				GL.Vertex3(x+1, y, z);
				GL.Vertex3(x+1, y, z + 1);
				GL.Vertex3(x + 1, y+1, z);
				GL.Vertex3(x + 1, y+1, z + 1);
				GL.Vertex3(x , y + 1, z);
				GL.Vertex3(x , y + 1, z + 1);

				GL.Vertex3(x, y, z);
				GL.Vertex3(x, y + 1, z);
				GL.Vertex3(x+1, y, z);
				GL.Vertex3(x+1, y + 1, z);
				GL.Vertex3(x + 1, y, z+1);
				GL.Vertex3(x + 1, y + 1, z+1);
				GL.Vertex3(x , y, z + 1);
				GL.Vertex3(x , y + 1, z + 1);
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