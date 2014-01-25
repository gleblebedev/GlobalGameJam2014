using OpenTK;

namespace Game.Model
{
	public class Basis
	{
		private Vector3 origin = new Vector3();
		private Vector3 x = new Vector3(1,0,0);
		private Vector3 y = new Vector3(0,1,0);
		private Vector3 z = new Vector3(0,0,1);

		public Basis()
		{
		}
		public Basis(Vector3 origin)
		{
			this.origin = origin;
		}
		public Basis(Vector3 origin, Vector3 forward, Vector3 up)
		{
			this.origin = origin;
			x = forward;
			x.Normalize();
			z = up;
			y = Vector3.Cross(z, x);
			y.Normalize();
			z = Vector3.Cross(x, y);
			z.Normalize();
		}

		public Vector3 Origin
		{
			get
			{
				return this.origin;
			}
			set
			{
				this.origin = value;
			}
		}

		public Vector3 X
		{
			get
			{
				return this.x;
			}
		}

		public Vector3 Y
		{
			get
			{
				return this.y;
			}
		}

		public Vector3 Z
		{
			get
			{
				return this.z;
			}
		}

		public Vector3 VectorToWorld(Vector3 vector)
		{
			return vector.X * this.X + vector.Y * this.Y + vector.Z * this.Z;
		}
		public Vector3 ToWorld(Vector3 position)
		{
			return this.origin + position.X * this.X + position.Y * this.Y + position.Z * this.Z;
		}

		public void Rotate(Vector3 axis, float angle)
		{
			Quaternion q = Quaternion.FromAxisAngle(axis,angle);
			x = Vector3.Transform(x, q);
			y = Vector3.Transform(y, q);
			z = Vector3.Cross(x,y);
			z.Normalize();
		}

		public Basis Clone()
		{
			return new Basis(origin) {x = x,y = y,z =z};
		}
	}
}