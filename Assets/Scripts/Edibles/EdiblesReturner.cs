using System;
using UnityEngine;

namespace Edibles
{
	[RequireComponent(typeof(Collider))]
	public class EdiblesReturner : MonoBehaviour
	{
		public event Action<Edible> OnEdibleOutOfArea;

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent<Edible>(out var edible))
			{
				OnEdibleOutOfArea?.Invoke(edible);
			}
		}
	}
}