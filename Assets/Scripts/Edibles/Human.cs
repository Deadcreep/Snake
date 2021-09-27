using UnityEngine;

namespace Edibles
{
	public class Human : Edible
	{
		public Color Color
		{
			get => color;
			set
			{
				if (value != color)
				{
					color = value;
					foreach (var renderer in renderers)
					{
						renderer.material.color = value;
					}
				}
			}
		}

		[SerializeField] private Color color;
		private MeshRenderer[] renderers;

		private void Awake()
		{
			renderers = GetComponentsInChildren<MeshRenderer>(true);
		}
	}
}