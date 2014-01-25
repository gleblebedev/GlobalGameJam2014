using System;

using OpenTK;

namespace Game.Model
{
	public class Fly : IControlledCreature
	{
		#region Implementation of IControlledCreature

		public Basis Position { get; private set; }

		public float Pitch { get; set; }

		public Vector3 LookDirection { get; private set; }

		public void Update(TimeSpan dt)
		{
			
		}

		public void Move(Vector3 direction, float stepScale)
		{
		}

		public void Rotate(float angle)
		{

		}

		#endregion
	}
}