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
	public enum EyesCount
	{
		Blind = 0,
		/// <summary>
		/// One Eye. Tribute to Mads Mikkelsen!
		/// </summary>
		OneEye = 1,
		Fly = 2,
		ThreeEye = 3,
		Medium = 4,
		FiveEye =5,
		SixEye = 6,
		SevenEye = 7,
		Spider = 8
	}
}