namespace Game.Model
{
	public class MaterialMap
	{
		public const byte Empty = 0;
		WorldMaterial[] lookup = new WorldMaterial[256];

		public WorldMaterial this[int index]
		{
			get
			{
				return lookup[index];
			}
			set
			{
				lookup[index] = value;
			}
		}
	}
}