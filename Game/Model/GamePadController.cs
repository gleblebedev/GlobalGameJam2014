using System;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Input;

using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace Game.Model
{
	public class GamePadController : BaseController, IController
	{
		private readonly int index;

		public GamePadController(int index,IControlledCreature spider):base(spider)
		{
			this.index = index;
		}

		#region Implementation of IController

		public void OnKeyDown(KeyEventArgs keyEventArgs)
		{
			
		}

		public void OnKeyUp(KeyEventArgs keyEventArgs)
		{
			
		}

		public override void Update(TimeSpan dt)
		{
			var state = OpenTK.Input.GamePad.GetState(index);
			base.Update(dt);

			forward = state.DPad.IsUp;
			backward = state.DPad.IsDown;
			trunLeft = state.DPad.IsLeft;
			trunRight = state.DPad.IsRight;
			if (!state.IsConnected)
				return;
			var walk = state.ThumbSticks.Left.Y;
			var rot = state.ThumbSticks.Left.X;
			var pitch = state.ThumbSticks.Right.Y;
			var eps = 0.15f;
			if (walk > -eps && walk < eps) walk = 0;
			if (rot > -eps && rot < eps) rot = 0;
			if (pitch > -eps && pitch < eps) pitch = 0;
			if (walk != 0)
			{
				this.Spider.Move(new Vector3(1,0,0), walk* (float)dt.TotalSeconds);
				this.Spider.IsInMove = true;
			}
			if (rot != 0)
			{
				this.Spider.Rotate(-rot*(float)dt.TotalSeconds);
			}
			if (pitch != 0)
			{
				this.Spider.Pitch += -(pitch * (float)dt.TotalSeconds);
			}
		}

		public void OnMouseMove(MouseEventArgs mouseEventArgs)
		{
			
		}

		public void Deactivate()
		{
			
		}

		#endregion
	}
}