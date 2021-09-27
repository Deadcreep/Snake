using System;
using UnityEngine;

public class SpeedManager : MonoBehaviour
{
	public static float CurrentSpeed { get; private set; }
	[SerializeField] private float defaultSpeed = 2;
	private static SpeedManager instance;
	public static event Action<float> OnSpeedChanged;

	private void Awake()
	{
		if (!instance)
		{
			instance = this;
			CurrentSpeed = defaultSpeed;
		}
		else
			Destroy(this);
	}

	public static void MultiplySpeed(float multiplier)
	{
		CurrentSpeed = instance.defaultSpeed * multiplier;
		OnSpeedChanged?.Invoke(CurrentSpeed);
	}

	public static void ResetSpeed()
	{
		CurrentSpeed = instance.defaultSpeed;
		OnSpeedChanged?.Invoke(CurrentSpeed);
	}
}