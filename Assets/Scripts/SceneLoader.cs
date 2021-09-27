using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	public void LoadScene(int index)
	{
		SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
		SceneManager.LoadSceneAsync(index);
	}
}