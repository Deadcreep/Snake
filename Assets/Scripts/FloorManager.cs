using UnityEngine;

public class FloorManager : MonoBehaviour
{
	[SerializeField] private Transform first;
	[SerializeField] private Transform second;
	[SerializeField] private float distanceToSwitch;
	[SerializeField] private float passedDistance = 0;
	private float segmentSize;
	private Transform next;
	private Transform current;
	private Transform temp;

	private void Start()
	{
		segmentSize = first.GetComponent<MeshRenderer>().bounds.size.z;
		distanceToSwitch = segmentSize;
		current = second;
		next = first;
	}

	private void FixedUpdate()
	{
		passedDistance += SpeedManager.CurrentSpeed * Time.fixedDeltaTime;
		if (passedDistance > distanceToSwitch)
		{
			next.position = current.position + Vector3.forward * segmentSize;
			temp = current;
			current = next;
			next = temp;
			passedDistance = 0;
		}
	}
}