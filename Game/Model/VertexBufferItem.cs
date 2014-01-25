using System.Runtime.InteropServices;

using OpenTK;
using OpenTK.Graphics;

namespace Game.Model
{
	[StructLayout(LayoutKind.Sequential)]
	public struct VertexBufferItem
	{
		public Vector3 Pos;
		public Color4 Color4;

		public Vector2 UV;

		public static int stride = 4*(3+4+2);

		public VertexBufferItem(Vector3 vector3, Color4 color4, Vector2 uv)
		{
			this.Pos = vector3;
			this.Color4 = color4;
			this.UV = uv;
		}
	}
}