using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class MP_GameStateManager : SP_GameStateManager, IOnEventCallback
{
	public TEAM MyTeam { get; private set; }
	public TEAM opponentTeam { get; private set; }
	public TEAM ActiveTeam { get; private set; }

	private Transform parent;





	public new BaseState<MP_GameStateManager> State { get; private set; }
	public PlayingState playingState { get; private set; } = new PlayingState();
	public PauseState pauseState { get; private set; } = new PauseState();





	[SerializeField] protected new UnitPhoton _selectedUnit;

	public new UnitPhoton SelectedUnit
	{
		get => _selectedUnit; set
		{
			_selectedUnit = value;
		}
	}


	public new List<UnitPhoton> Units
	{
		get => _units;
		set => _units = value;

	}


	public new List<UnitPhoton> EnemyUnits
	{
		get => _enemyUnits;
		set => _enemyUnits = value;
	}
	public override void Awake()
	{
		// Reading Data sent by the roomManager (the creator of the this component)
		MyTeam = (TEAM)photonView.InstantiationData[0];
		opponentTeam = (TEAM)photonView.InstantiationData[1];
		ActiveTeam = (TEAM)photonView.InstantiationData[2];

		//Hashtable hash = new Hashtable();
		//hash.Add("ViewID", photonView.ViewID);
		//PhotonNetwork.LocalPlayer.SetCustomProperties(hash);


		if (photonView.IsMine == false)
		{
			parent = GameObject.FindWithTag("opponnetTeam").transform;
			transform.SetParent(parent);
			enabled = false;
		}
		else
		{
			parent = PhotonView.Find((int)photonView.InstantiationData[3]).transform;
			transform.SetParent(parent);

		}



	}


	public void generateUnits()
	{
		for (int i = 0; i < Units.Count; i++)
		{

			UnitPhoton newUnit = CreateController(Units[i].name, Units[i].transform.position);
			Units[i] = newUnit;
		}
	}

	private UnitPhoton CreateController(string unitModelName, Vector3 ancherPoint, Quaternion? rotation = null)
	{
		Quaternion rot = rotation ?? Quaternion.identity;
		// we send this instance (gamestateManeger) to the UnitPhoton
		GameObject go = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", unitModelName), ancherPoint, rot, 0, new object[] { photonView.ViewID, });
		return go.GetComponent<UnitPhoton>();
	}




	[PunRPC]
	public void ReceiveEnemyList(List<object> enemies)
	{
		foreach (var item in enemies)
		{
			Debug.Log($"{item}");
		}

		EnemyUnits = enemies.Cast<UnitPhoton>().ToList();
		photonView.RPC("ReceiveEnemyList", RpcTarget.Others, new object[] { Units.Cast<object>().ToArray() });
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



	public void initGameManager()
	{

		//transform.SetParent(parent);
		SwitchState(playingState);

	}
	public void setDependencices(TEAM team, TEAM opponentTeam, TEAM activeTeam, Transform parent)
	{
		MyTeam = team;
		ActiveTeam = activeTeam;
		this.opponentTeam = opponentTeam;
		this.parent = parent;
	}


	public override void OnEnable()
	{
		base.OnEnable();
		PhotonNetwork.AddCallbackTarget(this);
	}

	public override void OnDisable()
	{
		base.OnDisable();
		PhotonNetwork.RemoveCallbackTarget(this);

	}
	public void SwitchState(BaseState<MP_GameStateManager> newState)
	{
		State?.ExitState(this);
		State = newState;
		State.EnterState(this);
	}

	private void enableUnits(bool BOOL)
	{
		foreach (UnitPhoton unit in Units)
		{
			unit.enabled = BOOL;
		}
	}

	public override string ToString()
	{
		return $" GM of player {transform.name} with viewID {photonView.ViewID}";
	}

}