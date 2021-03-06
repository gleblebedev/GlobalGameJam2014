using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Game.Model
{
	public class GameScene:IScene
	{
		private readonly World world;
	    private readonly GameOptions options;


	    private float a = 0.0f;


		private Fly fly;
		private IList<PlayerData> players;
		public GameScene(World world, GameOptions options)
		{
		    this.world = world;
		    this.options = options;
            this.Restart();
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
					return new GamePadController(0,creature);
				case ControlType.Gamepad2:
					return new GamePadController(1,creature);
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
				playerData.Viewport.Render(0, yStep * index,width, yStep*(index+1), ()=> this.RenderImpl(index,playerData.MovementFactor));
			}
		}

		public void Update(TimeSpan dt)
		{
			if (dt.TotalSeconds > 0.1)
			{
				dt = TimeSpan.FromSeconds(0.1);
			}
			foreach (var playerData in players)
			{
				playerData.Creature.Update(dt);
				playerData.Controller.Update(dt);
			    CheckCollision(fly, playerData.Creature);
				if (playerData.Creature.IsInMove)
				{
					playerData.MovementFactor = Math.Min(1,playerData.MovementFactor + (float)dt.TotalSeconds);
				}
				else
				{
					playerData.MovementFactor = Math.Max(0, playerData.MovementFactor - (float)dt.TotalSeconds);
				}
			}
			fly.Update(dt, players.Select(x=>x.Creature));
            
		}

	    private void CheckCollision(Fly fly1, IControlledCreature player)
	    {
			if (fly1.IsInMove)
				return;
		    var lentgh = (fly1.Position.Origin - player.Position.Origin).LengthFast;

            //Console.WriteLine(lentgh);
	        double someMagicValue = 0.8;
	        if (lentgh < someMagicValue)
	        {
	            player.Score += 1;
	            var dialogResult = MessageBox.Show(String.Format("{0} got the fly! Do you want to play again?", player.Name), "", MessageBoxButtons.OKCancel);
                if (dialogResult == DialogResult.OK)
                {
                    this.Restart();
                }
                else
                {
                    Application.Exit();
                }
	        }
	    }

	    private void Restart()
	    {
            players = new List<PlayerData>();
            fly = new Fly(world, this.Spawn(world));
            int index = 1;
            foreach (var player in options.Players)
            {
                if (player.Control != ControlType.None)
                {

                    var playerData = new PlayerData() { Viewport = new SpiderScreen() { NumberOfEyes = (int)player.Eyes }, Creature = new Spider(world, this.Spawn(world)) { Name = String.Format("Player {0}", index++) } };
                    playerData.Controller = this.CreateController(player.Control, playerData.Creature);
                    players.Add(playerData);
                }
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

		public void ActivateControls()
		{
			
		}

		public void DeactivateControls()
		{
			foreach (var playerData in players)
			{
				playerData.Controller.Deactivate();
			}
		}

		private void RenderImpl(int curPlayer, float movementFactor)
		{
			world.Render(1.0f-movementFactor);

			for (int index = 0; index < this.players.Count; index++)
			{
				if (index != curPlayer)
				{
					if (Spider != null)
					{
						var playerData = players[index];
						var position = playerData.Creature.Position.Clone();
						position.Origin -= position.Z * 0.5f;
						var otherFactor = playerData.MovementFactor;
						var opacity = 1-Math.Max(
							movementFactor * (1 - otherFactor), (1 - movementFactor) * otherFactor);
						this.Spider.Render(position, opacity);
					}
				}
			}
			{
				var otherFactor = fly.MovementFactor;
				var opacity = 1 - Math.Max(
					movementFactor * (1 - otherFactor), (1 - movementFactor) * otherFactor);
				var position = fly.Position.Clone();
				position.Origin -= position.Z * 0.5f;
				this.Fly.Render(position, opacity);
			}
		}

		#endregion
	}
}