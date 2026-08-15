using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class AttachableObject : MonoBehaviour
{
	[SerializeField]
	PieceSO _AttachableObjectSO;
	private Rigidbody _rigidbody;

	private int _ignoreLayer = 2;
	public PieceSO AttachableObjectSO => _AttachableObjectSO;


	public bool IsAttachable { get; private set; } = true;
	private bool _isAttached;

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}
	private void Start()
	{
		GrabableObject grabableObject = GetComponent<GrabableObject>();
		grabableObject.OnGrab.AddListener(SetAttachable);
		_ignoreLayer = LayerMask.NameToLayer("Ignore Raycast");
	}
	private void OnDestroy()
	{
		GrabableObject grabableObject = GetComponent<GrabableObject>();
		grabableObject.OnGrab.RemoveListener(SetAttachable);
	}



	public void SetAttachable(bool value)
	{
		if (_isAttached)
		{
			return;
		}
		IsAttachable = !value;
	}
	public void Attached()
	{
		if (_isAttached)
		{
			return;
		}
		Destroy(_rigidbody);
		_isAttached = true;
		IsAttachable = false;
	}
	public void AttachObject(AttachableObject attachedSocketAttachableObject,Transform attachedSocketConnection,Transform thisSocketConnection)
	{
		if (!IsAttachable)
		{
			return;
		}
		Attached();
		attachedSocketAttachableObject.Attached();
		GameObject root = null;
		if (transform.parent == null)
		{
			root = GetAssemblyGameObject();
			transform.parent = root.transform;
		}
		else
		{
			root = transform.parent.gameObject;
		}

		Quaternion attachedSocketResult = Quaternion.Inverse(thisSocketConnection.rotation);//맞닿는 connection들은 rotation이 inverse여야함
		Quaternion turn = attachedSocketResult * Quaternion.Inverse(attachedSocketConnection.rotation);

		Vector3 resultEuler = attachedSocketResult.eulerAngles;
		Vector3 turnEuler = turn.eulerAngles;
		if (attachedSocketAttachableObject.transform.parent == null)
		{
			Vector3 attachedStartEuler = attachedSocketAttachableObject.transform.rotation.eulerAngles;


			attachedSocketAttachableObject.transform.rotation = turn*attachedSocketAttachableObject.transform.rotation;


			Vector3 attachedEndEuler = attachedSocketAttachableObject.transform.rotation.eulerAngles;
			Vector3 thisSockectForward = thisSocketConnection.forward;
			Vector3 attachedSocketForward = attachedSocketConnection.forward;


			Vector3 posDiff = thisSocketConnection.position - attachedSocketConnection.position;
			attachedSocketAttachableObject.transform.position += posDiff;

			attachedSocketAttachableObject.transform.parent = root.transform;
			Debug.Log(
    $"this forward = {thisSocketConnection.forward}\n" +
    $"attached forward = {attachedSocketConnection.forward}"
			);
		}
		else
		{
			attachedSocketAttachableObject.transform.parent.rotation = turn * attachedSocketAttachableObject.transform.parent.rotation;
			Vector3 posDiff = thisSocketConnection.position - attachedSocketConnection.transform.position;
			attachedSocketAttachableObject.transform.parent.position += posDiff;
			foreach (Transform child in attachedSocketAttachableObject.transform.parent)
			{
				child.parent = root.transform;
			}
			Destroy(attachedSocketAttachableObject.transform.parent);
		}
	}

	private GameObject GetAssemblyGameObject()
	{
		GameObject result = new GameObject("AssemblyObject");
		result.AddComponent<Rigidbody>();
		result.layer = _ignoreLayer;
		return result;
	}




}

