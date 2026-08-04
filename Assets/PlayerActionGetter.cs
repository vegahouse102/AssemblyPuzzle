using UnityEngine;

public class PlayerActionGetter : MonoBehaviour
{
	PlayerAction _inputActions;
	private void Awake()
	{
		_inputActions = new PlayerAction();
	}
	private void OnEnable()
	{
		_inputActions.Enable();
	}
	private void OnDisable()
	{
		_inputActions.Disable();
	}
	public PlayerAction InputActions { get { return _inputActions; } }
}
