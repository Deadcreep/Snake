using Edibles;
using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private float targetLevelDuration = 40f;
	[SerializeField] private int colorChangeCount = 5;
	[SerializeField] private Colorizer colorizer;
	[SerializeField] private AreaManager areaManager;
	[SerializeField] private ColorManager colorManager;
	[SerializeField] private ScoreManager scoreManager;
	[SerializeField] private GameObject snakeGo;
	[SerializeField] private FeverManager feverManager;
	private int passedTime;

	public event Action<bool> OnGameEnded;

	private void Start()
	{
		StartCoroutine(CompleteGame());
		areaManager.OnEated += HandleOnEated;
		int targetRowsCount = (int)(SpeedManager.CurrentSpeed * targetLevelDuration / areaManager.Step);
		int distanceBetweenColorChange = targetRowsCount / (colorChangeCount + 1);
		for (int i = 1; i <= colorChangeCount; i++)
		{
			areaManager.ReservePositionForColorizer(i * distanceBetweenColorChange);
		}
		colorManager.SetupColorizer(distanceBetweenColorChange * areaManager.Step, areaManager.Offset);
	}

	private void OnDestroy()
	{
		areaManager.OnEated -= HandleOnEated;
	}

	private void HandleOnEated(CellType cell)
	{
		if (feverManager.IsFevered)
			return;
		if (cell == CellType.Incorrect || cell == CellType.Bomb)
		{
			EndGame(false);
		}
	}

	private void EndGame(bool result)
	{
		snakeGo.SetActive(false);
		areaManager.StopGenerate();
		SpeedManager.MultiplySpeed(0);
		OnGameEnded?.Invoke(result);
	}

	private IEnumerator CompleteGame()
	{
		yield return new WaitForSeconds(targetLevelDuration);
		EndGame(true);
	}
}