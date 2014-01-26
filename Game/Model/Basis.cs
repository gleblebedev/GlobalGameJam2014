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
			if (float.IsNaN(x.X) || float.IsNaN(y.X) || float.IsNaN(z.X))
			{
				ResetRotOnError();
				return;
			}
		}

		private void ResetRotOnError()
		{
			ResetRotation();
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
			if (float.IsNaN(x.X) || float.IsNaN(y.X) || float.IsNaN(z.X))
			{
				ResetRotOnError();
				return;
			}
		}

		public Basis Clone()
		{
			return new Basis(origin) {x = x,y = y,z =z};
		}

		public void SetRotation(Vector3 x, Vector3 z)
		{
			this.x = x;
			this.x.Normalize();
			this.y = Vector3.Cross(z, x);
			this.y.Normalize();
			this.z = Vector3.Cross(x, y);
			this.z.Normalize();
		}

		public void ResetRotation()
		{
			x = new Vector3(1, 0, 0);
			y = new Vector3(0, 1, 0);
			z = new Vector3(0, 0, 1);
		}

		public Matrix4 GetMatrix(float scale)
		{
			return new Matrix4(
				new Vector4(scale * x.X, scale * x.Y, scale * x.Z, 0),
				new Vector4(scale * y.X, scale * y.Y, scale * y.Z, 0),
				new Vector4(scale * z.X, scale * z.Y, scale * z.Z, 0),
				new Vector4(origin.X, origin.Y, origin.Z, 1)
				);
		}
	}
}