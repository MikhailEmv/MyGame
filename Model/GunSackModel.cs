using System;
using System.Windows;

namespace _2DWar.Model
{
	public class GunSackModel
	{
		private int gunSackSpeed;
		public bool IsAlreadyCought { get; private set; }
		public Vector Position { get; private set; }
		public string Id { get; private set; }

		public GunSackModel(string id, Vector position)
		{
			IsAlreadyCought = true;
			Id = id;
			Position = position;
			gunSackSpeed = 2;
		}

		public void Pretermission()
		{
			IsAlreadyCought = false;
		}

		public Vector Move(Directions direction)
		{
			var X = Position.X;
			var Y = Position.Y;

			if (direction == Directions.Down) 
			{
				Y += gunSackSpeed;
			}

			if (Y > 700) 
			{
				IsAlreadyCought = false;
			}

			Position = new Vector(X, Y);
			return Position;
		}
	}
}