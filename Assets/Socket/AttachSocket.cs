
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

	public PieceSO AttachableObjectSO => _thisObject.AttachableObjectSO;

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
			if (otherSocket.AttachableObjectSO == _connectedObjectSO)
			{
				Debug.Log("Attached");

				//AttachObject(other.attachedRigidbody.gameObject);
			}
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.TryGetComponent<AttachSocket>(out AttachSocket otherSocket))
		{
			if (otherSocket.AttachableObjectSO == _connectedObjectSO)
			{
				Debug.Log("Detached");
			}
		}
	}

	public void AttachObject(GameObject AttachableObject)//obj´Â 
	{
		AttachableObject.transform.localPosition = _localAttachPosition;
		AttachableObject.transform.localRotation = Quaternion.Euler(_localAttachRotation);
		if (AttachableObject.TryGetComponent<AttachableObject>(out AttachableObject obj)){
			obj.AttachObject(_thisObject);
		}
	}
}
