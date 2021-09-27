using DG.Tweening;
using UnityEngine;

namespace Snake
{
	[RequireComponent(typeof(MeshRenderer))]
	public class Segment : MonoBehaviour
	{
		public Tween EatingTween
		{
			get
			{
				return eatingTween;
			}
		}

		public bool IsEating => eatingTween.IsPlaying();
		private Tween eatingTween;
		private MeshRenderer _renderer;
		private float defaultX;

		private void Awake()
		{
			eatingTween = transform.DOPunchScale(new Vector3(0.15f, 0f, 0f), 0.6f, 1, 0)
								.SetAutoKill(false)
								.Pause();
			_renderer = GetComponent<MeshRenderer>();
			defaultX = transform.localScale.x;
		}

		public void SetColor(Color color)
		{
			_renderer.material.SetColor("_Color", color);
		}

		private void OnDestroy()
		{
			eatingTween.Kill(false);
		}
	}
}