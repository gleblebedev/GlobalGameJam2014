using System;

using OpenTK;

namespace Game.Model
{
	public class Spider : IControlledCreature
	{
		private readonly World world;

		public Spider(World world, Vector3 contactPoint, Vector3 contactPointNormal)
		{
			this.world = world;
			this.contactPoint = contactPoint;
			this.contactPointNormal = contactPointNormal;
			UpdateBasis();
		}

		private void UpdateBasis()
		{
			var up = this.world.GetAverageNormal(this.contactPoint, this.contactPointNormal);
			this.Position.Origin = this.contactPoint + up * 0.5f;
			var x = this.Position.X;
			var y = Vector3.Cross(up, x);
			x = Vector3.Cross(y, up);
			this.Position.SetRotation(x,up);
		}

		private Basis position = new Basis();

		private float pitch;

		private Vector3 contactPoint;

		private Vector3 contactPointNormal;

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

		public float Pitch
		{
			get
			{
				return this.pitch;
			}
			set
			{
				this.pitch = Math.Max(-MaxPitch, Math.Min(MaxPitch, value));
			}
		}

		public void Move(Vector3 direction, float stepScale)
		{
			if (stepScale == 0)
				return;
			if (stepScale < 0)
			{
				direction = -direction;
				stepScale = -stepScale;
			}
			direction = (Position.X * direction.X + Position.Y * direction.Y + Position.Z * direction.Z);
			direction = direction - contactPointNormal * Vector3.Dot(direction, contactPointNormal);
			direction.Normalize();
			float eps = 1e-6f;
			if (direction.X > -eps && direction.X < eps) direction.X = 0;
			if (direction.Y > -eps && direction.Y < eps) direction.Y = 0;
			if (direction.Z > -eps && direction.Z < eps) direction.Z = 0;
			direction.Normalize();

			//Position.Origin += direction*stepScale;
			var center = contactPoint + contactPointNormal * 0.5f;
			var x = (int)Math.Floor(center.X);
			var y = (int)Math.Floor(center.Y);
			var z = (int)Math.Floor(center.Z);

			//float maxStepScale = stepScale;

			//if (direction.X > eps)
			//{
			//}
			//else if (direction.X < eps)
			//{
				
			//}
			
			var newContact = contactPoint + direction * stepScale;

			contactPoint = newContact;
			
			UpdateBasis();
		}

		public void Rotate(float angle)
		{
			Position.Rotate(Position.Z, angle);
		}

		public Vector3 LookDirection
		{
			get
			{
				var b = this.Position.Clone();
				b.Rotate(b.Y, Pitch);
				return b.X;
			}
		}

		private const float MaxPitch = 1;

		public void Spawn(Vector3 contactPoint, Vector3 contactPointNormal)
		{
			this.contactPoint = contactPoint;
			this.contactPointNormal = contactPointNormal;
		}
	}
}