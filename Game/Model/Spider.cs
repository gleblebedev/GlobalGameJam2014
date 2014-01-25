using System;

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

		private const float MaxPitch = 1;
	}
}