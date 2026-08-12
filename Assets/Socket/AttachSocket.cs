
using UnityEngine;

public class AttachSocket : MonoBehaviour
{
	[SerializeField]
	AttachableObject _thisObject;
	[SerializeField]
	PieceSO _connectedObjectSO;
	[SerializeField]
	Vector3 _attachedLocalPosition;
	[SerializeField]
	Quaternion _attachedLocalRotation;


	private AttachSocket _attachedSocket;


	public PieceSO AttachableObjectSO => _thisObject.AttachableObjectSO;

	public AttachableObject ThisAttachableObject => _thisObject;

	private void Awake()
	{
#if UNITY_EDITOR
		Debug.Assert(AttachableObjectSO != null);
		Debug.Assert(_thisObject!=null);
#endif
	}
	private void OnTriggerEnter(Collider other)
	{
		if (!ThisAttachableObject.IsAttachable)
			return;
		if (other.TryGetComponent<AttachSocket>(out AttachSocket otherSocket))
		{
			if (otherSocket.AttachableObjectSO == _connectedObjectSO
				&&other.attachedRigidbody != null 
				&& other.attachedRigidbody.TryGetComponent<GrabableObject>(out GrabableObject grabableObject))
			{
				Debug.Log("socketEnter");
				_attachedSocket = otherSocket;
				grabableObject.OnGrab.AddListener(HandleAttach);
			}
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (!ThisAttachableObject.IsAttachable)
			return;
		if (other.TryGetComponent<AttachSocket>(out AttachSocket otherSocket))
		{
			if (otherSocket.AttachableObjectSO == _connectedObjectSO
				&& other.attachedRigidbody != null
				&& other.attachedRigidbody.TryGetComponent<GrabableObject>(out GrabableObject grabableObject))
			{
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
			_thisObject.AttachObject(_attachedSocket.ThisAttachableObject,
				_attachedLocalPosition,_attachedLocalRotation,
				_attachedSocket._attachedLocalPosition,_attachedSocket._attachedLocalRotation);
		}
	}
	public void TestAttachObject(GameObject AttachableObject)//obj´Â 
	{
		GameObject AssemblyObject = new GameObject("AssemblyObject");
		
		AttachableObject.transform.localPosition = _attachedLocalPosition;
		AttachableObject.transform.localRotation = _attachedLocalRotation;
		if (AttachableObject.TryGetComponent<AttachableObject>(out AttachableObject obj)){
			//obj.AttachObject(_thisObject,_attachedLocalPosition,_attachedLocalRotation);
		}
	}
}
