using UnityEngine;
using System;
using System.Collections.Generic;
public class AttachableObject : MonoBehaviour
{
	[SerializeField]
	PieceSO _AttachableObjectSO;

	public PieceSO AttachableObjectSO => _AttachableObjectSO;


	public bool IsAttachable { get; private set; } = true;


	public void SetAttachable(bool value)
	{
		IsAttachable = value;
	}
	public void AttachObject(AttachableObject otherAttachableObject)
	{
		if (!IsAttachable)
		{
			return;
		}

	}




}

