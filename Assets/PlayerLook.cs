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
	void Update()
	{
		Vector2 delta = _actionGetter.InputActions.Player.Look.ReadValue<Vector2>() ;
		_player.Rotate(Vector3.up*_sensitivity*delta.x*Time.deltaTime);
		_curCameraDegree -= _sensitivity * delta.y * Time.deltaTime;
		_curCameraDegree = Mathf.Clamp(_curCameraDegree,_minCameraDegree,_maxCameraDegree);
		_camera.localRotation = Quaternion.Euler(_curCameraDegree,0,0);
	}
}
