namespace Game.Model
{
	public class GameOptions
	{
		PlayerOptions[] players = new PlayerOptions[]
			{
				new PlayerOptions() { Control = ControlType.WASD}, 
				new PlayerOptions() {Control = ControlType.None}, 
			};

		public PlayerOptions[] Players { get
		{
			return this.players;
		} set
		{
			this.players = value;
		}}

		public VoxelArray VoxelArray
		{
			get
			{
				return this.voxelArray;
			}
			set
			{
				this.voxelArray = value;
			}
		}

		private VoxelArray voxelArray = new VoxelArray(32,32,32);

	}
}