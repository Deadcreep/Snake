using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Edibles
{
	[CreateAssetMenu(fileName = "EdiblesWeights", menuName = "Edibles/Weights")]
	public class EdiblesWeights : ScriptableObject
	{
		[SerializeField] public List<WeightedCell> weights;

		public float TotalWeight
		{
			get
			{
				if (totalWeight == 0)
					totalWeight = weights.Sum(x => x.Weight);
				return totalWeight;
			}
		}

		[SerializeField] private float totalWeight = 0;

		private void OnEnable()
		{
			if (weights == null || weights.Count == 0)
			{
				weights = new List<WeightedCell>();
				foreach (var item in Enum.GetValues(typeof(CellType)).Cast<CellType>())
				{
					weights.Add(new WeightedCell(item, 0));
				}
			}
		}

		private void OnValidate()
		{
			totalWeight = weights.Sum(x => x.Weight);
		}

		[ContextMenu("Test")]
		private void Test()
		{
			List<CellType> result = new List<CellType>();
			for (int i = 0; i < 100; i++)
			{
				result.Add(this.GetRandomCell());
			}
			StringBuilder sb = new StringBuilder(0);
			foreach (var item in result.GroupBy(x => x).Select(x => (x, x.Count())))
			{
				sb.Append(item.x.Key + " " + item.Item2 + "; ");
			}
			Debug.Log($"[EdiblesWeights] {TotalWeight} {sb}", this);
		}
	}

	public static class EdiblesWeightsExtensions
	{
		public static CellType GetRandomCell(this EdiblesWeights weights)
		{
			float randomWeight = Random.Range(0, weights.TotalWeight);
			foreach (WeightedCell cell in weights.weights)
			{
				if (randomWeight <= cell.Weight)
				{
					return cell.Type;
				}

				randomWeight -= cell.Weight;
			}
			return CellType.Empty;
		}
	}

	[Serializable]
	public struct WeightedCell
	{
		public CellType Type;
		[Range(0, 1)] public float Weight;

		public WeightedCell(CellType type, float weight)
		{
			this.Type = type;
			this.Weight = weight;
		}
	}
}