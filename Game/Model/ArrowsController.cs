using System.Windows.Forms;

namespace Game.Model
{
	public class ArrowsController :BaseController, IController
	{
		public ArrowsController(IControlledCreature spider)
			: base(spider)
		{
		}
		public void OnKeyDown(KeyEventArgs keyEventArgs)
		{
			switch (keyEventArgs.KeyCode)
			{
				case Keys.J:
					this.left = true;
					keyEventArgs.Handled = true;
					break;
				case Keys.L:
					this.right = true;
					keyEventArgs.Handled = true;
					break;
				case Keys.I:
					this.forward = true;
					keyEventArgs.Handled = true;
					break;
				case Keys.K:
					this.backward = true;
					keyEventArgs.Handled = true;
					break;
			}
		}

		public void OnKeyUp(KeyEventArgs keyEventArgs)
		{
			switch (keyEventArgs.KeyCode)
			{
				case Keys.J:
					this.left = false;
					keyEventArgs.Handled = true;
					break;
				case Keys.L:
					this.right = false;
					keyEventArgs.Handled = true;
					break;
				case Keys.I:
					this.forward = false;
					keyEventArgs.Handled = true;
					break;
				case Keys.K:
					this.backward = false;
					keyEventArgs.Handled = true;
					break;
			}
		}
		public void OnMouseMove(MouseEventArgs mouseEventArgs)
		{
		}
	}
}