namespace Game.Model
{
	public class PlayerOptions
	{
		private ControlType control = ControlType.WASD;

		private EyesCount eyes = EyesCount.FiveEye;

		public ControlType Control
		{
			get
			{
				return this.control;
			}
			set
			{
				this.control = value;
			}
		}

		public EyesCount Eyes
		{
			get
			{
				return this.eyes;
			}
			set
			{
				this.eyes = value;
			}
		}
	}
}