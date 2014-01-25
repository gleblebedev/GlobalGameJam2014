namespace Game.Model
{
	public interface IControlledCreature
	{
		Basis Position { get;  }

		float Pitch { get; set; }
	}
}