using System.Windows;

namespace _2DWar.Model
{
	public class BulletModel
	{
		public bool IsActive { get; private set; }
		public string Id { get; private set; }
		private int limitRightSide;
		private int bulletSpeed;
		public Vector Position { get; private set; }

		public BulletModel(string id, Vector position, int limitRightSide)
		{
			IsActive = true;
			this.limitRightSide = limitRightSide;
			Id = id;
			bulletSpeed = 80;
			Position = position;
		}

		public void Disactivation()
        {
			IsActive = false;
        }

		public Vector Move(Directions direction)
		{
			var X = Position.X;
			var Y = Position.Y;

			if (direction == Directions.Right && IsActive) X += bulletSpeed;
			if (X >= limitRightSide) IsActive = false;

			Position = new Vector(X, Y);
			return Position;
		}
	}
}