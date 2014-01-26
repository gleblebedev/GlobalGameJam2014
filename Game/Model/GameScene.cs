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



		private float a = 0.0f;


		private Fly fly;
		private IList<PlayerData> players;
		public GameScene(World world, GameOptions options)
		{
			this.world = world;
			players = new List<PlayerData>();
			fly = new Fly(world, this.Spawn(world));
			foreach (var player in options.Players)
			{
				if (player.Control != ControlType.None)
				{
					var playerData = new PlayerData() { Viewport = new SpiderScreen() { NumberOfEyes = (int)player.Eyes }, Creature = new Spider(world, this.Spawn(world)) };
					playerData.Controller = this.CreateController(player.Control,playerData.Creature);
					players.Add(playerData);
				}
			}
		}

		public Model3D Spider { get; set; }
		public Model3D Fly { get; set; }
		private IController CreateController(ControlType control, IControlledCreature creature)
		{
			switch (control)
			{
				case ControlType.None:
					return null;
					break;
				case ControlType.WASD:
					return new WasdController(creature);
				case ControlType.Arrows:
					return new ArrowsController(creature);
				case ControlType.Gamepad1:
					return new WasdController(creature);
				case ControlType.Gamepad2:
					return new WasdController(creature);
				default:
					throw new ArgumentOutOfRangeException("control");
			}
		}

		private Vector3 Spawn(World world)
		{
			Vector3 point;
			Vector3 contactPoint, contactPointNormal;
			do
			{
				point = this.GetSpawnPoint();
			}
			while (!world.TraceRay(point, point - new Vector3(0, 0, 1) * 10.0f, out contactPoint, out contactPointNormal));
			return contactPoint + contactPointNormal * 0.5f;
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
			var yStep = height / players.Count;
			for (int index = 0; index < this.players.Count; index++)
			{
				var playerData = this.players[index];
				playerData.Viewport.Position = playerData.Creature.Position;
				playerData.Viewport.Pitch = playerData.Creature.Pitch;
				float opacity = 0;
				if (playerData.Creature.IsInMove)
				{
				}
				else
				{
					opacity = 1.0f;
				}
				playerData.Viewport.Render(0, yStep * index,width, yStep*(index+1), ()=> this.RenderImpl(index,opacity));
			}
		}

		public void Update(TimeSpan dt)
		{
			foreach (var playerData in players)
			{
				playerData.Creature.Update(dt);
				playerData.Controller.Update(dt);
			}
			fly.Update(dt);
		}

	    private void CheckCollision(Fly fly1, IControlledCreature player)
	    {
            var lentgh = Math.Sqrt(Math.Pow(fly1.Position.Origin.X - player.Position.Origin.X, 2) +
                      Math.Pow(fly1.Position.Origin.Y - player.Position.Origin.Y, 2) +
                      Math.Pow(fly1.Position.Origin.Z - player.Position.Origin.Z, 2));

	        double someMagicValue = 5;
	        if (lentgh < someMagicValue)
            {
                //�� ��������
            }

	    }
		public void OnKeyDown(KeyEventArgs keyEventArgs)
		{
			foreach (var playerData in players)
			{
				playerData.Controller.OnKeyDown(keyEventArgs);
			}
		}

		public void OnKeyUp(KeyEventArgs keyEventArgs)
		{
			foreach (var playerData in players)
			{
				playerData.Controller.OnKeyUp(keyEventArgs);
			}
		}

		public void OnMouseMove(MouseEventArgs mouseEventArgs)
		{
			foreach (var playerData in players)
			{
				playerData.Controller.OnMouseMove(mouseEventArgs);
			}
		}

		private void RenderImpl(int curPlayer, float opacity)
		{
			world.Render(opacity);

			for (int index = 0; index < this.players.Count; index++)
			{
				if (index != curPlayer)
				{
					if (Spider != null)
					{
						var position = players[index].Creature.Position.Clone();
						position.Origin -= position.Z * 0.5f;
						this.Spider.Render(position,0.5f);
					}
				}
			}
			{
				var position = fly.Position.Clone();
				position.Origin -= position.Z * 0.5f;
				this.Fly.Render(position, 0.5f);
			}
		}

		#endregion
	}
}