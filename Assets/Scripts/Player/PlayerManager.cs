using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IOnEventCallback
{
	private PhotonView PV;
	public TEAM MyTeam { get; private set; }
	public TEAM ActiveTeam { get; private set; }


	public RoomManager roomManager { get; private set; }
	[SerializeField] private Transform unitHolder;
	[SerializeField] private List<PlayerControler> units = new List<PlayerControler>();

	public BaseState<PlayerManager> State { get; private set; }
	public PlayingState playingState { get; private set; } = new PlayingState();
	public PauseState pauseState { get; private set; } = new PauseState();


	private void Awake()
	{
		generateUnits();

	}

	public void generateUnits()
	{

		unitHolder = Instantiate(unitHolder, unitHolder.position, unitHolder.rotation, transform);
		foreach (Transform child in unitHolder)
		{
			PlayerControler unit = CreateController("PlayerController", child.position);
			unit.parent = child;
			units.Add(unit);

		}



	}

	private PlayerControler CreateController(string unitModelName, Vector3 ancherPoint, Quaternion? rotation = null)
	{
		Quaternion rot = rotation ?? Quaternion.identity;
		GameObject go = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", unitModelName), ancherPoint, rot);
		return go.GetComponent<PlayerControler>();
	}







	public void OnEvent(EventData photonEvent)
	{
		byte eventCode = photonEvent.Code;
		if (eventCode == (byte)Ev.SwitchState)
		{
			ActiveTeam = (TEAM)photonEvent.CustomData;
			TEAM activeTEAM = (TEAM)PhotonNetwork.CurrentRoom.CustomProperties["ActiveTeam"];
			string res = string.Join("\n", $" listen to {Ev.SwitchState} => " +
				$"{this.GetType().Name} has active team {ActiveTeam}" +
				$" and my team is {MyTeam} " +
				$"and room ActiveTeam in Romm is  \"{activeTEAM}\"",
				$"");
			Debug.LogError(res);

			//if (MyTeam == TEAM.Black)
			//	roomManager.blackText.text = res;
			//if (MyTeam == TEAM.White)
			//	roomManager.whiteText.text = res;

			//if (activeTEAM != MyTeam) enableUnits(false);
			//else if (activeTEAM == MyTeam) enableUnits(true);

		}
	}
	private void Update()
	{
		State.Update(this);
	}

	private void init()
	{
		SwitchState(playingState);
	}
	public void setDependencices(TEAM team, TEAM activeTeam, RoomManager roomManager)
	{
		MyTeam = team;
		ActiveTeam = activeTeam;
		this.roomManager = roomManager;
		init();
	}


	private void OnEnable()
	{
		PhotonNetwork.AddCallbackTarget(this);
		Debug.Log($"enable called ");
	}

	private void OnDisable()
	{
		PhotonNetwork.RemoveCallbackTarget(this);
		Debug.Log($"disable called ");

	}
	public void SwitchState(BaseState<PlayerManager> newState)
	{
		State?.ExitState(this);
		State = newState;
		State.EnterState(this);
	}

	private void enableUnits(bool BOOL)
	{
		foreach (PlayerControler unit in units)
		{
			unit.enabled = BOOL;
		}
	}

}