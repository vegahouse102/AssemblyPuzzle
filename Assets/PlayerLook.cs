using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
	[SerializeField]
	PlayerActionGetter _actionGetter;
	[SerializeField]
	Transform _player;
	[SerializeField]
	Transform _camera;
	[SerializeField]
	float _sensitivity;
	[SerializeField]
	float _minCameraDegree;
	[SerializeField]
	float _maxCameraDegree;
	float _curCameraDegree;
	bool _active = true;
	void Update()
	{
		if (!_active)
			return;
		Vector2 delta = _actionGetter.InputActions.Player.Look.ReadValue<Vector2>() ;
		_player.Rotate(Vector3.up*_sensitivity*delta.x*Time.deltaTime);
		_curCameraDegree -= _sensitivity * delta.y * Time.deltaTime;
		_curCameraDegree = Mathf.Clamp(_curCameraDegree,_minCameraDegree,_maxCameraDegree);
		_camera.localRotation = Quaternion.Euler(_curCameraDegree,0,0);
	}


	public void On()
	{
		_active = true;	
	}
	public void Off()
	{
		_active = false;
	}
}
