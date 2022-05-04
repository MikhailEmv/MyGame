using System.Windows;

namespace _2DWar.Model
{
    public class CloudsModel
    {
        private int LimitRightSide;
        private int backgroundSpeed;
        public Vector Position { get; private set; }

        public CloudsModel(Vector position, int limitRightSide)
        {
            LimitRightSide = limitRightSide;
            Position = position;
            backgroundSpeed = 20;
        }

		public Vector Move(Directions direction)
		{
			var X = Position.X;

			if (direction == Directions.Right)
            {
                X += backgroundSpeed;
                if (X >= LimitRightSide) X = 0;
            }

			Position = new Vector(X, 0);
			return Position;
		}
	}
}
