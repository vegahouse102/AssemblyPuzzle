using UnityEngine;


public class PlayerGrab : MonoBehaviour
{
	[SerializeField]
	PlayerActionGetter actionGetter;
	[SerializeField]
	Transform _camera;
	[SerializeField]
	float _grabMaxDistance;
	[SerializeField]
	float _grabForce;

	[SerializeField]
	float _grabDistance;



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
				
				Vector3 pos = _camera.position+ _camera.forward * _grabDistance;
				_grabedObject.transform.rotation = Quaternion.identity;
				if(_grabedObject.TryGetComponent(out Rigidbody rigidbody))
				{
					_grabedObject.Grab();
					Vector3 force = (pos - _grabedObject.transform.position) * _grabForce;
				
					rigidbody.AddForce(force, ForceMode.Acceleration);
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
