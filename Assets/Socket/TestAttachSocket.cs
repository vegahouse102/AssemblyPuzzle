using DG.Tweening;
using UnityEngine;

public class TestAttachSocket : MonoBehaviour
{
	[SerializeField]
	AttachSocket _socket;
	[SerializeField]
	GameObject _testAttachableObject;
	void Start()
	{
		Sequence sequence = DOTween.Sequence();
		sequence.AppendInterval(3f);
		sequence.AppendCallback(() =>  _socket.AttachObject(_testAttachableObject));
	}

	// Update is called once per frame
	void Update()
	{

	}
}

