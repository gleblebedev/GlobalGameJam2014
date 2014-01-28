using System;

using OpenTK;

namespace Game.Model
{
	public class Spider : IControlledCreature
	{
		#region Constants and Fields

		private const float MaxPitch = 1;

		private readonly World world;

		private Basis animationEnd;

		private float animationProgress;

		private Basis animationStart;

		private Vector3 contactPointNormal = new Vector3(0, 0, 1);

		private bool isAimated;

		private float pitch;

		private Basis position = new Basis();

		private float speed = 3.0f;

		#endregion

		#region Constructors and Destructors

		public Spider(World world, Vector3 startPosition)
		{
			this.world = world;
			this.position.Origin = startPosition;
			this.UpdateBasis();
		}

		#endregion

		#region Public Properties

		public Vector3 LookDirection
		{
			get
			{
				var b = this.Position.Clone();
				b.Rotate(b.Y, this.Pitch);
				return b.X;
			}
		}

		public bool IsInMove { get; set; }
	    public int Score { get; set; }
	    public string Name { get; set; }

	    public float Pitch
		{
			get
			{
				return this.pitch;
			}
			set
			{
				this.pitch = Math.Max(-MaxPitch, Math.Min(MaxPitch, value));
			}
		}

		public Basis Position
		{
			get
			{
				return this.position;
			}
			private set
			{
				this.position = value;
			}
		}

		#endregion

		#region Public Methods and Operators

		public void Move(Vector3 direction, float stepScale)
		{
			if (this.isAimated)
			{
				return;
			}
			if (stepScale == 0)
			{
				return;
			}
			if (stepScale < 0)
			{
				direction = -direction;
				stepScale = -stepScale;
			}
			stepScale *= this.speed;
			// calculate direction in wold coordinates
			direction = (this.Position.X * direction.X + this.Position.Y * direction.Y + this.Position.Z * direction.Z);

			// calculate direction according to cuurent contact normal
			direction = direction - this.contactPointNormal * Vector3.Dot(direction, this.contactPointNormal);
			direction.Normalize();

			var prevPos = this.Position.Origin;
			var newPos = prevPos + direction * stepScale;

			Vector3 n;
			if (this.world.TraceBox(prevPos, ref newPos, out n))
			{
				this.StartAnimation(newPos, n);
				return;
			}
			prevPos = newPos;
			newPos = prevPos - this.contactPointNormal * 0.5f;
			if (!this.world.TraceBox(prevPos, ref newPos, out n))
			{
				prevPos = newPos;
				newPos = prevPos - direction;
				if (this.world.TraceBox(prevPos, ref newPos, out n))
				{
					this.StartAnimation(newPos, n);
				}
				return;
			}
			this.Position.Origin = prevPos;
		}

		public void Rotate(float angle)
		{
			if (this.isAimated)
			{
				return;
			}
			this.Position.Rotate(this.Position.Z, angle);
		}

		public void Update(TimeSpan dt)
		{
			if (this.isAimated)
			{
				this.animationProgress = (float)Math.Min(1, this.animationProgress + dt.TotalSeconds * this.speed);
				var kStart = (1 - this.animationProgress);
				var kEnd = (this.animationProgress);
				this.Position.Origin = this.animationStart.Origin * kStart + this.animationEnd.Origin * kEnd;
				var n = this.animationStart.Z * kStart + this.animationEnd.Z * kEnd;
				this.pitch = this.pitch * 0.7f;
				var x = Vector3.Cross(this.Position.Y, n);
				if (float.IsNaN(x.X) || x.LengthSquared <= 1e-6)
				{
					var y = Vector3.Cross(n, this.Position.X);
					x = Vector3.Cross(y, n);
				}

				this.Position.SetRotation(x, n);
				if (this.animationProgress >= 1.0f)
				{
					this.contactPointNormal = this.animationEnd.Z;
					this.isAimated = false;
				}
			}
		}

		#endregion

		#region Methods

		private bool StartAnimation(Vector3 newPos, Vector3 n)
		{
			var animateToPos = newPos + n * 0.5f;
			animateToPos.X = 0.5f + (float)Math.Floor(animateToPos.X);
			animateToPos.Y = 0.5f + (float)Math.Floor(animateToPos.Y);
			animateToPos.Z = 0.5f + (float)Math.Floor(animateToPos.Z);

			this.animationStart = this.Position.Clone();
			var x = Vector3.Cross(this.Position.Y, n);
			if (float.IsNaN(x.X) || x.LengthSquared <= 1e-6)
			{
				var y= Vector3.Cross(n, this.Position.X);
				x = Vector3.Cross(y, n);
			}
			this.animationEnd = new Basis(animateToPos, x.Normalized(), n);
			this.animationProgress = 0;
			if (!this.world.IsEmpty(animateToPos))
			{
				return true;
			}
			this.isAimated = true;
			return false;
		}

		private void UpdateBasis()
		{
		}

		#endregion
	}
}