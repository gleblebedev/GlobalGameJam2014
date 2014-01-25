namespace Game.Model
{
	public class GameOptions
	{
		PlayerOptions[] players = new PlayerOptions[]
			{
				new PlayerOptions(), 
				new PlayerOptions(), 
			};

		public PlayerOptions[] Players { get
		{
			return this.players;
		} set
		{
			this.players = value;
		}}
	}
}