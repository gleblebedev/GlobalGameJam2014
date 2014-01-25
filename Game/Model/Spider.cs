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
			direction = (Position.X * direction.X + Position.Y * direction.Y + Position.Z * direction.Z);
			direction = direction - contactPointNormal * Vector3.Dot(direction, contactPointNormal);
			direction.Normalize();

			//Position.Origin += direction*stepScale;
			contactPoint += direction * stepScale;
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