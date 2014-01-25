using System;
using System.Windows.Forms;

namespace Game.Model
{
	public interface IController
	{
		void OnKeyDown(KeyEventArgs keyEventArgs);

		void OnKeyUp(KeyEventArgs keyEventArgs);

		void Update(TimeSpan dt);

		void OnMouseMove(MouseEventArgs mouseEventArgs);
	}
}