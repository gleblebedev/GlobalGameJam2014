using System;

using OpenTK;

namespace Game.Model
{
	public class SimpleSpider : IControlledCreature
	{
		#region Constants and Fields

		private readonly World world;

		private Basis position = new Basis();

		private float pitch;

		#endregion

		#region Constructors and Destructors

		public SimpleSpider(World world, Vector3 startPosition)
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

		bool isAimated;

		private Vector3 contactPointNormal = new Vector3(0,0,1);

		private float animationProgress;

		private Basis animationStart;

		private Basis animationEnd;

		private float speed = 3.0f;

		public void Update(TimeSpan dt)
		{
			if (isAimated)
			{
				animationProgress = (float)Math.Min(1, animationProgress + dt.TotalSeconds*speed);
				var kStart = (1 - animationProgress);
				var kEnd = (animationProgress);
				this.Position.Origin = this.animationStart.Origin * kStart + this.animationEnd.Origin * kEnd;
				var n = this.animationStart.Z * kStart + this.animationEnd.Z * kEnd;
				var x = Vector3.Cross(this.Position.Y, n);
				Position.SetRotation(x,n);
				if (animationProgress >= 1.0f)
				{
					this.contactPointNormal = this.animationEnd.Z;
					isAimated = false;
				}
			}
		}

		public void Move(Vector3 direction, float stepScale)
		{
			if (isAimated)
				return;
			if (stepScale == 0)
				return;
			if (stepScale < 0)
			{
				direction = -direction;
				stepScale = -stepScale;
			}
			stepScale *= speed;
			// calculate direction in wold coordinates
			direction = (Position.X * direction.X + Position.Y * direction.Y + Position.Z * direction.Z);

			// calculate direction according to cuurent contact normal
			direction = direction - contactPointNormal*Vector3.Dot(direction, contactPointNormal);
			direction.Normalize();

			var prevPos = Position.Origin;
			var newPos = prevPos + direction * stepScale;

			Vector3 n;
			if (world.TraceBox(prevPos, ref newPos, out n))
			{
				this.StartAnimation(newPos, n);
				return;
			}
			prevPos = newPos;
			newPos = prevPos - contactPointNormal*0.5f;
			if (!world.TraceBox(prevPos, ref newPos, out n))
			{
				prevPos = newPos;
				newPos = prevPos-direction;
				if (world.TraceBox(prevPos, ref newPos, out n))
				{
					this.StartAnimation(newPos, n);
				}
				return;
			}
			Position.Origin = prevPos;
		}

		private bool StartAnimation(Vector3 newPos, Vector3 n)
		{
			var animateToPos = newPos + n * 0.5f;
			animateToPos.X = 0.5f + (float)Math.Floor(animateToPos.X);
			animateToPos.Y = 0.5f + (float)Math.Floor(animateToPos.Y);
			animateToPos.Z = 0.5f + (float)Math.Floor(animateToPos.Z);

			this.animationStart = this.Position.Clone();
			var x = Vector3.Cross(this.Position.Y, n);
			this.animationEnd = new Basis(animateToPos, x.Normalized(), n);
			this.animationProgress = 0;
			if (!this.world.IsEmpty(animateToPos))
			{
				return true;
			}
			this.isAimated = true;
			return false;
		}

		public void Rotate(float angle)
		{
			if (isAimated)
				return;
			Position.Rotate(Position.Z, angle);
		}
		private const float MaxPitch = 1;
		#endregion

		#region Methods

		private void UpdateBasis()
		{
		}

		#endregion
	}
}