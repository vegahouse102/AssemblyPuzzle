using UnityEngine;
using UnityEngine.Events;

public class PlayerGrab : MonoBehaviour
{
	[SerializeField]
	PlayerActionGetter actionGetter;
	[SerializeField]
	Transform _camera;
	[SerializeField]
	float _grabMaxDistance;

	[SerializeField]
	float _grabDistance;
	[SerializeField] 
	float _grabSpeed;
	[SerializeField]
	float _rotationSpeed;
	

	public UnityEvent OnStartGrabRotation;
	public UnityEvent OnEndGrabRotation;


	GrabState _curstate;
	GrabableObject _grabedObject;
	private void Start()
	{
		StateChange(GrabState.None);
	}
	void Update()
	{
		OnUpdate(_curstate);
	}
	private void FixedUpdate()
	{
		OnFixedUpdate(_curstate);
	}
	void OnUpdate(GrabState grabState)
	{
		switch (grabState)
		{
			case GrabState.None:
				if (actionGetter.InputActions.Player.Grab.WasPressedThisFrame())
				{
					if (Physics.Raycast(_camera.position,_camera.forward,out RaycastHit info,_grabMaxDistance)&&info.transform.TryGetComponent<GrabableObject>(out GrabableObject obj))
					{
						_grabedObject = obj;
						StateChange(GrabState.Grab);
					}
				}
				break;
			case GrabState.Grab:
				if (actionGetter.InputActions.Player.Grab.WasPressedThisFrame())
				{
					StateChange(GrabState.None);
				}
				if (actionGetter.InputActions.Player.GrabRotation.WasPressedThisFrame())
				{
					OnStartGrabRotation?.Invoke();
				}else if (actionGetter.InputActions.Player.GrabRotation.WasReleasedThisFrame())
				{
					OnEndGrabRotation?.Invoke();
				}
				if (actionGetter.InputActions.Player.GrabRotation.IsPressed())
				{
					Vector2 delta = actionGetter.InputActions.Player.Look.ReadValue<Vector2>();
					Quaternion rot =
					    Quaternion.AngleAxis(delta.y * _rotationSpeed * Time.deltaTime, _camera.right) *
					    Quaternion.AngleAxis(-delta.x * _rotationSpeed * Time.deltaTime, _camera.up);
					Rigidbody rigidbody = _grabedObject.GetComponent<Rigidbody>();
					rigidbody.MoveRotation(rot * rigidbody.rotation);
				}
				break;
		}
	}
	void OnFixedUpdate(GrabState grabState)
	{
		switch (grabState)
		{
			case GrabState.None:
				break;
			case GrabState.Grab:

				if (_grabedObject.TryGetComponent(out Rigidbody rigidbody))
				{
					Vector3 targetPos = _camera.position + _camera.forward * _grabDistance;

					Vector3 velocity = (targetPos - rigidbody.position) * _grabSpeed;

					rigidbody.linearVelocity = velocity;
					rigidbody.angularVelocity = Vector3.zero;
				}
				break;
		}
	}
	void OnStart(GrabState grabState)
	{
		switch (grabState)
		{
			case GrabState.None:
				break;
			case GrabState.Grab:
				break;
		}
	}
	void OnEnd(GrabState grabState)
	{
		switch (grabState)
		{
			case GrabState.None:
				break;
			case GrabState.Grab:
				_grabedObject.UnGrab();
				break;
		}
	}
	void StateChange(GrabState grabState)
	{
		OnEnd(_curstate);
		OnStart(grabState);
		_curstate = grabState;
	}
	enum GrabState
	{
		None,
		Grab
	}
}
