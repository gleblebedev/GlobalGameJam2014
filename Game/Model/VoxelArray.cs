using System;
using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Game.Model
{
	[JsonConverter(typeof(VoxelArrayJsonConverter))]
	public class VoxelArray:IEnumerable<byte>
	{
		private readonly int sizeX;

		private readonly int sizeY;

		private readonly int sizeZ;

		private int version = 0;

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

		public int Version
		{
			get
			{
				return this.version;
			}
		}

		public void FillBox(byte type, int minX, int minY, int minZ,int maxX, int maxY, int maxZ)
		{
			for (int x = minX; x <= maxX && x<SizeX; ++x)
				for (int y = minY; y <= maxY && y<SizeY; ++y)
					for (int z = minZ; z <= maxZ && z<SizeZ; ++z)
					{
						this.data[x, y, z] = type;
					}
			IncVersion();
		}
		public void OutlineBox(byte type, int minX, int minY, int minZ, int maxX, int maxY, int maxZ)
		{
			this.FillBox(type, minX, minY, minZ, minX, maxY, maxZ);
			this.FillBox(type, maxX, minY, minZ, maxX, maxY, maxZ);

			this.FillBox(type, minX, minY, minZ, maxX, minY, maxZ);
			this.FillBox(type, minX, maxY, minZ, maxX, maxY, maxZ);

			this.FillBox(type, minX, minY, minZ, maxX, maxY, minZ);
			this.FillBox(type, minX, minY, maxZ, maxX, maxY, maxZ);

            //SomeRandomBullshit
            this.FillBox(1, 10, 1, 10, 12, 2, 13);
			IncVersion();
		}

		private void IncVersion()
		{
			++version;
		}

		public int this[int x, int y, int z]
		{
			get
			{
				return this.data[
					Math.Max(0,Math.Min(x,sizeX-1)),
					Math.Max(0,Math.Min(y,sizeY-1)),
					Math.Max(0,Math.Min(z,sizeZ-1))
					];
			}
		}
		public IEnumerator<byte> GetEnumerator()
		{
			for (int x = 0; x < SizeX; ++x)
				for (int y = 0; y < SizeY; ++y)
					for (int z = 0; z < SizeZ; ++z)
					{
						yield return this.data[x, y, z];
					}
		}

		#region Implementation of IEnumerable

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}
}