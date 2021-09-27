using UnityEngine;
using UnityEngine.UI;

public class EndGamePanel : MonoBehaviour
{
	[SerializeField] private string goodFinalMessage = "Level completed";
	[SerializeField] private string badFinalMessage = "Level failed";
	[SerializeField] private GameObject panel;
	[SerializeField] private Text textField;
	[SerializeField] private Text humansField;
	[SerializeField] private Text crystalsField;
	[SerializeField] private GameManager manager;
	[SerializeField] private ScoreManager scoreManager;

	private void Start()
	{
		manager.OnGameEnded += HandleEndGame;
	}

	private void OnDestroy()
	{
		manager.OnGameEnded -= HandleEndGame;
	}

	private void HandleEndGame(bool result)
	{
		textField.text = result ? goodFinalMessage : badFinalMessage;
		humansField.text = scoreManager.EatedHumansCount.Value.ToString();
		crystalsField.text = scoreManager.EatedCrystalsCount.Value.ToString();
		panel.SetActive(true);
	}
}