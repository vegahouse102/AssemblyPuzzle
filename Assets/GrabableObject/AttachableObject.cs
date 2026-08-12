using UnityEngine;

public class AttachableObject : MonoBehaviour
{
	[SerializeField]
	PieceSO _AttachableObjectSO;
	private Rigidbody _rigidbody;

	private int _ignoreLayer = 2;
	public PieceSO AttachableObjectSO => _AttachableObjectSO;


	public bool IsAttachable { get; private set; } = true;


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
		IsAttachable = !value;
	}
	public void Attached()
	{
		Destroy(_rigidbody);
	}
	public void AttachObject(AttachableObject otherAttachableObject,Vector3 attachedLocalPosition,Quaternion attachedLocalRotation,Vector3 thisLocalPosition,Quaternion thisLocalRotation)
	{
		if (!IsAttachable)
		{
			return;
		}
		Attached();
		otherAttachableObject.Attached();
		GameObject root = new GameObject("AssemblyObject");
		root.AddComponent<Rigidbody>();
		root.layer = _ignoreLayer;


		root.transform.position = transform.position- thisLocalPosition;
		root.transform.rotation = transform.rotation;

		transform.parent = root.transform;
		otherAttachableObject.transform.parent = root.transform;



		otherAttachableObject.transform.localPosition = attachedLocalPosition;
		otherAttachableObject.transform.localRotation = attachedLocalRotation;

	}




}

