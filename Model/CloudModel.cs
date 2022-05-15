using System.Windows;

namespace _2DWar.Model
{
    public class CloudModel
    {
        private int limitRightSide;
        private int backgroundSpeed;
        public int Height { get; private set; }
        public int Width { get; private set; }

        public Vector Position { get; private set; }

        public CloudModel(Vector position, int limitRightSide, int height, int width)
        {
            this.limitRightSide = limitRightSide;
            Height = height;
            Width = width;
            Position = position;
            backgroundSpeed = 5;
        }

		public Vector Move(Directions direction)
		{
			var X = Position.X;
            var Y = Position.Y;

			if (direction == Directions.Right)
            {
                X += backgroundSpeed;
                if (X >= limitRightSide) X = 0;
            }

			Position = new Vector(X, Y);
			return Position;
		}
	}
}