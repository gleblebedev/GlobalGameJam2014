using System;

using OpenTK;

namespace Game.Model
{
	public interface IControlledCreature
	{
		Basis Position { get;  }

		float Pitch { get; set; }

		Vector3 LookDirection { get;  }

		bool IsInMove { get; set; }

		void Update(TimeSpan dt);

		void Move(Vector3 direction, float stepScale);

		void Rotate(float angle);
	}
}