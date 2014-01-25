using OpenTK;

namespace Game.Model
{
	public interface IControlledCreature
	{
		Basis Position { get;  }

		float Pitch { get; set; }

		void Move(Vector3 direction, float stepScale);

		void Rotate(float angle);
	}
}