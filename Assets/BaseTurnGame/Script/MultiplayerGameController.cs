using UnityEngine;

public class MultiplayerGameController : GameController
{
	private NetworkManager networkManager;
	private LocalPlayer localPlayer;
	private UiManager UIManager;
	private Transform cameraSetup;



	public void StartNewGame()
	{
		checkControllerInit();
		UIManager.OnGameStarted();
		//CreatePiecesFromLayout(startingBoardLayout);
		TryToStartThisGame();

	}
	public void TryToStartThisGame()
	{

		if (networkManager.IsRoomFull())
		{
			Debug.Log($"game started");
		}

	}


	internal void SetNetworkManager(NetworkManager networkManager)
	{
		this.networkManager = networkManager;
	}

	internal void SetDependencies(Transform cameraSetup, UiManager UIManager, MultiplayerBoard board)
	{
		this.cameraSetup = cameraSetup;
		this.UIManager = UIManager;
	}

	public void checkControllerInit()
	{
		if (cameraSetup == null) Debug.Log($"{nameof(cameraSetup)} is null");
		if (UIManager == null) Debug.Log($"{nameof(UIManager)} is null");
		if (networkManager == null) Debug.Log($"{nameof(networkManager)} is null");
	}

	public LocalPlayer getLocalPlayer()
	{
		return localPlayer;
	}

	public void EndTurn()
	{
		ChangeActiveTeam();
	}

	public void ChangeActiveTeam()
	{
		if (ActivePlayer is WhitePlayer)
			ActivePlayer = blackPlayer;
		else if (ActivePlayer is BlackPlayer)
			ActivePlayer = whitePlayer;
	}
	public void SetLocalPlayer(TEAM team)
	{
		if (team == TEAM.Black)
		{
			ActivePlayer = blackPlayer;
			localPlayer = blackPlayer;
		}
		else if (team == TEAM.White)
		{
			ActivePlayer = whitePlayer;
			localPlayer = whitePlayer;
		}
	}


	public void Update()
	{
		ActivePlayer.Update();
	}








}