
using UnityEngine;

public class AttachSocket : MonoBehaviour
{
	[SerializeField]
	AttachableObject _thisObject;
	[SerializeField]
	PieceSO _connectedObjectSO;
	[SerializeField]
	Vector3 _localAttachPosition;
	[SerializeField]
	Vector3 _localAttachRotation;


	private AttachableObject _attachedObject;
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
		if (other.TryGetComponent<AttachSocket>(out AttachSocket otherSocket))
		{
			if (otherSocket.AttachableObjectSO == _connectedObjectSO
				&&other.attachedRigidbody != null 
				&& other.attachedRigidbody.TryGetComponent<GrabableObject>(out GrabableObject grabableObject))
			{
				_attachedObject = otherSocket.ThisAttachableObject;
				grabableObject.OnUnGrab.AddListener(HandleAttach);
			}
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.TryGetComponent<AttachSocket>(out AttachSocket otherSocket))
		{
			if (otherSocket.AttachableObjectSO == _connectedObjectSO
				&& other.attachedRigidbody != null
				&& other.attachedRigidbody.TryGetComponent<GrabableObject>(out GrabableObject grabableObject))
			{
				_attachedObject = null;
				grabableObject.OnUnGrab.RemoveListener(HandleAttach);
			}
		}
	}
	private void HandleAttach()
	{
		if (_attachedObject != null)
		{
			_thisObject.AttachObject(_attachedObject);
		}
	}
	public void TestAttachObject(GameObject AttachableObject)//obj´Â 
	{
		GameObject AssemblyObject = new GameObject("AssemblyObject");
		
		AttachableObject.transform.localPosition = _localAttachPosition;
		AttachableObject.transform.localRotation = Quaternion.Euler(_localAttachRotation);
		if (AttachableObject.TryGetComponent<AttachableObject>(out AttachableObject obj)){
			obj.AttachObject(_thisObject);
		}
	}
}
