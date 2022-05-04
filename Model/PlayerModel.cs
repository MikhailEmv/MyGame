using System.Windows;

namespace _2DWar.Model
{
	public class PlayerModel
	{ 
		private int playerSpeed;
		public Vector Position { get; private set; }

		public PlayerModel(Vector position)
		{
			Position = position;
			playerSpeed = 4;
		}

		public Vector Move(Directions direction)
		{
			var X = Position.X;
			var Y = Position.Y;

			switch (direction)
            {
				case (Directions.Down):
					Y += playerSpeed;
					break;
				case (Directions.Up):
					Y -= playerSpeed;
					break;
				case (Directions.Left):
					X -= playerSpeed;
					break;
				case (Directions.Right):
					X += playerSpeed;
					break;
			}

			Position = new Vector(X, Y);
			return Position;
		}
	}
}