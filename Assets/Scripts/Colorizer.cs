using Snake;
using System;
using UniRx.InternalUtil;
using UnityEngine;

public class Colorizer : MonoBehaviour
{
	[SerializeField] private Eater eater;
	[SerializeField] private ParticleSystem[] particles;
	[SerializeField] private MeshRenderer renderer;

	public event Action OnColorized;

	private void Awake()
	{
		renderer = GetComponent<MeshRenderer>();
		particles = GetComponentsInChildren<ParticleSystem>();
	}

	public void SetColor(Color color)
	{
		renderer.material.SetColor("_Color", color);
		foreach (var item in particles)
		{
			var temp = item.main;
			temp.startColor = color;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.GetInstanceID() == eater.gameObject.GetInstanceID())
		{
			OnColorized?.Invoke();			
		}
	}
}