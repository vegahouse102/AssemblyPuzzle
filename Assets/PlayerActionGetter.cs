using UnityEngine;

public class PlayerActionGetter : MonoBehaviour
{
	PlayerAction _inputActions;
	public void Start()
	{
		
	}
	public PlayerAction InputActions { get { return _inputActions; } }
}
