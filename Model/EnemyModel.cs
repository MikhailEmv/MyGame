using System.Windows;

namespace _2DWar.Model
{
    public class EnemyModel
    {
		private int enemySpeed;
		public Vector Position { get; private set; }

		public EnemyModel(Vector position)
		{
			Position = position;
			enemySpeed = 4;
		}

		public Vector Move(Directions direction)
		{
			var X = Position.X;
			var Y = Position.Y;

			switch (direction)
			{
				case (Directions.Down):
					Y += enemySpeed;
					break;
				case (Directions.Up):
					Y -= enemySpeed;
					break;
				case (Directions.Left):
					X -= enemySpeed;
					break;
				case (Directions.Right):
					X += enemySpeed;
					break;
			}

			Position = new Vector(X, Y);
			return Position;
		}
	}
}
