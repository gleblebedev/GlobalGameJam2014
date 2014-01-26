using System;
using OpenTK;

namespace Game.Model
{
    public class Fly : INonControlledCreature
    {
        private readonly World world;
        public Basis Position { get; private set; }
        public float Pitch { get; set; }

	    public bool IsInMove
	    {
		    get
		    {
			    return think == this.DoFly;
		    }
	    }

	    private Random random = new Random();

        private TimeSpan timeOfMovement = new TimeSpan(0, 0, 0, 5);

	    private Action<TimeSpan> think;

	    private Vector3 targert;

	    private Vector3 targertN;

	    public float MovementFactor = 0;

	    public Fly(World world, Vector3 pos)
        {
            this.world = world;
            this.Position = new Basis(pos);
	        think = Sit;
		    MovementFactor = 0;
            this.ChooseDirection();
        }
		public void Sit(TimeSpan dt)
		{
			MovementFactor = Math.Max(0, MovementFactor -(float) dt.TotalSeconds);
			timeOfMovement = timeOfMovement - dt;
			if (timeOfMovement.Ticks <= 0)
			{
				this.ChooseDirection();
				think = this.DoFly;
			}
		}
		public void DoFly(TimeSpan dt)
		{
			MovementFactor = Math.Min(1, MovementFactor + (float)dt.TotalSeconds);

			var d = targert - Position.Origin;
			var l = d.Length;
			float speed = 10.0f;
			var step = (float)dt.TotalSeconds * speed;
			if (step >= l)
			{
				this.Position.Origin = targert;
				var y = RandomVector();
				var x = Vector3.Cross(y, targertN);
				this.Position.SetRotation(x, targertN);
				think = this.Sit;
				timeOfMovement = TimeSpan.FromSeconds(random.NextDouble() * 25.0f);
				return;
			}
			else
			{
				this.Position.Origin += d*(step / l);
				this.Position.SetRotation(d,new Vector3(0,0,1));
			}
		}

	    public void Update(TimeSpan dt)
	    {
		    think(dt);
        }
		Vector3 RandomVector()
		{
			var xComponent = (float)random.NextDouble() - 0.5f ;
			var yComponent = (float)random.NextDouble() - 0.5f ;
			var zComponent = (float)random.NextDouble() - 0.5f ;
			return new Vector3(xComponent,yComponent,zComponent);
		}
        private void ChooseDirection()
        {
	        var r = RandomVector() + Position.Z * 0.5f;
			r.NormalizeFast();
	        var from = this.Position.Origin;
			var to = from + r*30.0f;
	        to.X = Math.Min(world.SizeX - 1, Math.Max(1, to.X));
			to.Y = Math.Min(world.SizeY - 1, Math.Max(1, to.Y));
			to.Z = Math.Min(world.SizeZ - 1, Math.Max(1, to.Z));
			Vector3 n;
	        if (!world.TraceBox(from,ref to,out n))
	        {
		        think = this.Sit;
		        timeOfMovement = TimeSpan.FromSeconds(random.NextDouble() * 5.0f);
				return;
	        }
	        var pos = to + n * 0.5f;
	        this.targert = pos;
	        this.targertN = n;

        }
    }
}