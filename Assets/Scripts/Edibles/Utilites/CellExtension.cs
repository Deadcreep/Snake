namespace Edibles.Utilites
{
	public static class CellExtension
	{
		public static bool IsNegative(this CellType type)
		{
			return type == CellType.Bomb || type == CellType.Incorrect;
		}

		public static bool IsPositive(this CellType type)
		{
			return type == CellType.Crystal || type == CellType.Correct;
		}
	}
}