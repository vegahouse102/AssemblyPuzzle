using UnityEngine;

public class PlayerMove : MonoBehaviour
{
	[SerializeField]
	PlayerActionGetter playerAction;
	[SerializeField]
	Transform player;
	[SerializeField]
	float _moveSpeed;
	void Update()
	{
		Vector2 move = playerAction.InputActions.Player.Move.ReadValue<Vector2>();
		player.position += _moveSpeed*(player.forward * move.y + player.right * move.x)*Time.deltaTime; 
	}
}
