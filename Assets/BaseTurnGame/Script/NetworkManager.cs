using Photon.Pun;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
	[SerializeField] private GameInitializer gameInitializer;
	[SerializeField] private UiManager uiManager;

	public MultiplayerGameController multiplayerGameController { get; private set; }

	private void Awake()
	{
	}

	public void Connect()
	{
		if (PhotonNetwork.IsConnected)
		{
			PhotonNetwork.JoinRandomRoom();
		}
		else
		{
			PhotonNetwork.ConnectUsingSettings();
		}
	}

	public override void OnConnectedToMaster()
	{
		Debug.LogError($"Connected to server. Looking for random ");
		PhotonNetwork.JoinRandomRoom();
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		Debug.LogError($"Joining random room failed becuse of {message}. Creating new one ");
		PhotonNetwork.CreateRoom("Room1");
		//PhotonNetwork.CreateRoom(null);
	}

	public bool IsRoomFull()
	{
		return PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers;
	}

	public override void OnJoinedRoom()
	{
		Debug.LogError($"Player {PhotonNetwork.LocalPlayer.ActorNumber} joined a room ");
		//gameInitializer.CreateMultiplayerBoard();
		PrepareTeamSelectionOptions();
		uiManager.ShowTeamSelectionScreen();
	}

	internal void SetDependencies(MultiplayerGameController controller)
	{
		multiplayerGameController = controller;
	}

	public void SetPlayerTeam(int team)
	{
		TEAM selectedTeam = (TEAM)team;
		if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
		{
			var player = PhotonNetwork.CurrentRoom.GetPlayer(1);
			if (player.CustomProperties.ContainsKey("team"))
			{
				TEAM occupiedTeam = (TEAM)player.CustomProperties["team"];
				selectedTeam = occupiedTeam == TEAM.Black ? TEAM.White : TEAM.Black;
				MultiplayerGameController controller = gameInitializer.InitializeMultiplayerController(selectedTeam);
				SetDependencies(controller);
				multiplayerGameController.SetLocalPlayer(occupiedTeam);
				multiplayerGameController.StartNewGame();
				return;
			}
		}
		PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "team", selectedTeam } });


		//chessGameController.SetupCamera((TeamColor)teamInt);
	}

	private void PrepareTeamSelectionOptions()
	{
		if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
		{
			var player = PhotonNetwork.CurrentRoom.GetPlayer(1);
			if (player.CustomProperties.ContainsKey("team"))
			{
				TEAM occupiedTeam = (TEAM)player.CustomProperties["team"];
				uiManager.DisableButton(occupiedTeam);
			}
		}
	}
}

public enum TEAM
{
	None,
	Black,
	White
}