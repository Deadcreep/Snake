using DG.Tweening;
using Snake;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Edibles
{
	public class ColorManager : MonoBehaviour
	{
		public Color correctColor => currentNode.Value;
		public Color randomIncorrectColor => incorrectColors[Random.Range(0, incorrectColors.Count)];
		[SerializeField] private SegmentsManager segmentsManager;
		[SerializeField] private Colorizer colorizer;
		[SerializeField] private List<Color> colors;
		private List<Color> incorrectColors;
		private LinkedList<Color> colorSequence;
		private LinkedListNode<Color> currentNode;
		private ReactiveCommand colorizerMovingCommad;
		private Vector3 stepVector;

		private void Awake()
		{
			CreateSequence();
			colorizer.OnColorized += Colorize;
			incorrectColors = new List<Color>(colors);
			incorrectColors.Remove(correctColor);
		}

		private void Start()
		{
			colorizer.SetColor(currentNode.Next.Value);
		}

		private void OnDestroy()
		{
			colorizer.OnColorized -= Colorize;
		}

		public void SetupColorizer(float colorizerStep)
		{
			stepVector = Vector3.forward * colorizerStep;
			colorizerMovingCommad = new ReactiveCommand();
			colorizerMovingCommad.Delay(System.TimeSpan.FromSeconds(1))
								 .Subscribe(UpdateColorizer)
								 .AddTo(this);

			colorizer.transform.position = new Vector3(0f, colorizer.transform.position.y, colorizerStep);
		}

		private void Colorize()
		{
			UpdateCorrectColor();
			segmentsManager.SetColor(currentNode.Value);
			colorizerMovingCommad.Execute();
		}

		private void UpdateCorrectColor()
		{
			for (int i = 0; i < incorrectColors.Count; i++)
			{
				if (incorrectColors[i] == (currentNode.Next ?? colorSequence.First).Value)
				{
					incorrectColors[i] = correctColor;
					break;
				}
			}
			currentNode = currentNode.Next ?? colorSequence.First;
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
			Debug.Log($"[ColorManager] sequence created", this);
		}

		private void UpdateColorizer(Unit _)
		{
			Debug.Log($"[ColorManager] update colorizer {correctColor} ", this);
			colorizer.transform.position += stepVector;
			colorizer.SetColor((currentNode.Next ?? colorSequence.First).Value);
		}
	}
}