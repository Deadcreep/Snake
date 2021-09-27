using Edibles.Pools;
using Snake;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Edibles
{
	public class AreaManager : MonoBehaviour
	{
		public float Step => zStep;
		[SerializeField] private int areaWidth = 3;
		[SerializeField] private int maxNegativeCount = 5;
		[SerializeField] private int maxCrystalCount = 5;
		[SerializeField] private float generationSpeedMultiplier;
		[SerializeField] private float zOffset = 5;
		[SerializeField] private float zStep = 2;
		[SerializeField] private List<float> xPositions;
		[SerializeField] private float yOffset = 0.5f;
		[SerializeField] private int initialRowCount = 5;
		[SerializeField] private ColorManager colorManager;
		[SerializeField] private Eater eater;
		[SerializeField] private EdiblesReturner returner;

		[Header("Prefabs")]
		[SerializeField] private Edible bombPrefab;
		[SerializeField] private Edible crystalPrefab;
		[SerializeField] private Human humanPrefab;

		[Header("Pools settings")]
		[SerializeField] private int preloadCount = 20;
		[SerializeField] private int preloadThreshold = 3;

		private AbstractEdiblesGenerator generator;
		private EdiblePool bombPool;
		private EdiblePool crystalPool;
		private HumanPool humanPool;

		private int rowsCount = 0;
		private float generationSpeed;
		private WaitForSeconds generationWait;

		private LinkedList<int> reservedPositions = new LinkedList<int>();
		private LinkedListNode<int> nextToInsert;

		public event Action<CellType> OnEated;


		private IEnumerator Start()
		{
			SetSpeed(SpeedManager.CurrentSpeed);
			SpeedManager.OnSpeedChanged += SetSpeed;
			eater.OnEated += HandleEated;
			returner.OnEdibleOutOfArea += ReturnEdible;
			generator = new EdiblesGenerator(areaWidth, maxNegativeCount, maxCrystalCount);
			bombPool = new EdiblePool(bombPrefab, transform);
			crystalPool = new EdiblePool(crystalPrefab, transform);
			humanPool = new HumanPool(humanPrefab, transform);

			var x = bombPool.PreloadAsync(preloadCount, preloadThreshold);
			var x2 = crystalPool.PreloadAsync(preloadCount, preloadThreshold);
			var x3 = humanPool.PreloadAsync(preloadCount, preloadThreshold);

			yield return x.ToAwaitableEnumerator();
			yield return x2.ToAwaitableEnumerator();
			yield return x3.ToAwaitableEnumerator();

			for (int i = 0; i < initialRowCount; i++)
			{
				GenerateRow();
			}
			StartCoroutine(ContinuousGenerate());
		}

		private void OnDestroy()
		{
			SpeedManager.OnSpeedChanged -= SetSpeed;
			returner.OnEdibleOutOfArea -= ReturnEdible;
			eater.OnEated -= HandleEated;

			humanPool.Dispose();
			bombPool.Dispose();
			crystalPool.Dispose();
		}

		public bool TryReservePosition(int position)
		{
			if (position < rowsCount)
				return false;
			foreach (var reserved in reservedPositions)
			{
				if (reserved == position)
					return false;
			}

			reservedPositions.AddLast(position);
			if (nextToInsert == null)
				nextToInsert = reservedPositions.Last;
			return true;
		}

		public void StopGenerate()
		{
			StopAllCoroutines();
		}

		private void HandleEated(Edible edible)
		{
			switch (edible.type)
			{
				case CellType.Bomb:
					HandleBombEated(edible);
					break;

				case CellType.Incorrect:
				case CellType.Correct:
					HandleHumanEated(edible as Human);
					break;

				case CellType.Crystal:
					HandleCrystalEated(edible);
					break;

				default:
					break;
			}
		}

		private void SetSpeed(float speed)
		{
			generationSpeed = 1 / (speed * generationSpeedMultiplier);
			generationWait = new WaitForSeconds(generationSpeed);
		}

		private void ReturnEdible(Edible edible)
		{
			switch (edible.type)
			{
				case CellType.Bomb:
					bombPool.Return(edible);
					break;

				case CellType.Incorrect:
				case CellType.Correct:
					humanPool.Return(edible as Human);
					break;

				case CellType.Crystal:
					crystalPool.Return(edible);
					break;

				default:
					break;
			}
		}

		private IEnumerator ContinuousGenerate()
		{
			while (enabled)
			{
				rowsCount++;
				if (nextToInsert?.Value == rowsCount)
				{
					zOffset += zStep;
					nextToInsert = nextToInsert.Next;
				}
				else
					GenerateRow();

				yield return generationWait;
			}
		}

		private void GenerateRow()
		{
			var row = generator.GenerateRow();
			for (int i = 0; i < areaWidth; i++)
			{
				if (row[i] != CellType.Empty)
				{
					var transfom = GetCell(row[i]);
					transfom.position = new Vector3(xPositions[i], yOffset, zOffset);
				}
			}
			zOffset += zStep;
		}

		private Transform GetCell(CellType type)
		{
			switch (type)
			{
				case CellType.Bomb:
					return bombPool.Rent().transform;

				case CellType.Incorrect:
					var incorrect = humanPool.Rent();
					incorrect.Color = colorManager.RandomIncorrectColor;
					incorrect.type = CellType.Incorrect;
					return incorrect.transform;

				case CellType.Correct:
					var correct = humanPool.Rent();					
					correct.Color = colorManager.CorrectColor;
					correct.type = CellType.Correct;
					return correct.transform;

				case CellType.Crystal:
					return crystalPool.Rent().transform;

				case CellType.Empty:
					return null;

				default:
					throw new ArgumentException($"Unexpected cell type {type}");
			}
		}

		private void HandleCrystalEated(Edible crystal)
		{
			crystalPool.Return(crystal);
			OnEated?.Invoke(CellType.Crystal);
		}

		private void HandleHumanEated(Human human)
		{
			humanPool.Return(human);
			if (human.Color == colorManager.CorrectColor)
				OnEated?.Invoke(CellType.Correct);
			else
				OnEated?.Invoke(CellType.Incorrect);
		}

		private void HandleBombEated(Edible bomb)
		{
			bombPool.Return(bomb);
			OnEated?.Invoke(CellType.Bomb);
		}
	}
}