using System;

using OpenTK;

namespace Game.Model
{
	public class BaseController 
	{
		protected readonly IControlledCreature spider;

		protected bool left;

		protected bool right;

		protected bool forward;

		protected bool backward;

		protected bool trunLeft;

		protected bool trunRight;

		public void Update(TimeSpan dt)
		{
			var scale = (float)dt.TotalSeconds;
			if (this.trunLeft && !this.trunRight)
			{
				this.spider.Rotate(scale);
			}
			if (!this.trunLeft && this.trunRight)
			{
				this.spider.Rotate(-scale);
			}
			if (this.left && !this.right)
			{
				this.spider.Move(new Vector3(0, 1, 0), scale);
			}
			if (!this.left && this.right)
			{
				this.spider.Move(new Vector3(0, -1, 0), scale);
			}
			if (this.forward && !this.backward)
			{
				this.spider.Move(new Vector3(1, 0, 0.3f), scale);
			}
			if (!this.forward && this.backward)
			{
				this.spider.Move(new Vector3(-1, 0, 0), scale);
			}
		}
		protected BaseController(IControlledCreature spider)
		{
			this.spider = spider;
			
		}
	}
}