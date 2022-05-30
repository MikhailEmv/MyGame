using System;
using System.Windows;

namespace _2DWar.Model
{
    public class EnemyModel
    {
		public bool IsAlive { get; private set; }
		private Random rnd;
		public int enemySpeed = 3;
		public Vector Position { get; private set; }
		public string Id { get; private set; }
		public int Height { get; private set; }
		public int Width { get; private set; }

		public EnemyModel(string id, Vector position, int height, int width)
		{
			IsAlive = true;
			rnd = new Random();
			Id = id;
			Position = position;
			Height = height;
			Width = width;
		}

		public void Mortification()
        {
			IsAlive = false;
        }

		private void ChangeSize()
		{
			var newSize = rnd.Next(80, 100);
			Height = newSize;
			Width = newSize;
		}

		public void ChangeSizeForNewEnemies()
		{
			var newSize = rnd.Next(100, 130);
			Height = newSize;
			Width = newSize;
		}

		public Vector Move(Directions direction)
		{
			var X = Position.X;
			var Y = Position.Y;

			if (direction == Directions.Left && IsAlive)
            {
				X -= enemySpeed; 
            }

			if (X < 0) 
			{
				IsAlive = false;
			} 

			ChangeSize();

			Position = new Vector(X, Y);
			return Position;
		}
	}
}