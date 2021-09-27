using Edibles;
using Snake;
using System.Collections;
using UnityEngine;

public class FeverManager : MonoBehaviour
{
	public bool IsFevered { get; private set; }
	[SerializeField] private int crystalCountToFever = 3;
	[SerializeField] private float feverDuration = 5;
	[SerializeField] private float speedMultiplier = 3;
	[SerializeField] private Eater eater;
	[SerializeField] private MoveController moveController;
	[SerializeField] private int currentCount = 0;

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
		if (IsFevered) return;
		if (edible.type == CellType.Crystal)
		{
			currentCount++;
			if (currentCount >= crystalCountToFever)
				StartCoroutine(Fever());
		}
		else
			currentCount = 0;
	}

	private IEnumerator Fever()
	{
		IsFevered = true;
		moveController.IsEnabled = false;
		eater.IncreaseSize();
		var snakeTransform = moveController.transform;
		snakeTransform.position = new Vector3(0f, snakeTransform.position.y, snakeTransform.position.z);
		SpeedManager.MultiplySpeed(speedMultiplier);

		yield return new WaitForSeconds(feverDuration);

		SpeedManager.ResetSpeed();
		moveController.IsEnabled = true;
		eater.DecreaseSize();
		currentCount = 0;
		yield return new WaitForSeconds(0.5f);
		IsFevered = false;
	}
}