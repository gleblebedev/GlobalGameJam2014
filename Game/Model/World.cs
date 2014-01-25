﻿using System;
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
							items.Add(new VertexBufferItem(new Vector3(x + 1, y, z + 1), c10, quadUv[1]));
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

		public bool IsEmpty(int x, int y, int z)
		{
			if (voxels[x, y, z] == MaterialMap.Empty) return true;
			return false;
		}
		public bool IsEmpty(float x, float y, float z)
		{
			var intx = (int)Math.Floor(x);
			var inty = (int)Math.Floor(y);
			var intz = (int)Math.Floor(z);
			if (voxels[intx, inty, intz] == MaterialMap.Empty) return true;
			return false;
		}
		public bool IsEmpty(Vector3 v)
		{
			return IsEmpty(v.X,v.Y,v.Z);
		}
		public bool TraceRay(Vector3 origin, Vector3 destination, out Vector3 contactPoint, out Vector3 contactPointNormal)
		{
			Vector3 n = Vector3.Zero;
			bool res = false;
			var dx = destination.X - origin.X;
			if (dx > 1e-6)
			{
				res |= this.TracePosX(origin, ref destination, ref n);
			}
			else if (dx < -1e-6)
			{
				res |= this.TraceNegX(origin, ref destination, ref n);
			}
			var dy = destination.Y - origin.Y;
			if (dy > 1e-6)
			{
				res |= this.TracePosY(origin, ref destination, ref n);
			}
			else if (dy < -1e-6)
			{
				res |= this.TraceNegY(origin, ref destination, ref n);
			}
			var dz = destination.Z - origin.Z;
			if (dz > 1e-6)
			{
				res |= this.TracePosZ(origin, ref destination, ref n);
			}
			else if (dz< -1e-6)
			{
				res |= this.TraceNegZ(origin, ref destination, ref n);
			}
			contactPoint = destination;
			contactPointNormal = n;
			return res;
		}
		private bool TraceNegX(Vector3 origin, ref Vector3 destination, ref Vector3 n)
		{
			float dx = destination.X - origin.X;
			int x = (int)Math.Floor(origin.X);
			while (x > destination.X)
			{
				var scale = (x - origin.X) / dx;
				var point = origin * (1.0f - scale) + destination * scale;
				if (!IsEmpty(point - new Vector3(0.5f, 0, 0)))
				{
					destination = point;
					n = new Vector3(1, 0, 0);
					return true;
				}
				--x;
			}
			return false;
		}
		private bool TraceNegY(Vector3 origin, ref Vector3 destination, ref Vector3 n)
		{
			float dy = destination.Y - origin.Y;
			int x = (int)Math.Floor(origin.Y);
			while (x > destination.Y)
			{
				var scale = (x - origin.Y) / dy;
				var point = origin * (1.0f - scale) + destination * scale;
				if (!IsEmpty(point - new Vector3(0,0.5f, 0)))
				{
					destination = point;
					n = new Vector3(0, 1, 0);
					return true;
				}
				--x;
			}
			return false;
		}
		private bool TraceNegZ(Vector3 origin, ref Vector3 destination, ref Vector3 n)
		{
			float dz = destination.Z - origin.Z;
			int z = (int)Math.Floor(origin.Z);
			while (z > destination.Z)
			{
				var scale = (z - origin.Z) / dz;
				var point = origin*(1.0f-scale)  + destination*scale;
				if (!IsEmpty(point - new Vector3(0,0,0.5f)))
				{
					destination = point;
					n = new Vector3(0, 0, 1);
					return true;
				}
				--z;
			}
			return false;
		}
		private bool TracePosX(Vector3 origin, ref Vector3 destination, ref Vector3 n)
		{
			float dz = destination.X - origin.X;
			int x = 1 + (int)Math.Floor(origin.X);
			while (x < destination.X)
			{
				var scale = (x - origin.X) / dz;
				var point = origin * (1.0f - scale) + destination * scale;
				if (!IsEmpty(point + new Vector3(0.5f, 0, 0)))
				{
					destination = point;
					n = new Vector3(-1,0, 0);
					return true;
				}
				++x;
			}
			return false;
		}
		private bool TracePosY(Vector3 origin, ref Vector3 destination, ref Vector3 n)
		{
			float dy = destination.Y - origin.Y;
			int x = 1 + (int)Math.Floor(origin.Y);
			while (x < destination.Y)
			{
				var scale = (x - origin.Y) / dy;
				var point = origin * (1.0f - scale) + destination * scale;
				if (!IsEmpty(point + new Vector3(0, 0.5f, 0)))
				{
					destination = point;
					n = new Vector3(0, -1,0);
					return true;
				}
				++x;
			}
			return false;
		}
		private bool TracePosZ(Vector3 origin, ref Vector3 destination, ref Vector3 n)
		{
			float dz = destination.Z - origin.Z;
			int z = 1+(int)Math.Floor(origin.Z);
			while (z < destination.Z)
			{
				var scale = (z - origin.Z) / dz;
				var point = origin * (1.0f - scale) + destination * scale;
				if (!IsEmpty(point + new Vector3(0, 0, 0.5f)))
				{
					destination = point;
					n = new Vector3(0, 0, -1);
					return true;
				}
				++z;
			}
			return false;
		}
		private int  NextPositiveValue(float f)
		{
			var v = (int)Math.Floor(f);
			return (int)(v + 1);
		}
		private int NextNegativeValue(float f)
		{
			var v = (int)Math.Floor(f);
			return (int)v;
		}
	}
}
