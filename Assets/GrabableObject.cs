using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class GrabableObject :MonoBehaviour
{
	Rigidbody _rigidbody;
	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}
	public void UnGrab()
	{
		_rigidbody.useGravity = true;
	}
}