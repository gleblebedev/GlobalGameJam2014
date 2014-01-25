using System;
using System.Drawing;
using System.Windows.Forms;

using OpenTK;

namespace Game.Model
{
	public class WasdController : IController
	{
		private readonly IControlledCreature spider;

		private bool left;

		private bool right;

		private bool forward;

		private bool backward;

		private Point? mouseLocation;

		private bool trunLeft;

		private bool trunRight;

		public WasdController(IControlledCreature spider)
		{
			this.spider = spider;
		}

		public void OnKeyDown(KeyEventArgs keyEventArgs)
		{
			switch (keyEventArgs.KeyCode)
			{
				case Keys.A:
					this.left = true;
					keyEventArgs.Handled = true;
					break;
				case Keys.D:
					this.right = true;
					keyEventArgs.Handled = true;
					break;
				case Keys.Q:
					this.trunLeft = true;
					keyEventArgs.Handled = true;
					break;
				case Keys.E:
					this.trunRight = true;
					keyEventArgs.Handled = true;
					break;
				case Keys.W:
					this.forward = true;
					keyEventArgs.Handled = true;
					break;
				case Keys.S:
					this.backward = true;
					keyEventArgs.Handled = true;
					break;
			}
		}

		public void OnKeyUp(KeyEventArgs keyEventArgs)
		{
			switch (keyEventArgs.KeyCode)
			{
				case Keys.A:
					this.left = false;
					keyEventArgs.Handled = true;
					break;
				case Keys.D:
					this.right = false;
					keyEventArgs.Handled = true;
					break;
				case Keys.Q:
					this.trunLeft = false;
					keyEventArgs.Handled = true;
					break;
				case Keys.E:
					this.trunRight = false;
					keyEventArgs.Handled = true;
					break;
				case Keys.W:
					this.forward = false;
					keyEventArgs.Handled = true;
					break;
				case Keys.S:
					this.backward = false;
					keyEventArgs.Handled = true;
					break;
			}
		}

		public void Update(TimeSpan dt)
		{
			var scale = (float)dt.TotalSeconds;
			if (trunLeft && !trunRight)
			{
				spider.Rotate(scale);
			}
			if (!trunLeft && trunRight)
			{
				spider.Rotate(-scale);
			}
			if (left && !right)
			{
				spider.Move(new Vector3(0,1,0), scale);
			}
			if (!left && right)
			{
				spider.Move(new Vector3(0, -1, 0), scale);
			}
			if (forward && !backward)
			{
				spider.Move(new Vector3(1, 0, 0.3f), scale);
			}
			if (!forward && backward)
			{
				spider.Move(new Vector3(-1, 0, 0), scale);
			}
		}

		public void OnMouseMove(MouseEventArgs mouseEventArgs)
		{
			if (mouseLocation != null)
			{
				if ((mouseEventArgs.Button & MouseButtons.Left) == MouseButtons.Left)
				{
					var dy = mouseEventArgs.Location.Y - mouseLocation.Value.Y;
					var dx = mouseEventArgs.Location.X - mouseLocation.Value.X;
					if (dy != 0)
						spider.Pitch -= dy*0.002f;
					if (dx != 0)
					{
						spider.Position.Rotate(spider.Position.Z, dx * 0.002f);
					}
				}
			}
			this.mouseLocation = mouseEventArgs.Location;
		}
	}
}