using System;

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
				return this.voxelArray ?? (this.voxelArray = CreateVoxelArray());
			}
			set
			{
				this.voxelArray = value;
			}
		}

		private VoxelArray CreateVoxelArray()
		{
			var voxelArray = new VoxelArray(32, 32, 32);

			voxelArray.OutlineBox(1, 0, 0, 0, 31, 31, 31);
			for (int i = 0; i < 100; ++i)
			{
				int size = rnd.Next(3);
				var index = rnd.Next(voxelArray.SizeX * voxelArray.SizeY * voxelArray.SizeZ);
				var x = index % voxelArray.SizeX;
				index /= voxelArray.SizeX;
				var y = index % voxelArray.SizeY;
				index /= voxelArray.SizeY;
				var z = index % voxelArray.SizeZ;
				voxelArray.FillBox((byte)(rnd.Next(3) + 2), x, y, z, x + size, y + size, z + size);
			}
			return voxelArray;
		}

		private static Random rnd = new Random(0);

		private VoxelArray voxelArray;

	}
}