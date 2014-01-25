using OpenTK.Graphics;

namespace Game.Model
{
	public class WorldMaterial
	{
		private Color4 color = Color4.White;

		public Color4 Color
		{
			get
			{
				return this.color;
			}
			set
			{
				this.color = value;
			}
		}

		public Texture Texture { get; set; }
	}
}