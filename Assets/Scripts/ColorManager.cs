using DG.Tweening;
using Snake;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Edibles
{
	public class ColorManager : MonoBehaviour
	{
		public Color CorrectColor => currentNode.Value;
		public Color RandomIncorrectColor => incorrectColors[Random.Range(0, incorrectColors.Count)];
		[SerializeField] private SegmentsManager segmentsManager;
		[SerializeField] private Colorizer colorizer;
		[SerializeField] private List<Color> colors;
		private List<Color> incorrectColors;
		private LinkedList<Color> colorSequence;
		private LinkedListNode<Color> currentNode;
		private LinkedListNode<Color> nextNode;
		private ReactiveCommand colorizerMovingCommad;
		private Vector3 stepVector;

		private void Awake()
		{
			CreateSequence();
			colorizer.OnColorized += Colorize;
			incorrectColors = new List<Color>(colors);
			incorrectColors.Remove(CorrectColor);
		}

		private void Start()
		{
			colorizer.SetColor(currentNode.Next.Value);
			segmentsManager.SetColor(CorrectColor);
		}

		private void OnDestroy()
		{
			colorizer.OnColorized -= Colorize;
		}

		public void SetupColorizer(float colorizerStep, float initialOffset)
		{
			stepVector = Vector3.forward * colorizerStep;
			colorizerMovingCommad = new ReactiveCommand();
			colorizerMovingCommad.Delay(System.TimeSpan.FromSeconds(1))
								 .Subscribe(UpdateColorizer)
								 .AddTo(this);

			colorizer.transform.position = new Vector3(0f, colorizer.transform.position.y, colorizerStep + initialOffset);
		}

		public Color GetColorAt(int indexInSequence)
		{
			
			if (indexInSequence >= colorSequence.Count)
			{
				Debug.Log($"[ColorManager] index {indexInSequence % colorSequence.Count}", this);
				return colorSequence.ElementAt(indexInSequence % colorSequence.Count);
			}
			else
			{
				return colorSequence.ElementAt(indexInSequence);
			}
		}

		private void Colorize()
		{
			UpdateCorrectColor();
			segmentsManager.SetColor(currentNode.Value);
			colorizerMovingCommad.Execute();
		}

		private void UpdateCorrectColor()
		{
			//for (int i = 0; i < incorrectColors.Count; i++)
			//{
			//	if (incorrectColors[i] == nextNode.Value)
			//	{
			//		incorrectColors[i] = CorrectColor;
			//		break;
			//	}
			//}
			incorrectColors = colorSequence.Where(x => x != nextNode.Value).ToList();
			currentNode = nextNode;
			nextNode = nextNode.Next ?? colorSequence.First;
		}

		private void CreateSequence()
		{
			for (var i = 0; i < colors.Count; ++i)
			{
				var r = Random.Range(i, colors.Count);
				var tmp = colors[i];
				colors[i] = colors[r];
				colors[r] = tmp;
			}
			colorSequence = new LinkedList<Color>(colors);
			currentNode = colorSequence.First;
			nextNode = currentNode.Next;
		}

		private void UpdateColorizer(Unit _)
		{
			colorizer.transform.position += stepVector;
			colorizer.SetColor(nextNode.Value);
		}
	}
}