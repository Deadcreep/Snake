using Edibles.Utilites;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Edibles
{
	public class EdiblesGenerator : AbstractEdiblesGenerator
	{
		private int areaWidth = 3;
		private int maxNegativeCount = 3;
		private int maxCrystalCount = 5;

		private readonly List<CellType> cellTypes = new List<CellType>(){
				CellType.Bomb, CellType.Correct, CellType.Crystal, CellType.Empty, CellType.Incorrect
		};

		private CellType randomCell => cellTypes[Random.Range(0, cellTypes.Count)];

		private Row prevRow = new Row();
		private Row currentRow = new Row();
		private int negativeCount = 0;
		private int crystalCount = 0;

		public EdiblesGenerator(int areaWidth, int maxNegativeCount, int maxCrystalCount)
		{
			this.areaWidth = areaWidth;
			this.maxNegativeCount = maxNegativeCount;
			this.maxCrystalCount = maxCrystalCount;
		}

		public void Clear()
		{
			prevRow.Cells.Clear();
			currentRow.Cells.Clear();
			negativeCount = 0;
			crystalCount = 0;
		}

		public override Row GenerateRow()
		{
			prevRow = currentRow;
			List<CellType> cells = new List<CellType>();
			currentRow = new Row() { Cells = cells };
			int rowNegativeCount = 0;
			int rowCrystalCount = 0;
			for (int i = 0; i < areaWidth; i++)
			{
				CellType cell;
				do
				{
					cell = randomCell;
				} while (!IsCanPlaceCell(cell, i));
				if (cell.IsNegative())
					rowNegativeCount++;
				if (cell == CellType.Crystal)
					rowCrystalCount++;
				cells.Add(cell);
			}

			if (rowNegativeCount == 0)
				negativeCount = 0;
			else
				negativeCount += rowNegativeCount;
			if (rowCrystalCount == 0)
				crystalCount--;
			else
				crystalCount += rowCrystalCount;			
			return currentRow;
		}

		private bool IsCanPlaceCell(CellType type, int position)
		{
			if (prevRow.Cells.Count == 0) return true;
			if (type.IsPositive())
			{
				bool possible = !currentRow.Cells.Any(x => x.IsPositive() && x != type)
							&& !prevRow.Cells[position].IsNegative();
				if (type == CellType.Crystal)
					possible &= crystalCount <= 0;
				return possible;
			}
			else if (type.IsNegative())
			{
				if (negativeCount >= maxNegativeCount)
					return false;
				if (currentRow.Cells.Count(x => x.IsNegative()) >= areaWidth - 2)
					return false;
				else if (position == 0)
					return !(prevRow.Cells[0].IsNegative() || prevRow.Cells[1].IsNegative());
				else
					return !(prevRow.Cells[position].IsNegative() || prevRow.Cells[position - 1].IsNegative());
			}
			else
				return true;
		}
	}
}