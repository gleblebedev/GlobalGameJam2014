using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;

namespace Game.Model
{
	public class World
	{
		private readonly VoxelArray voxels;

		private readonly MaterialMap materialMap;

		public World(VoxelArray voxels,MaterialMap materialMap)
		{
			this.voxels = voxels;
			this.materialMap = materialMap;
		}

		public int SizeX
		{
			get
			{
				return voxels.SizeX;
			}
		}

		public int SizeY
		{
			get
			{
				return voxels.SizeY;
			}
		}
		public int SizeZ
		{
			get
			{
				return voxels.SizeZ;
			}
		}

		public WorldMaterial GetMaterialAt(int x, int y, int z)
		{
			return materialMap[voxels[x, y, z]];
		}

		public void Render()
		{
			for (int x = 0; x < this.SizeX; ++x)
				for (int y = 0; y < this.SizeY; ++y)
					for (int z = 0; z < this.SizeZ; ++z)
					{
						this.RenderVoxel(x, y, z, this.GetMaterialAt(x, y, z));
					}
		}

		private void RenderVoxel(int x, int y, int z, WorldMaterial worldMaterial)
		{
			if (worldMaterial == null)
				return;
			GL.CullFace(CullFaceMode.Front);
			GL.Enable(EnableCap.CullFace);
			if (worldMaterial.Texture != null)
			{
				worldMaterial.Texture.Set(0);
			}
			GL.Begin(PrimitiveType.Quads);

			GL.Color4(worldMaterial.Color);
			GL.TexCoord2(0, 0);
			GL.Vertex3(x, y, z);
			GL.TexCoord2(1, 0);
			GL.Vertex3(x, y + 1, z);
			GL.TexCoord2(1, 1);
			GL.Vertex3(x, y + 1, z + 1);
			GL.TexCoord2(0, 1);
			GL.Vertex3(x, y, z + 1);

			GL.TexCoord2(0, 0);
			GL.Vertex3(x + 1, y, z);
			GL.TexCoord2(0, 1);
			GL.Vertex3(x + 1, y, z + 1);
			GL.TexCoord2(1, 1);
			GL.Vertex3(x + 1, y + 1, z + 1);
			GL.TexCoord2(1, 0);
			GL.Vertex3(x + 1, y + 1, z);

			GL.TexCoord2(0, 0);
			GL.Vertex3(x, y, z);
			GL.TexCoord2(0, 1);
			GL.Vertex3(x, y, z + 1);
			GL.TexCoord2(1, 1);
			GL.Vertex3(x + 1, y, z + 1);
			GL.TexCoord2(1, 0);
			GL.Vertex3(x + 1, y, z);

			GL.TexCoord2(0, 0);
			GL.Vertex3(x, y + 1, z);
			GL.TexCoord2(1, 0);
			GL.Vertex3(x + 1, y + 1, z);
			GL.TexCoord2(1, 1);
			GL.Vertex3(x + 1, y + 1, z + 1);
			GL.TexCoord2(0, 1);
			GL.Vertex3(x, y + 1, z + 1);

			GL.TexCoord2(0, 0);
			GL.Vertex3(x, y, z);
			GL.TexCoord2(1, 0);
			GL.Vertex3(x + 1, y, z);
			GL.TexCoord2(1, 1);
			GL.Vertex3(x + 1, y + 1, z);
			GL.TexCoord2(0, 1);
			GL.Vertex3(x, y + 1, z);

			GL.TexCoord2(0, 0);
			GL.Vertex3(x, y, z + 1);
			GL.TexCoord2(0, 1);
			GL.Vertex3(x, y + 1, z + 1);
			GL.TexCoord2(1, 1);
			GL.Vertex3(x + 1, y + 1, z + 1);
			GL.TexCoord2(1, 0);
			GL.Vertex3(x + 1, y, z + 1);

			GL.End();
		}
	}
}
