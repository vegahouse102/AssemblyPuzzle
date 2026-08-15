
using UnityEngine;

public class AttachSocket : MonoBehaviour
{

	[SerializeField]
	PieceSO _connectedObjectSO;
	
	Transform _socketTransform;


	private AttachSocket _attachedSocket;


	

	private AttachableObject _thisObject;

	public PieceSO AttachableObjectSO => _thisObject.AttachableObjectSO;

	//public AttachableObject ThisAttachableObject => _thisObject;
	private void Awake()
	{
		_thisObject = GetComponentInParent<AttachableObject>();

		_socketTransform = transform;
#if UNITY_EDITOR
		Debug.Assert(AttachableObjectSO != null);
		Debug.Assert(_thisObject!=null);
		Debug.Assert(_thisObject != null);
#endif
	}
	private void OnTriggerEnter(Collider other)
	{
		if (!_thisObject.IsAttachable)
			return;
		if (other.TryGetComponent<AttachSocket>(out AttachSocket otherSocket))
		{
			if (otherSocket.AttachableObjectSO == _connectedObjectSO)
			{
				GrabableObject grabableObject = otherSocket.GetComponentInParent<GrabableObject>();
				if (grabableObject == null||!grabableObject.IsGrab)
					return;

				Debug.Log("socketEnter");
				_attachedSocket = otherSocket;
				grabableObject.OnGrab.AddListener(HandleAttach);
			}
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (!_thisObject.IsAttachable)
			return;
		if (other.TryGetComponent<AttachSocket>(out AttachSocket otherSocket))
		{
			if (otherSocket.AttachableObjectSO == _connectedObjectSO)
			{
				GrabableObject grabableObject = otherSocket.GetComponentInParent<GrabableObject>();
				if (grabableObject == null || !grabableObject.IsGrab)
					return;

				Debug.Log("socketExit");
				_attachedSocket = null;
				grabableObject.OnGrab.RemoveListener(HandleAttach);
			}
		}
	}
	private void HandleAttach(bool value)
	{
		if (value)
			return;
		Debug.Log("attach");
		if (_attachedSocket != null)
		{
			_thisObject.AttachObject(_attachedSocket._thisObject,
				_attachedSocket._socketTransform,_socketTransform);
		}
	}
	public void TestAttachObject(GameObject AttachableObject)//obj´Â 
	{
		GameObject AssemblyObject = new GameObject("AssemblyObject");
	
	}
}
