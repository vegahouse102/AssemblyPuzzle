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
	[SerializeField]
	Transform _grabPos;

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
#if UNITY_EDITOR
		if (actionGetter.InputActions.Player.Grab.IsPressed())
		{
			Debug.DrawLine(_camera.position, _camera.position+ _camera.forward*_grabMaxDistance);
		}
#endif

		switch (grabState)
		{
			case GrabState.None:
				if (actionGetter.InputActions.Player.Grab.WasPressedThisFrame())
				{ 
					if (Physics.Raycast(_camera.position, _camera.forward, out RaycastHit info, _grabMaxDistance,1) )
					{
						if (info.collider.TryGetComponent<GrabableObject>(out GrabableObject obj))
						{
							//Debug.Log("grabhit");
							//Debug.DrawRay(_camera.position, _camera.forward);
							_grabedObject = obj;
							StateChange(GrabState.Grab);
						}
						else
						{
						}
						
					}
				}
				break;
			case GrabState.Grab:

				if (actionGetter.InputActions.Player.GrabRotation.WasPressedThisFrame())
				{
					OnStartGrabRotation?.Invoke();
				}else if (actionGetter.InputActions.Player.GrabRotation.WasReleasedThisFrame())
				{
					OnEndGrabRotation?.Invoke();
				}else if (actionGetter.InputActions.Player.GrabRotation.IsPressed())
				{

				}
				else
				{
					if (actionGetter.InputActions.Player.Grab.WasPressedThisFrame())
					{
						StateChange(GrabState.None);
					}
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
				_grabPos.position = _camera.position + _camera.forward * _grabDistance;
				if (actionGetter.InputActions.Player.GrabRotation.IsPressed())
				{
					Vector2 delta = actionGetter.InputActions.Player.Look.ReadValue<Vector2>();
					_grabedObject.Rotate(delta, _camera);

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
				_grabedObject.StartGrab(_grabPos);
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
