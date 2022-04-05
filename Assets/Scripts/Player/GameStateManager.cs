using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameStateManager : MonoBehaviour, IOnEventCallback
{
	private PhotonView PV;
	public TEAM MyTeam { get; private set; }
	public TEAM ActiveTeam { get; private set; }


	public RoomManager roomManager { get; private set; }
	//[SerializeField] private Transform unitHolder;
	[SerializeField] private List<UnitPhoton> units = new List<UnitPhoton>();

	public BaseState<GameStateManager> State { get; private set; }
	public PlayingState playingState { get; private set; } = new PlayingState();
	public PauseState pauseState { get; private set; } = new PauseState();

	public List<Vector3> positions = new List<Vector3>();



	private UnitPhoton _selectedUnit;

	public UnitPhoton SelectedUnit
	{
		get => _selectedUnit; set
		{
			_selectedUnit = value;

			//Debug.Log($"Selected  {SelectedUnit} ");
		}
	}



	//private void Awake()
	//{
	//	generateUnits();

	//}

	public void generateUnits()
	{

		foreach (Vector3 pos in positions)
		{
			UnitPhoton unit = CreateController("PlayerController", pos);
			unit.parent = transform;
			unit.setDependencies(this);
			units.Add(unit);

		}


	}

	private UnitPhoton CreateController(string unitModelName, Vector3 ancherPoint, Quaternion? rotation = null)
	{
		Quaternion rot = rotation ?? Quaternion.identity;
		GameObject go = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", unitModelName), ancherPoint, rot);
		return go.GetComponent<UnitPhoton>();
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



	private void initGameManager()
	{
		SwitchState(playingState);

	}
	public void setDependencices(TEAM team, TEAM activeTeam, RoomManager roomManager)
	{
		MyTeam = team;
		ActiveTeam = activeTeam;
		this.roomManager = roomManager;

		initGameManager();
		//initUnits();
	}

	private void initUnits()
	{
		foreach (UnitPhoton unit in units)
		{
			//unit.initUnit(this, roomManager);
		}
	}

	private void OnEnable()
	{
		PhotonNetwork.AddCallbackTarget(this);
	}

	private void OnDisable()
	{
		PhotonNetwork.RemoveCallbackTarget(this);

	}
	public void SwitchState(BaseState<GameStateManager> newState)
	{
		State?.ExitState(this);
		State = newState;
		State.EnterState(this);
	}

	private void enableUnits(bool BOOL)
	{
		foreach (UnitPhoton unit in units)
		{
			unit.enabled = BOOL;
		}
	}

}