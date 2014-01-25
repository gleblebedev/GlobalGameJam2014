using System;

using OpenTK;

namespace Game.Model
{
	public class Spider : IControlledCreature
	{
		private Basis position = new Basis();

		private float pitch;

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
	}
}