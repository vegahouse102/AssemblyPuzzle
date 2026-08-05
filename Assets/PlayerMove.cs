using UnityEngine;

public class PlayerMove : MonoBehaviour
{
	[SerializeField]
	PlayerActionGetter playerAction;
	[SerializeField]
	CharacterController player;
	[SerializeField]
	float _moveSpeed;
	void Update()
	{
		Vector2 move = playerAction.InputActions.Player.Move.ReadValue<Vector2>();
		player.Move(_moveSpeed * (player.transform.forward * move.y + player.transform.right * move.x) * Time.deltaTime);

	}
}
