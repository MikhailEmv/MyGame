using System.Windows;

namespace _2DWar.Model
{
	public class PlayerModel
	{ 
		public int playerSpeed = 8;
		public bool jumpFlag;
		public Vector Position { get; private set; }

		public PlayerModel(Vector position, bool jumpFlag)
		{
			Position = position;
			this.jumpFlag = jumpFlag;
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