using System.Linq;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Game.Model
{
	public class IndexStream3D
	{
		public string Key { get; set; }
		public int Channel { get; set; }
		public int[] Data { get; set; }
	}
	public class Stream3D
	{
		public string Key { get; set; }
		public int Channel { get; set; }

		public Vector3[] Data { get; set; }
	}
	public class Geometry3D
	{
		public Stream3D[]Streams { get; set; }
		public Submesh3D[] Submeshes { get; set; }
	}
	public class Submesh3D
	{
		public IndexStream3D[] Streams { get; set; }
	}
	public class Model3D
	{
		public Geometry3D[] Geometries { get; set; }

		public void Render(Basis position, float opacity)
		{
			if (opacity >= 1)
				return;
			var geo = Geometries.FirstOrDefault();
			if (geo == null)
				return;
			var posVB = geo.Streams.FirstOrDefault(x => x.Key == "Position");
			foreach (var submesh3D in geo.Submeshes)
			{
				var posIB = submesh3D.Streams.FirstOrDefault(x => x.Key == "Position");
				if (posIB != null && posVB != null)
				{
					GL.MatrixMode(MatrixMode.Modelview);
					GL.PushMatrix();
					var m = position.GetMatrix(0.01f);
					GL.MultMatrix(ref m);
					if (opacity > 0)
					{
						GL.Enable(EnableCap.Blend);
						GL.BlendFunc(BlendingFactorSrc.SrcAlpha,BlendingFactorDest.OneMinusSrcAlpha);
					}
					GL.Begin(BeginMode.Triangles);
					if (opacity > 0)
					{
						GL.Color4(new Color4(0.0f, 0.0f, 0.0f, 1.0f - opacity));
					}
					else
						GL.Color4(new Color4(0.0f, 0.0f, 0.0f, 1.0f));
					foreach (var i in posIB.Data)
					{
						GL.Vertex3(posVB.Data[i]);
					}
					GL.End();
					GL.Disable(EnableCap.Blend);
					GL.PopMatrix();
				}
			}

		}
	}
}