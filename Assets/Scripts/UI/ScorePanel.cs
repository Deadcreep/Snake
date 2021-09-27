using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour
{
	[SerializeField] private Text humansField;
	[SerializeField] private Text crystalsField;
	[SerializeField] private ScoreManager manager;

	private void Start()
	{
		manager.EatedHumansCount.Subscribe(x => humansField.text = x.ToString()).AddTo(this);
		manager.EatedCrystalsCount.Subscribe(x => crystalsField.text = x.ToString()).AddTo(this);
	}
}