using System;

using OpenTK;

namespace Game.Model
{
	public interface IViewports
	{
		float Pitch { get; set; }

		Basis Position { get; set; }

		void Render(int minX, int minY, int maxX, int maxY, Action renderCallback);
	}
}