using System.Windows;

namespace _2DWar.Model
{
	public class BulletModel
	{
		private int bulletSpeed;
		public Vector Position { get; private set; }

		public BulletModel(Vector position)
		{
			Position = position;
			bulletSpeed = 80;
		}

		public Vector Move(Directions direction)
		{
			var X = Position.X;

			if (direction == Directions.Right) X += bulletSpeed;

			Position = new Vector(X, 0);
			return Position;
		}
	}
}