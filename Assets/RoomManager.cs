using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
	public static RoomManager Instance;
	public PlayerManager whiteManagerPrefab;
	public PlayerManager blackManagerPrefab;
	PlayerManager whitePlayer;
	PlayerManager blackPlayer;

	public TextMeshProUGUI whiteText;
	public TextMeshProUGUI blackText;
	public Transform MyTeamHolder;
	public Transform OpponetHolder;






	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}



	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		Debug.Log($"{scene.name} is loaded");
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
					PlayerManager pm = Instantiate(whiteManagerPrefab, Vector3.zero, Quaternion.identity);
					pm.setDependencices(TEAM.White, activeTEAM, this);
					pm.transform.SetParent(MyTeamHolder);
					whitePlayer = pm;

					Debug.LogError($"this is master Player ");
					Debug.LogError($"his team is {pm.MyTeam} and the active team is {whitePlayer.ActiveTeam}");

				}
				else
				{
					PlayerManager pm = Instantiate(blackManagerPrefab, Vector3.zero, Quaternion.identity);
					pm.setDependencices(TEAM.Black, activeTEAM, this);
					blackPlayer = pm;
					pm.transform.SetParent(MyTeamHolder);
					Debug.LogError($"this is Client");
					Debug.LogError($"his team is {blackPlayer.MyTeam} and the active team is {blackPlayer.ActiveTeam}");


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