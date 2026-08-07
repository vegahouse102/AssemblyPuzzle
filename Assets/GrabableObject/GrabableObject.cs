using UnityEngine;
using UnityEngine.Events;
public class GrabableObject :MonoBehaviour
{
	Rigidbody _rigidbody;
	public UnityEvent OnGrab;
	public UnityEvent OnUnGrab;
	private void Awake()
	{
		_rigidbody = GetComponentInParent<Rigidbody>();
		Debug.Assert( _rigidbody != null );
	}
	public void Grab()
	{
		OnGrab?.Invoke();
	}
	public void UnGrab()
	{
		
		_rigidbody.useGravity = true;
		OnUnGrab?.Invoke();
	}
}