using UnityEngine;

namespace Edibles
{
	public abstract class AbstractEdiblesGenerator
	{
		public abstract Row GenerateRow();
	}

	public enum CellType
	{
		Empty,
		Bomb,
		Incorrect,
		Correct,
		Crystal
	}
}