using UnityEngine;

public class UiManager : MonoBehaviour
{
	[Header("Dependencies")]
	[SerializeField] private NetworkManager networkManager;

	[Header("Screen Gameobjects")]
	[SerializeField] private GameObject ConnectScreen;
	[SerializeField] private TeamScreen TeamSelectionScreen;


	[SerializeField] private GameObject GameModeSelectionScreen;

	private void Awake()
	{
		OnGameLaunched();
	}

	private void OnGameLaunched()
	{
		GameModeSelectionScreen.SetActive(true);
	}

	public void onSingleplayerModeSelected()
	{
		TeamSelectionScreen.gameObject.SetActive(false);
		ConnectScreen.SetActive(false);
		GameModeSelectionScreen.SetActive(false);
	}

	public void onMultiplayerModeSelected()
	{
		TeamSelectionScreen.gameObject.SetActive(false);
		ConnectScreen.SetActive(true);
		GameModeSelectionScreen.SetActive(false);
	}

	public void onConnectPressed()
	{
		networkManager.Connect();
	}

	public void SelectTeam(int team)
	{
		networkManager.SetPlayerTeam(team);
	}

	internal void ShowTeamSelectionScreen()
	{
		TeamSelectionScreen.gameObject.SetActive(true);
		ConnectScreen.SetActive(false);
		GameModeSelectionScreen.SetActive(false);
	}

	public void DisableButton(TEAM occupiedTeam)
	{
		if (occupiedTeam == TEAM.Black)
			TeamSelectionScreen.BlackBtn.interactable = false;
		else if (occupiedTeam == TEAM.White)
			TeamSelectionScreen.WhiteBtn.interactable = false;
	}

	public void OnGameStarted()
	{
		TeamSelectionScreen.gameObject.SetActive(false);
		ConnectScreen.SetActive(false);
		GameModeSelectionScreen.SetActive(false);
	}

	public void EndTurn()
	{
		networkManager.multiplayerGameController.EndTurn();
	}
}