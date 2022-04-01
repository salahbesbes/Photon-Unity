using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Luncher : MonoBehaviourPunCallbacks
{
	public TMP_InputField roomNameInputField;
	public static Luncher Instance;
	public GameObject startButton;

	private void Awake()
	{
		if (Instance == null)
		{
			PhotonNetwork.AutomaticallySyncScene = true;
			Instance = this;
		}
		else
			Destroy(gameObject);
	}

	private void Start()
	{
		PhotonNetwork.ConnectUsingSettings();
		CanvasManager.Instance.openMenu("loading");
		Debug.Log($"try to connect to master server ");
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log($"connecte to master server ");

		PhotonNetwork.JoinLobby();
		// automaticly load the scene to all client
		PhotonNetwork.AutomaticallySyncScene = true;
	}

	public override void OnJoinedLobby()
	{
		Debug.Log($"joined Lobby");
		CanvasManager.Instance.openMenu("Menu");
		PhotonNetwork.NickName = $"Player {Random.Range(0, 40)}";
	}

	public void CreateRoom()
	{
		if (string.IsNullOrEmpty(roomNameInputField?.text))
		{
			CanvasManager.Instance.showErrorMessage("Rooom Name cant be Null or Empty");
			return;
		}
		CanvasManager.Instance.openMenu("loading");
		PhotonNetwork.CreateRoom(roomNameInputField.text);
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		CanvasManager.Instance.showErrorMessage($"{returnCode} : {message}");
		Debug.Log($" created room Failed");
	}

	public override void OnJoinedRoom()
	{
		RoomMenu roomMenu = CanvasManager.Instance.getMenu<RoomMenu>("room");
		if (roomMenu == null) return;
		roomMenu.roomMenuNameGui.text = PhotonNetwork.CurrentRoom.Name;
		CanvasManager.Instance.openMenu("room");

		foreach (Transform child in roomMenu.playersContainer)
		{
			Destroy(child.gameObject);
		}

		foreach (Player player in PhotonNetwork.PlayerList)
		{
			Instantiate(roomMenu.playerPrefab, roomMenu.playersContainer).GetComponent<PlayerItem>().SetUp(player);
		}

		startButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
		//Debug.Log($"{PhotonNetwork.LocalPlayer} joinned {PhotonNetwork.CurrentRoom.Name} room");
	}

	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		CanvasManager.Instance.showErrorMessage($"{returnCode} : {message}");
	}

	public override void OnLeftRoom()
	{
		CanvasManager.Instance.openMenu("Menu");
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		RoomListMenu roomListMenu = CanvasManager.Instance.getMenu<RoomListMenu>("roomListMenu");
		Debug.Log($"{roomList.Count}");
		foreach (Transform child in roomListMenu.roomsContainer)
		{
			Destroy(child.gameObject);
		}

		foreach (RoomInfo room in roomList)
		{
			Debug.Log($"{room.Name}");
			if (room.RemovedFromList) continue;
			Instantiate(roomListMenu.roomPrefab, roomListMenu.roomsContainer).GetComponent<RoomItem>().SetUp(room);
		}
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		RoomMenu roomMenu = CanvasManager.Instance.getMenu<RoomMenu>("room");

		Instantiate(roomMenu.playerPrefab, roomMenu.playersContainer).GetComponent<PlayerItem>().SetUp(newPlayer);
	}

	public override void OnMasterClientSwitched(Player newMasterClient)
	{
		startButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
	}

	public void JoinRoom(RoomInfo room)
	{
		CanvasManager.Instance.openMenu("loading");
		PhotonNetwork.JoinRoom(room.Name);
	}

	public void leaveRoom()
	{
		CanvasManager.Instance.openMenu("loading");
		PhotonNetwork.LeaveRoom();
	}

	public void StartGame()
	{
		// 1 is the index of the scene we want to load in the build menu
		PhotonNetwork.LoadLevel(1);
	}
}