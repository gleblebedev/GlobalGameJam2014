using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Game.Model
{
	public class World
	{
		private readonly VoxelArray voxels;

		private readonly MaterialMap materialMap;

		private List<VertexBuffer> vbs;

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
			EnsureVertexBuffers();
			foreach (var vertexBuffer in vbs)
			{
				var worldMaterial = materialMap[vertexBuffer.material];
				if (worldMaterial.Texture != null)
				{
					worldMaterial.Texture.Set(0);
				}
				else
				{
					GL.Disable(EnableCap.Texture2D);
				}
				GL.CullFace(CullFaceMode.Back);
				GL.Enable(EnableCap.CullFace);
				GL.Enable(EnableCap.DepthTest);
				vertexBuffer.Enable();
				GL.DrawArrays(BeginMode.Quads, 0, vertexBuffer.Count);
				vertexBuffer.Disable();
			}
			//for (int i = 0; i < 256; i++)
			//{

			//	var vertexBufferItems = vblist[i];
			//	if (vertexBufferItems != null)
			//	{
			//		//GL.CullFace(CullFaceMode.Front);
			//		//GL.Enable(EnableCap.CullFace);
			//		var worldMaterial = materialMap[i];
			//		if (worldMaterial.Texture != null)
			//		{
			//			worldMaterial.Texture.Set(0);
			//		}
			//		else
			//		{
			//			GL.Disable(EnableCap.Texture2D);
			//		}
			//		//GL.Begin(BeginMode.Quads);
			//		//foreach (var vertexBufferItem in vertexBufferItems)
			//		//{
			//		//	GL.Color4(vertexBufferItem.Color4);
			//		//	GL.TexCoord2(vertexBufferItem.UV);
			//		//	GL.Vertex3(vertexBufferItem.Pos);
			//		//}
			//		//GL.End();
			//	}
			//}
		}

		private void EnsureVertexBuffers()
		{
			if (vbs != null)
				return;
			Vector2[] quadUv = new Vector2[]
				{
					new Vector2(0, 0),
					new Vector2(1, 0),
					new Vector2(1, 1),
					new Vector2(0, 1),
				};
			var vblist = new List<VertexBufferItem>[256];
			for (int x = 0; x < this.SizeX; ++x)
				for (int y = 0; y < this.SizeY; ++y)
					for (int z = 0; z < this.SizeZ; ++z)
					{
						var a = this.voxels[x, y, z];
						if (a == MaterialMap.Empty )
							continue;

						var color4 = materialMap[a].Color;

						var items = vblist[a];
						if (items == null)
						{
							items = vblist[a] = new List<VertexBufferItem>();
						}
						var ambientOcclustion = 0.6f;


						if (this.voxels[x - 1, y, z] == MaterialMap.Empty)
						{
							Color4 c00, c01, c11, c10;
							c00 = c01 = c11 = c10 = color4;
							if (this.voxels[x - 1, y - 1, z] != MaterialMap.Empty)
							{
								ScaleColor(ref c00, ambientOcclustion);
								ScaleColor(ref c01, ambientOcclustion);
							}
							if (this.voxels[x - 1, y + 1, z] != MaterialMap.Empty)
							{
								ScaleColor(ref c10, ambientOcclustion);
								ScaleColor(ref c11, ambientOcclustion);
							}
							if (this.voxels[x - 1, y , z-1] != MaterialMap.Empty)
							{
								ScaleColor(ref c00, ambientOcclustion);
								ScaleColor(ref c10, ambientOcclustion);
							}
							if (this.voxels[x - 1, y, z+1 ] != MaterialMap.Empty)
							{
								ScaleColor(ref c01, ambientOcclustion);
								ScaleColor(ref c11, ambientOcclustion);
							}

							items.Add(new VertexBufferItem(new Vector3(x, y, z), c00, quadUv[0]));
							items.Add(new VertexBufferItem(new Vector3(x, y, z + 1), c01, quadUv[3]));
							items.Add(new VertexBufferItem(new Vector3(x, y + 1, z + 1), c11, quadUv[2]));
							items.Add(new VertexBufferItem(new Vector3(x, y + 1, z), c10, quadUv[1]));
						}
						if (this.voxels[x + 1, y, z] == MaterialMap.Empty)
						{
							Color4 c00, c01, c11, c10;
							c00 = c01 = c11 = c10 = color4;
							if (this.voxels[x + 1, y - 1, z] != MaterialMap.Empty)
							{
								ScaleColor(ref c00, ambientOcclustion);
								ScaleColor(ref c01, ambientOcclustion);
							}
							if (this.voxels[x + 1, y + 1, z] != MaterialMap.Empty)
							{
								ScaleColor(ref c10, ambientOcclustion);
								ScaleColor(ref c11, ambientOcclustion);
							}
							if (this.voxels[x + 1, y, z - 1] != MaterialMap.Empty)
							{
								ScaleColor(ref c00, ambientOcclustion);
								ScaleColor(ref c10, ambientOcclustion);
							}
							if (this.voxels[x + 1, y, z + 1] != MaterialMap.Empty)
							{
								ScaleColor(ref c01, ambientOcclustion);
								ScaleColor(ref c11, ambientOcclustion);
							}

							items.Add(new VertexBufferItem(new Vector3(x + 1, y, z), c00, quadUv[0]));
							items.Add(new VertexBufferItem(new Vector3(x + 1, y + 1, z), c10, quadUv[1]));
							items.Add(new VertexBufferItem(new Vector3(x + 1, y + 1, z + 1), c11, quadUv[2]));
							items.Add(new VertexBufferItem(new Vector3(x + 1, y, z + 1), c01, quadUv[3]));
						}
						if (this.voxels[x, y-1, z] == MaterialMap.Empty)
						{
							Color4 c00, c01, c11, c10;
							c00 = c01 = c11 = c10 = color4;
							if (this.voxels[x - 1, y - 1, z] != MaterialMap.Empty)
							{
								ScaleColor(ref c00, ambientOcclustion);
								ScaleColor(ref c01, ambientOcclustion);
							}
							if (this.voxels[x+1, y -1 , z] != MaterialMap.Empty)
							{
								ScaleColor(ref c10, ambientOcclustion);
								ScaleColor(ref c11, ambientOcclustion);
							}
							if (this.voxels[x , y-1, z - 1] != MaterialMap.Empty)
							{
								ScaleColor(ref c00, ambientOcclustion);
								ScaleColor(ref c10, ambientOcclustion);
							}
							if (this.voxels[x , y-1, z + 1] != MaterialMap.Empty)
							{
								ScaleColor(ref c01, ambientOcclustion);
								ScaleColor(ref c11, ambientOcclustion);
							}

							items.Add(new VertexBufferItem(new Vector3(x, y, z), c00, quadUv[0]));
							items.Add(new VertexBufferItem(new Vector3(x + 1, y, z), c10, quadUv[1]));
							items.Add(new VertexBufferItem(new Vector3(x + 1, y, z + 1), c11, quadUv[2]));
							items.Add(new VertexBufferItem(new Vector3(x, y, z + 1), c01, quadUv[3]));
						}
						if (this.voxels[x, y + 1, z] == MaterialMap.Empty)
						{
							Color4 c00, c01, c11, c10;
							c00 = c01 = c11 = c10 = color4;
							if (this.voxels[x - 1, y + 1, z] != MaterialMap.Empty)
							{
								ScaleColor(ref c00, ambientOcclustion);
								ScaleColor(ref c01, ambientOcclustion);
							}
							if (this.voxels[x + 1, y + 1, z] != MaterialMap.Empty)
							{
								ScaleColor(ref c10, ambientOcclustion);
								ScaleColor(ref c11, ambientOcclustion);
							}
							if (this.voxels[x, y + 1, z - 1] != MaterialMap.Empty)
							{
								ScaleColor(ref c00, ambientOcclustion);
								ScaleColor(ref c10, ambientOcclustion);
							}
							if (this.voxels[x, y + 1, z + 1] != MaterialMap.Empty)
							{
								ScaleColor(ref c01, ambientOcclustion);
								ScaleColor(ref c11, ambientOcclustion);
							}

							items.Add(new VertexBufferItem(new Vector3(x, y + 1, z), c00, quadUv[0]));
							items.Add(new VertexBufferItem(new Vector3(x, y + 1, z + 1), c01, quadUv[3]));
							items.Add(new VertexBufferItem(new Vector3(x + 1, y + 1, z + 1), c11, quadUv[2]));
							items.Add(new VertexBufferItem(new Vector3(x + 1, y + 1, z), c10, quadUv[1]));
						}
						if (this.voxels[x, y, z - 1] == MaterialMap.Empty)
						{
							Color4 c00, c01, c11, c10;
							c00 = c01 = c11 = c10 = color4;
							if (this.voxels[x - 1, y, z - 1] != MaterialMap.Empty)
							{
								ScaleColor(ref c00, ambientOcclustion);
								ScaleColor(ref c01, ambientOcclustion);
							}
							if (this.voxels[x + 1, y, z - 1] != MaterialMap.Empty)
							{
								ScaleColor(ref c10, ambientOcclustion);
								ScaleColor(ref c11, ambientOcclustion);
							}
							if (this.voxels[x, y - 1, z - 1] != MaterialMap.Empty)
							{
								ScaleColor(ref c00, ambientOcclustion);
								ScaleColor(ref c10, ambientOcclustion);
							}
							if (this.voxels[x, y + 1, z - 1] != MaterialMap.Empty)
							{
								ScaleColor(ref c01, ambientOcclustion);
								ScaleColor(ref c11, ambientOcclustion);
							}

							items.Add(new VertexBufferItem(new Vector3(x, y, z), c00, quadUv[0]));
							items.Add(new VertexBufferItem(new Vector3(x, y + 1, z), c01, quadUv[3]));
							items.Add(new VertexBufferItem(new Vector3(x + 1, y + 1, z), c11, quadUv[2]));
							items.Add(new VertexBufferItem(new Vector3(x + 1, y, z), c10, quadUv[1]));
						}
						if (this.voxels[x, y, z + 1] == MaterialMap.Empty)
						{
							Color4 c00, c01, c11, c10;
							c00 = c01 = c11 = c10 = color4;
							if (this.voxels[x - 1, y, z + 1] != MaterialMap.Empty)
							{
								ScaleColor(ref c00, ambientOcclustion);
								ScaleColor(ref c01, ambientOcclustion);
							}
							if (this.voxels[x + 1, y, z + 1] != MaterialMap.Empty)
							{
								ScaleColor(ref c10, ambientOcclustion);
								ScaleColor(ref c11, ambientOcclustion);
							}
							if (this.voxels[x, y - 1, z + 1] != MaterialMap.Empty)
							{
								ScaleColor(ref c00, ambientOcclustion);
								ScaleColor(ref c10, ambientOcclustion);
							}
							if (this.voxels[x, y + 1, z + 1] != MaterialMap.Empty)
							{
								ScaleColor(ref c01, ambientOcclustion);
								ScaleColor(ref c11, ambientOcclustion);
							}
							items.Add(new VertexBufferItem(new Vector3(x, y, z + 1), c00, quadUv[0]));
							items.Add(new VertexBufferItem(new Vector3(x + 1, y, z + 1), c11, quadUv[1]));
							items.Add(new VertexBufferItem(new Vector3(x + 1, y + 1, z + 1), c11, quadUv[2]));
							items.Add(new VertexBufferItem(new Vector3(x, y + 1, z + 1), c01, quadUv[3]));
						}
					}
			this.vbs = new List<VertexBuffer>();
			for (int index = 0; index < vblist.Length; index++)
			{
				var vertexBufferItem = vblist[index];
				if (vertexBufferItem != null && vertexBufferItem.Count > 0)
				{
					this.vbs.Add(new VertexBuffer((byte)index, vertexBufferItem));
				}
			}
		}

		private void ScaleColor(ref Color4 c00, float f)
		{
			c00 = new Color4((c00.R*f),(c00.G*f),(c00.B*f),c00.A);
		}
	}
}
