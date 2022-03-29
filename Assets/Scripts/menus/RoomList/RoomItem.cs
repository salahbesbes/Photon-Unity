using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomItem : MonoBehaviour
{
	RoomInfo info;
	[SerializeField] TextMeshProUGUI roomNameGui;

	public void SetUp(RoomInfo room)
	{
		info = room;
		roomNameGui.text = room.Name;
	}
	public void onClick()
	{
		Luncher.Instance.JoinRoom(info);
	}
}
