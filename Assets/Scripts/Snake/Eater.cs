using Edibles;
using System;
using UnityEngine;

namespace Snake
{
	[RequireComponent(typeof(BoxCollider))]
	public class Eater : MonoBehaviour
	{
		private BoxCollider collider;
		public event Action<Edible> OnEated;

		private void Awake()
		{
			collider = GetComponent<BoxCollider>();
		}

		public void IncreaseSize()
		{
			collider.size = new Vector3(20, 1, 1);
		}

		public void DecreaseSize()
		{
			collider.size = Vector3.one;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent<Edible>(out var edible))
			{
				OnEated?.Invoke(edible);
			}
		}
	}
}