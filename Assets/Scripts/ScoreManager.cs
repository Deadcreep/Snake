using Edibles;
using Snake;
using UniRx;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	public IntReactiveProperty EatedHumansCount = new IntReactiveProperty();
	public IntReactiveProperty EatedCrystalsCount = new IntReactiveProperty();
	[SerializeField] private Eater eater;

	private void Start()
	{
		eater.OnEated += HandleEated;
	}

	private void OnDestroy()
	{
		eater.OnEated -= HandleEated;
	}

	private void HandleEated(Edible edible)
	{
		if (edible.type == CellType.Correct)
			EatedHumansCount.Value++;
		if (edible.type == CellType.Crystal)
			EatedCrystalsCount.Value++;
	}
}