using System;

using OpenTK;

namespace Game.Model
{
	public interface IViewports
	{
		Vector3 Eye { get; set; }
		Vector3 Forward { get; set; }
		Vector3 Up { get; set; }
		void Render(int minX, int minY, int maxX, int maxY, Action renderCallback);
	}
}