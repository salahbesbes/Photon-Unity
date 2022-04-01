using Photon.Pun;
using System.IO;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
	[Header("Game mode dependent objects")]
	//[SerializeField] private SingleplayerChessGameController singleplayerControllerPrefab;
	[SerializeField] private MultiplayerGameController multiplayerControllerPrefab;
	//[SerializeField] private SinglePlayerBoard singleplayerBoardPrefab;

	[Header("Scene references")]
	public NetworkManager networkManager;
	public Transform cameraSetup;
	public UiManager uiManager;

	public Board blackBoard;
	public Board whiteBoard;
	public Transform BlackAncker;
	public Transform WhiteAncker;
	public GameObject CreateMultiplayerBoard(TEAM team)
	{
		if (!networkManager.IsRoomFull())
		{
			if (team == TEAM.White)
				return PhotonNetwork.Instantiate(Path.Combine("Prefab", "WhiteBoard"), WhiteAncker.position, WhiteAncker.rotation);
			else if (team == TEAM.Black)
				return PhotonNetwork.Instantiate(Path.Combine("Prefab", "BlackBoard"), BlackAncker.position, BlackAncker.rotation);

		}

		return null;
	}


	public MultiplayerGameController InitializeMultiplayerController(TEAM team)
	{
		GameObject res = CreateMultiplayerBoard(team);
		MultiplayerBoard board = res.GetComponent<MultiplayerBoard>();

		res = PhotonNetwork.Instantiate(Path.Combine("Prefab", "MultiplayerController"), WhiteAncker.position, WhiteAncker.rotation);
		MultiplayerGameController controller = res.GetComponent<MultiplayerGameController>();
		controller.SetDependencies(cameraSetup, uiManager, board);
		board.SetDependencies(controller);
		controller.SetNetworkManager(networkManager);
		controller.checkControllerInit();
		controller.InitializeGame(WhiteAncker, whiteBoard, BlackAncker, blackBoard, controller);

		//networkManager.SetDependencies(controller);

		return controller;

	}



	//[SerializeField] private CameraSetup cameraSetup;
	//[SerializeField] private ChessUIManager uiManager;
}