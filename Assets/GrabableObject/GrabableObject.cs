using UnityEngine;
using UnityEngine.Events;
public class GrabableObject :MonoBehaviour
{
	Rigidbody _rigidbody;

	[SerializeField]
	private float _grabSpeed = 6.86f;
	[SerializeField]
	private float _rotationSpeed = 4.71f;
	public UnityEvent OnGrab;
	public UnityEvent OnUnGrab;

	private Transform _grabPos;
	private void Awake()
	{
		_rigidbody = GetComponentInParent<Rigidbody>();
		Debug.Assert( _rigidbody != null );
	}
	public void StartGrab(Transform grabPos)
	{
		_grabPos = grabPos;
		OnGrab?.Invoke();
	}
	public void Rotate(Vector2 mouseDelta,Transform camera)
	{
		Quaternion rot =
					    Quaternion.AngleAxis(mouseDelta.y * _rotationSpeed * Time.deltaTime, camera.right) *
					    Quaternion.AngleAxis(-mouseDelta.x * _rotationSpeed * Time.deltaTime, camera.up);
		Rigidbody rigidbody = GetComponent<Rigidbody>();
		rigidbody.MoveRotation(rot * rigidbody.rotation);
	}
	private void FixedUpdate()
	{
		if( _grabPos != null )
		{
			Vector3 velocity = (_grabPos.position - _rigidbody.position) * _grabSpeed;

			_rigidbody.linearVelocity = velocity;
			_rigidbody.angularVelocity = Vector3.zero;
		}
	}
	public void UnGrab()
	{
		_grabPos = null;
		_rigidbody.useGravity = true;
		OnUnGrab?.Invoke();
	}
}