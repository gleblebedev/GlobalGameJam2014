namespace Game.Model
{
	class PlayerData
	{
		public IViewports Viewport { get; set; }
		public IController Controller { get; set; }

		public IControlledCreature Creature { get; set; }
	}
}