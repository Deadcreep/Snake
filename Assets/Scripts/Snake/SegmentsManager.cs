using DG.Tweening;
using Edibles;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Snake
{
	public class SegmentsManager : MonoBehaviour
	{
		public List<Segment> segments;
		//[SerializeField] private ColorManager colorManager;
		[SerializeField] private Eater eater;
		[SerializeField] private Segment prefab;
		[SerializeField] private int maxSegments = 10;
		private float speedZ;
		private WaitForSeconds colorizeWait = new WaitForSeconds(0.1f);
		private Color currentColor;
		private void Start()
		{
			segments.AddRange(GetComponentsInChildren<Segment>());
			SetupTweens();
			//SetColor(colorManager.CorrectColor);
			speedZ = SpeedManager.CurrentSpeed;
			eater.OnEated += HandleEated;
		}

		private void HandleEated(Edible edible)
		{
			if (edible.type == CellType.Correct || edible.type == CellType.Crystal)
			{
				if (segments[0].IsEating)
				{
					segments[segments.Count - 1].EatingTween.onComplete += AddNewSegment;
				}
				else
					segments[0].EatingTween.Restart();
			}
		}

		private void SetupTweens()
		{
			for (int i = 0; i < segments.Count - 1 && i < maxSegments; i++)
			{
				int next = i + 1;
				segments[i].EatingTween.onComplete = () => segments[next].EatingTween.Restart();
			}
			segments[segments.Count - 1].EatingTween.onComplete = AddNewSegment;
		}

		public void SetColor(Color color)
		{
			currentColor = color;
			StartCoroutine(Colorize(color));
		}

		private IEnumerator Colorize(Color color)
		{
			for (int i = 0; i < segments.Count; i++)
			{
				segments[i].SetColor(color);
				yield return colorizeWait;
			}
		}

		private void AddNewSegment()
		{
			if (segments.Count < maxSegments)
			{
				var newSegment = Instantiate(prefab, transform);
				newSegment.transform.SetAsLastSibling();
				newSegment.transform.localScale = Vector3.zero;
				newSegment.transform.position = segments.Last().transform.position;
				newSegment.transform.DOScale(prefab.transform.localScale, 0.3f);
				newSegment.SetColor(currentColor);
				newSegment.EatingTween.OnComplete(AddNewSegment);
				segments[segments.Count - 1].EatingTween.onComplete = () => newSegment.EatingTween.Restart();
				segments.Add(newSegment);
			}
			else
				segments[segments.Count - 1].EatingTween.onComplete = null;
		}

		private void Update()
		{
			for (int i = 1; i < segments.Count; i++)
			{
				var current = segments[i].transform;
				var prev = segments[i - 1].transform;
				var distance = Vector3.Distance(prev.position, current.position);
				var slerpSpeed = Time.deltaTime * distance / 0.1f * speedZ;
				current.SetPositionAndRotation(Vector3.Slerp(current.position, prev.position, slerpSpeed),
					Quaternion.Slerp(current.rotation, prev.rotation, slerpSpeed));
			}
		}
	}
}