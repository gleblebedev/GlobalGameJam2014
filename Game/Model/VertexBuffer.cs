using System;
using System.Collections.Generic;
using System.Linq;

using OpenTK.Graphics.OpenGL;

namespace Game.Model
{
	public class VertexBuffer
	{
		public readonly byte material;

		private int buffer;

		public VertexBuffer(byte material, ICollection<VertexBufferItem> items)
		{
			this.material = material;
			this.buffer = GL.GenBuffer();
			this.Count = items.Count;
			GL.BindBuffer(BufferTarget.ArrayBuffer,this.buffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(items.Count * VertexBufferItem.stride), items.ToArray(), BufferUsageHint.StaticDraw);
		}

		public int Count { get; private set; }

		public void Disable()
		{
			GL.DisableClientState(ArrayCap.VertexArray);
			GL.DisableClientState(ArrayCap.ColorArray);
			GL.DisableClientState(ArrayCap.TextureCoordArray);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		}
		public void Enable()
		{
			GL.EnableClientState(ArrayCap.VertexArray);
			GL.EnableClientState(ArrayCap.ColorArray);
			GL.EnableClientState(ArrayCap.TextureCoordArray);

			GL.BindBuffer(BufferTarget.ArrayBuffer, this.buffer);
			GL.VertexPointer(3, VertexPointerType.Float, VertexBufferItem.stride, new IntPtr(0));
			GL.ColorPointer(4, ColorPointerType.Float, VertexBufferItem.stride, new IntPtr(4*3));
			GL.TexCoordPointer(2, TexCoordPointerType.Float, VertexBufferItem.stride, new IntPtr(4*(3+4)));
		}

		public void Dispose()
		{
			if (buffer != 0)
			{
				this.Disable();
				GL.DeleteBuffer(buffer);
			}
		}
	}
}