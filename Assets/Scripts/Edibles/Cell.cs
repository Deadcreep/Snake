using System;
using UnityEngine;

namespace Edibles
{
	[Serializable]
	public class WeightedCell
	{
		public CellType Type;
		[Range(0, 1)] public float Weight;
	}
}