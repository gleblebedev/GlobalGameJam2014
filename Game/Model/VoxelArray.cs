namespace Game.Model
{
	public class VoxelArray
	{
		private readonly int sizeX;

		private readonly int sizeY;

		private readonly int sizeZ;

		private byte[,,] data;
		public VoxelArray(int sizeX, int sizeY, int sizeZ)
		{
			this.sizeX = sizeX;
			this.sizeY = sizeY;
			this.sizeZ = sizeZ;

			this.data = new byte[sizeX,sizeY,sizeZ];
		}

		public int SizeX { get
		{
			return sizeX;
		} }

		public int SizeY
		{
			get
			{
				return this.sizeY;
			}
		}

		public int SizeZ
		{
			get
			{
				return this.sizeZ;
			}
		}

		public void FillBox(byte type, int minX, int minY, int minZ,int maxX, int maxY, int maxZ)
		{
			for (int x = minX; x <= maxX; ++x)
				for (int y = minY; y <= maxY; ++y)
					for (int z = minZ; z <= maxZ; ++z)
					{
						this.data[x, y, z] = type;
					}
		}
		public void OutlineBox(byte type, int minX, int minY, int minZ, int maxX, int maxY, int maxZ)
		{
			this.FillBox(type, minX, minY, minZ, minX, maxY, maxZ);
			this.FillBox(type, maxX, minY, minZ, maxX, maxY, maxZ);
		}

		public int this[int x, int y, int z]
		{
			get
			{
				return this.data[x, y, z];
			}
		}
	}
}