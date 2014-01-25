using System;
using System.Collections.Generic;
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

		private IList<PlayerData> players;
		public GameScene(World world, GameOptions options)
		{
			this.world = world;
			players = new List<PlayerData>();
			
			foreach (var player in options.Players)
			{
				if (player.Control != ControlType.None)
				{
				}
			}

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

		#region Implementation of IScene

		public void Render(int width, int height)
		{
			

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

			
		}

		#endregion
	}
}