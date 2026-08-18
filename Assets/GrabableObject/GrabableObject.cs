
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
		
		if (IsGrab&&_grabPos!=null)
		{
			Vector3 velocity = (_grabPos.position - transform.position) * _grabSpeed;

			Rigidbody rigid = GetComponentInParent<Rigidbody>();
			if (rigid == null)
				Debug.Log("isnullrigidbody");
			rigid.linearVelocity = velocity;
			rigid.angularVelocity = Vector3.zero;
		}
	}
	private void SetGrab(bool value)
	{
		IsGrab = value;
		OnGrab?.Invoke(value);
	}
	public void StartGrab(Transform grabPos)
	{
		_grabPos = grabPos;
		
		Collider collider = GetComponent<Collider>();
		Transform root = collider.attachedRigidbody.transform;
		foreach(Transform child in root)
		{
			if(child.TryGetComponent<GrabableObject>(out GrabableObject grabableObject))
			{
				grabableObject.SetGrab(true);
			}
		}
		if (transform.parent == null)
		{
			SetGrab(true);
		}
	//	collider.isTrigger = true;
	}
	public void Rotate(Vector2 mouseDelta,Transform camera)
	{
		Quaternion rot =
					    Quaternion.AngleAxis(mouseDelta.y * _rotationSpeed * Time.deltaTime, camera.right) *
					    Quaternion.AngleAxis(-mouseDelta.x * _rotationSpeed * Time.deltaTime, camera.up);


		Rigidbody rigid = GetComponentInParent<Rigidbody>();
		if (transform.parent == null)
		{
			rigid.MoveRotation(rot*rigid.rotation);
		}
		else
		{
			Vector3 rootPos = transform.position + rot * (transform.parent.position-transform.position) ;
			rigid.MoveRotation(rot*rigid.rotation );
			rigid.MovePosition(rootPos);
			//Debug.Log($"{rootPos} {gameObject.name}");
		}
	}

	public void UnGrab()
	{
		_grabPos = null;
		Collider collider = GetComponent<Collider>();
		Transform root = collider.attachedRigidbody.transform;
		foreach (Transform child in root)
		{
			if (child.TryGetComponent<GrabableObject>(out GrabableObject grabableObject))
			{
				grabableObject.SetGrab(false);
			}
		}
		if (transform.parent == null)
		{
			SetGrab(false);
		}
		//collider.isTrigger = false;
	}
}