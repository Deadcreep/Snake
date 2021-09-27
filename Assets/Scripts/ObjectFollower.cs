using UnityEngine;

[ExecuteInEditMode]
internal class ObjectFollower : MonoBehaviour
{
	[SerializeField] private Transform target;
	[SerializeField] private Vector3 offset;
	[SerializeField] private Vector3 targetPos;
	[SerializeField] private float speed = 1f;
	private void Update()
	{
#if UNITY_EDITOR
		if (target)
#endif
			targetPos = target.position + offset;
		targetPos.x = 0f;
		transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
		transform.position = targetPos;
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawSphere(transform.position, 0.2f);
		Gizmos.color = Color.green;
		if (target)
			Gizmos.DrawSphere(target.position + offset, 0.2f);
	}
}