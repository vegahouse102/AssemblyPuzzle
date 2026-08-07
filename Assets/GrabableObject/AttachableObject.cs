using UnityEngine;
using System;
using System.Collections.Generic;
public class AttachableObject : MonoBehaviour
{
	[SerializeField]
	PieceSO _AttachableObjectSO;

	public PieceSO AttachableObjectSO => _AttachableObjectSO;

	public event Action<ChangeTransformDTO> OnChangeTransfrom;
	private List<AttachableObject> _connectedAttachableObjects = new();

	private Vector3 _prevPosition;
	private Vector3 _prevRotation;
	private void OnDestroy()
	{
		foreach(var attachedObject in _connectedAttachableObjects)
		{
			attachedObject.OnChangeTransfrom -= HandleOnChangeTransform;
		}
	}

	private void FixedUpdate()
	{
		
	}
	public void AttachObject(AttachableObject otherAttachableObject)
	{
		_connectedAttachableObjects.Add(otherAttachableObject);
		otherAttachableObject.OnChangeTransfrom += HandleOnChangeTransform;
	}
	public void HandleOnChangeTransform(ChangeTransformDTO changeDTO)
	{

	}

}
public struct ChangeTransformDTO
{
	public Vector3 DeltaPosition;
	public Vector3 DeltaRotation;
	public GameObject ChangedObject;
}
