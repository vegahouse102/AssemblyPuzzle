using DG.Tweening;
using System.Collections.Generic;
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
	}
	public void AttachObject(AttachableObject attachedSocketAttachableObject,Transform attachedSocketConnection,Transform thisSocketConnection)
	{
		if (!IsAttachable)
		{
			return;
		}
		if (attachedSocketAttachableObject == this)
			return;

		Debug.Log("AttachedObject");
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

		Quaternion targetRotation =
		    Quaternion.LookRotation(
			-thisSocketConnection.forward,
			thisSocketConnection.up
		    );//맞닿는 connection들은서로  rotation의 z축이 반대여야함여야함 up축방향이 같아야함
		Quaternion turn = targetRotation * Quaternion.Inverse(attachedSocketConnection.rotation);
		

		if (attachedSocketAttachableObject.transform.parent == null)
		{
			
			attachedSocketAttachableObject.transform.rotation = turn*attachedSocketAttachableObject.transform.rotation;


			Vector3 posDiff = thisSocketConnection.position - attachedSocketConnection.position;
			attachedSocketAttachableObject.transform.position += posDiff;

			attachedSocketAttachableObject.transform.parent = root.transform;
		}
		else
		{
			attachedSocketAttachableObject.transform.parent.rotation = turn * attachedSocketAttachableObject.transform.parent.rotation;
			Vector3 posDiff = thisSocketConnection.position - attachedSocketConnection.transform.position;
			attachedSocketAttachableObject.transform.parent.position += posDiff;

			List<Transform> childs = new();
			Transform attachedParentTransform = attachedSocketAttachableObject.transform.parent;

			for (int i = 0; i <  attachedParentTransform.childCount; i++)
			{
				childs.Add(attachedParentTransform.GetChild(i));
			}
			foreach (Transform child in childs)
			{
				child.SetParent(root.transform, true);
			}
			//Debug.Log(attachedParentTransform.gameObject.name);
			//Destroy(attachedParentTransform.gameObject);
		}
	}

	private GameObject GetAssemblyGameObject()
	{
		GameObject result = new GameObject("AssemblyObject");
		result.AddComponent<Rigidbody>();
		result.AddComponent<AssemblyObject>();
		result.layer = _ignoreLayer;
		return result;
	}




}

