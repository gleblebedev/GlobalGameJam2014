using System;
using System.Windows.Forms;

namespace Game.Model
{
	public interface IScene
	{
		void Render(int width, int height);

		void Update(TimeSpan dt);

		void OnKeyDown(KeyEventArgs keyEventArgs);

		void OnKeyUp(KeyEventArgs keyEventArgs);

		void OnMouseMove(MouseEventArgs mouseEventArgs);
	}
}