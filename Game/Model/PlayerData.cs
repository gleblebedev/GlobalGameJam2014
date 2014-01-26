namespace Game.Model
{
	class PlayerData
	{
		private float movementFactor = 0.5f;

		public IViewports Viewport { get; set; }
		public IController Controller { get; set; }
		public IControlledCreature Creature { get; set; }

		public float MovementFactor
		{
			get
			{
				return this.movementFactor;
			}
			set
			{
				this.movementFactor = value;
			}
		}
	}
}