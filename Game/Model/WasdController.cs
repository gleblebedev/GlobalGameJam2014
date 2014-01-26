using System.Drawing;
using System.Windows.Forms;

namespace Game.Model
{
	public class WasdController :BaseController, IController
	{

		private Point? mouseLocation;


		public WasdController(IControlledCreature spider):base(spider)
		{
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
						spider.Rotate(dx * 0.002f);
					}
				}
			}
			this.mouseLocation = mouseEventArgs.Location;
		}

		
	}
}