using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PlayerItem : MonoBehaviourPunCallbacks
{
	Player player;
	public TextMeshProUGUI playerNameGui;
	public void SetUp(Player _player)
	{
		player = _player;
		playerNameGui.text = _player.NickName;
	}


	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		if (player == otherPlayer)
			Destroy(gameObject);
	}

	public override void OnLeftRoom()
	{
		Destroy(gameObject);
	}

	public void onClick()
	{
		Debug.Log($"{player.NickName} {player.ActorNumber} {player.UserId}");
	}
}
