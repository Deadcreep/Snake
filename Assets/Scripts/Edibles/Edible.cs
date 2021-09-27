using UnityEngine;

namespace Edibles
{
	public class Edible : MonoBehaviour
	{
		public CellType type;
		[SerializeField] private Vector3 defaultScale;

		private void OnEnable()
		{
			transform.localScale = defaultScale;
		}
	}
}