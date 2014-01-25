using System.Drawing;
using System.Drawing.Imaging;

using OpenTK.Graphics.OpenGL;

namespace Game.Model
{
	public class Texture
	{
		private readonly Bitmap bmp;

		int id;
		public Texture(Bitmap bmp)
		{
			this.bmp = bmp;
		}

		public void Set(int i)
		{
			GL.ActiveTexture(TextureUnit.Texture0 + i);
			if (this.id == 0)
			{
				this.id = GL.GenTexture();
				GL.BindTexture(TextureTarget.Texture2D, id);

				BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

				GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
					OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

				bmp.UnlockBits(bmp_data);

				// We haven't uploaded mipmaps, so disable mipmapping (otherwise the texture will not appear).
				// On newer video cards, we can use GL.GenerateMipmaps() or GL.Ext.GenerateMipmaps() to create
				// mipmaps automatically. In that case, use TextureMinFilter.LinearMipmapLinear to enable them.
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
			}
			GL.BindTexture(TextureTarget.Texture2D, this.id);
			GL.Enable(EnableCap.Texture2D);
		}
	}
}