using System;

using OpenTK;

namespace Game.Model
{
	public interface IControlledCreature
	{
		Basis Position { get;  }

		float Pitch { get; set; }

		Vector3 LookDirection { get;  }

		void Update(TimeSpan dt);

		void Move(Vector3 direction, float stepScale);

		void Rotate(float angle);
	}
}