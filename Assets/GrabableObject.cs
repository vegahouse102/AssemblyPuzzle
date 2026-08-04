using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class GrabableObject :MonoBehaviour
{
	Rigidbody _rigidbody;
	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}
	public void Grab()
	{
		_rigidbody.useGravity = false;
		_rigidbody.linearVelocity = Vector3.zero;
		_rigidbody.angularVelocity = Vector3.zero;
	}
	public void UnGrab()
	{
		_rigidbody.useGravity = true;
	}
}