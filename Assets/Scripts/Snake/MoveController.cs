using UnityEngine;

internal class MoveController : MonoBehaviour
{
	public bool IsEnabled = true;
	[SerializeField] private Vector2 borders;
	[SerializeField] private float speedX;
	[SerializeField] private float maxXSpeed;
	[SerializeField] private Vector3 moveVector = default;
	[SerializeField] private Vector3 hitpoint;
	private Plane plane;
	private Rigidbody rb;

	private void Start()
	{
		IsEnabled = true;
		rb = GetComponent<Rigidbody>();
		plane = new Plane(Vector3.up, Vector3.zero);
		moveVector.z = SpeedManager.CurrentSpeed;
		SpeedManager.OnSpeedChanged += HandleSpeedChanged;
	}

	private void OnDestroy()
	{
		SpeedManager.OnSpeedChanged -= HandleSpeedChanged;
	}

	private void HandleSpeedChanged(float speed)
	{
		moveVector.z = speed;
	}

	private void Update()
	{
		moveVector.x = 0f;
#if UNITY_EDITOR
		if (IsEnabled && Input.GetMouseButton(0))
		{
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#elif UNITY_ANDROID
		if (IsEnabled && Input.touchCount > 0)
		{
			var ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
#endif
			if (plane.Raycast(ray, out float distance))
			{
				hitpoint = ray.GetPoint(distance);
				var currentX = Mathf.Clamp((hitpoint.x - transform.position.x) * speedX, -maxXSpeed, maxXSpeed);
				moveVector.x = currentX;
				if (transform.position.x + moveVector.x * Time.deltaTime > borders.y
					|| transform.position.x + moveVector.x * Time.deltaTime < borders.x)
					moveVector.x = 0;
			}
		}
		transform.LookAt(transform.position + moveVector);
		rb.velocity = moveVector;
	}
}