using Photon.Pun;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomManager : MonoBehaviourPunCallbacks
{
	public static RoomManager Instance;
	//public MP_GameStateManager whiteManagerPrefab;
	//public MP_GameStateManager blackManagerPrefab;
	public MP_GameStateManager LocalPlayer;
	public MP_GameStateManager Opponent;

	public TextMeshProUGUI whiteText;
	public TextMeshProUGUI blackText;
	public Transform MyTeamHolder;
	public Transform OpponetHolder;

	public Button switchGameState;
	public Button switchPlayerState;
	public int whiteTeamManagerId;
	public int blackTeamManagerId;


	public List<UnitPhoton> BlackUnits = new List<UnitPhoton>();


	public List<UnitPhoton> WhiteUnits = new List<UnitPhoton>();

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}




	private MP_GameStateManager instantiateGameManager(string unitModelName, Vector3 ancherPoint, object[] instantiateData, Quaternion? rotation = null)
	{

		Quaternion rot = rotation ?? Quaternion.identity;
		GameObject go = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", unitModelName), ancherPoint, rot, 0, new object[] { instantiateData[0], instantiateData[1], instantiateData[2], instantiateData[3], });
		return go.GetComponent<MP_GameStateManager>();
	}
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		// check if we load the scene that have build index 1
		if (scene.buildIndex == 1)
		{
			if (PhotonNetwork.InRoom == false)
			{
				SceneManager.LoadScene(0);
			}
			else
			{
				//GameObject controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
				//PlayerManager pm = controller.GetComponent<PlayerManager>();
				if (PhotonNetwork.LocalPlayer == PhotonNetwork.MasterClient)
				{

					LocalPlayer.SetDependenties(WhiteUnits, BlackUnits, TEAM.White);
					LocalPlayer.initGameManager();




				}
				else
				{
					LocalPlayer.SetDependenties(BlackUnits, WhiteUnits, TEAM.Black);
					LocalPlayer.initGameManager();
				}

			}

		}
	}



	public override void OnEnable()
	{
		base.OnEnable();
		// since this is a singleton class this callback is called when ever any scene
		// changes
		SceneManager.sceneLoaded += OnSceneLoaded;
	}
	public override void OnDisable()
	{
		base.OnDisable();
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
}



enum Ev
{
	SwitchState,
}