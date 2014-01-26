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
			bool isMoving = false;
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
				isMoving = true;
				this.spider.Move(new Vector3(0, 1, 0), scale);
			}
			if (!this.left && this.right)
			{
				isMoving = true;
				this.spider.Move(new Vector3(0, -1, 0), scale);
			}
			if (this.forward && !this.backward)
			{
				isMoving = true;
				this.spider.Move(new Vector3(1, 0, 0.3f), scale);
			}
			if (!this.forward && this.backward)
			{
				isMoving = true;
				this.spider.Move(new Vector3(-1, 0, 0), scale);
			}
			this.spider.IsInMove = isMoving;
		}
		protected BaseController(IControlledCreature spider)
		{
			this.spider = spider;
			
		}
	}
}