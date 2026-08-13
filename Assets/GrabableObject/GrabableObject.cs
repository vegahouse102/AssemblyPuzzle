
using UnityEngine;
using UnityEngine.Events;
public class GrabableObject :MonoBehaviour
{

	[SerializeField]
	private float _grabSpeed = 6.86f;
	[SerializeField]
	private float _rotationSpeed = 4.71f;
	private Transform _grabPos;
	public UnityEvent<bool> OnGrab;
	public bool IsGrab { get; private set; }
	private void Awake()
	{
	}
	private void FixedUpdate()
	{
		if (IsGrab)
		{
			Vector3 velocity = (_grabPos.position - transform.position) * _grabSpeed;

			Rigidbody rigid = GetComponentInParent<Rigidbody>();
			if (rigid == null)
				Debug.Log("isnullrigidbody");
			rigid.linearVelocity = velocity;
			rigid.angularVelocity = Vector3.zero;
		}
	}
	public void StartGrab(Transform grabPos)
	{
		_grabPos = grabPos;
		IsGrab = true;
		OnGrab?.Invoke(true);
	}
	public void Rotate(Vector2 mouseDelta,Transform camera)
	{
		Quaternion rot =
					    Quaternion.AngleAxis(mouseDelta.y * _rotationSpeed * Time.deltaTime, camera.right) *
					    Quaternion.AngleAxis(-mouseDelta.x * _rotationSpeed * Time.deltaTime, camera.up);



		Rigidbody rigid = GetComponentInParent<Rigidbody>();
			
			rigid.MoveRotation(rot * rigid.rotation);
	}

	public void UnGrab()
	{
		_grabPos = null;
		IsGrab = false;
		OnGrab?.Invoke(false);
	}
}