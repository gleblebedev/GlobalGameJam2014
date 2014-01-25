using System;
using OpenTK;

namespace Game.Model
{
    public class Fly : INonControlledCreature
    {
        private readonly World world;
        public Basis Position { get; private set; }
        public float Pitch { get; set; }

        private Vector3 directionMovement;
        private Random random = new Random();

        private TimeSpan timeOfMovement = new TimeSpan(0, 0, 0, 5);


        public Fly(World world)
        {
            this.world = world;
            this.Position = new Basis(new Vector3(10, 10, 10));
            this.Position.Origin = new Vector3(10, 10, 10);
            this.ChooseDirection();
        }

        public void Update(TimeSpan dt)
        {
            timeOfMovement = timeOfMovement - dt;
            if (timeOfMovement.Ticks > 0)
                this.Position.Origin += directionMovement;
            else
            {
                timeOfMovement = new TimeSpan(0, 0, 0, 5);
                this.ChooseDirection();
            }
        }

        private void ChooseDirection()
        {
            var xComponent = random.Next(-1, 1);
            if (Position.Origin.X > world.SizeX)
            {
                xComponent = random.Next(-1, 0);
            }
            else if (Position.Origin.X < 0)
            {
                xComponent = random.Next(0, 1);
            }

            var yComponent = random.Next(-1, 1);
            if (Position.Origin.Y > world.SizeY)
            {
                yComponent = random.Next(-1, 0);
            }
            else if(Position.Origin.Y < 0)
            {
                yComponent = random.Next(0, 1);
            }

            var zComponent = random.Next(-1, 1);
            if (Position.Origin.Z > world.SizeZ)
            {
                zComponent = random.Next(-1, 0);
            }
            else if (Position.Origin.Z < 0)
            {
                zComponent = random.Next(-1, 0);
            }

            this.directionMovement = new Vector3(xComponent, yComponent, zComponent);

        }
    }
}