using System;
using System.Collections.Generic;

namespace Edibles
{
	[Serializable]
	public class Row
	{
		public List<CellType> Cells = new List<CellType>();

		public CellType this[int index]
		{
			get => Cells[index];
			set => Cells[index] = value;
		}
	}
}