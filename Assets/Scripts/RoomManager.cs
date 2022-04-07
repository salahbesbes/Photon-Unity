using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomManager : MonoBehaviourPunCallbacks
{
	public static RoomManager Instance;
	public MP_GameStateManager whiteManagerPrefab;
	public MP_GameStateManager blackManagerPrefab;
	MP_GameStateManager whitePlayer;
	MP_GameStateManager blackPlayer;

	public TextMeshProUGUI whiteText;
	public TextMeshProUGUI blackText;
	public Transform MyTeamHolder;
	public Transform OpponetHolder;

	public Button switchGameState;
	public Button switchPlayerState;
	public int whiteTeamManagerId;
	public int blackTeamManagerId;


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
				TEAM activeTEAM = (TEAM)PhotonNetwork.CurrentRoom.CustomProperties["ActiveTeam"];
				if (PhotonNetwork.LocalPlayer == PhotonNetwork.MasterClient)
				{

					object[] dataToInit = new object[4] { TEAM.White, TEAM.Black, activeTEAM, MyTeamHolder.transform.GetComponent<PhotonView>().ViewID };
					MP_GameStateManager pm = instantiateGameManager(whiteManagerPrefab.name, Vector3.zero, dataToInit);
					whitePlayer = pm;


					// TODO: Share Units to the other player
					whiteTeamManagerId = pm.photonView.ViewID;

					//pm.setDependencices(TEAM.White, TEAM.Black, activeTEAM, MyTeamHolder);
					pm.initGameManager();

					pm.generateUnits();
					//Debug.LogError($"this is master Player ");
					//Debug.LogError($"his team is {pm.MyTeam} and the active team is {whitePlayer.ActiveTeam}");

				}
				else
				{
					object[] dataToInit = new object[4] { TEAM.Black, TEAM.White, activeTEAM, MyTeamHolder.transform.GetComponent<PhotonView>().ViewID };
					MP_GameStateManager pm = instantiateGameManager(blackManagerPrefab.name, Vector3.zero, dataToInit);
					blackTeamManagerId = pm.photonView.ViewID;

					PhotonView go = PhotonView.Find((int)PhotonNetwork.MasterClient.CustomProperties["ViewID"]);
					whitePlayer = go.GetComponent<MP_GameStateManager>();
					blackPlayer = pm;
					//pm.setDependencices(TEAM.Black, TEAM.White, activeTEAM, MyTeamHolder);
					pm.initGameManager();

					pm.generateUnits();

				}

			}

		}
	}

	public void SwitchActiveTeam()
	{
		TEAM newActiveteam;
		TEAM CurrentActiveTeamOfRoom = (TEAM)PhotonNetwork.CurrentRoom.CustomProperties["ActiveTeam"];

		if (CurrentActiveTeamOfRoom == TEAM.White)
		{
			newActiveteam = TEAM.Black;
		}
		else if (CurrentActiveTeamOfRoom == TEAM.Black)
		{
			newActiveteam = TEAM.White;
		}
		else
		{
			newActiveteam = TEAM.None;
			Debug.LogError($"the new active team is None ");
		}

		PhotonNetwork.CurrentRoom.SetCustomProperty("ActiveTeam", newActiveteam);

		object content = newActiveteam;
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
		PhotonNetwork.RaiseEvent((byte)Ev.SwitchState, content, raiseEventOptions, SendOptions.SendReliable);

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