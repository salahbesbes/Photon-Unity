using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MP_GameStateManager : SP_GameStateManager, IOnEventCallback
{
	[SerializeField] private TEAM _myteam;
	public TEAM MyTeam { get => _myteam; private set => _myteam = value; }


	[SerializeField] private TEAM _ActiveTeam;




	public new BaseState<MP_GameStateManager> State { get; private set; }
	public PlayingState playingState { get; private set; } = new PlayingState();
	public PauseState pauseState { get; private set; } = new PauseState();




	Player sender;


	/// <summary>
	/// Wraps accessing the "turn" custom properties of a room.
	/// </summary>
	/// <value>The turn index</value>
	public TEAM ActiveTeam
	{
		get { return PhotonNetwork.CurrentRoom.GetTurn(); }
		private set
		{
			PhotonNetwork.CurrentRoom.SetTurn(value, true);
		}
	}

	public float TurnDuration = 20f;

	public float ElapsedTimeInTurn
	{
		get { return ((float)(PhotonNetwork.ServerTimestamp - PhotonNetwork.CurrentRoom.GetStartTimeTurn())) / 1000.0f; }
	}


	public float RemainingSecondsInTurn
	{
		get { return Mathf.Max(0f, this.TurnDuration - this.ElapsedTimeInTurn); }
	}


	public bool IsCompletedByAll
	{
		get { return true; }
	}

	public bool IsFinishedByMe
	{
		get { return false; }
	}

	public bool TurnIsOver
	{
		get { return this.RemainingSecondsInTurn <= 0f; }
	}

	public IPunTurnManagerCallbacks TurnManagerListener;




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
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	public override void Start()
	{

	}





	#region Callbacks

	// called internally
	void ListenOnEvent(byte eventCode, object content, int senderId)
	{
		if (senderId == -1)
		{
			return;
		}

		sender = PhotonNetwork.CurrentRoom.GetPlayer(senderId);

		if (eventCode == (byte)Ev.SwitchState)
		{
			Debug.Log($"{PhotonNetwork.LocalPlayer.NickName} listen to {Ev.SwitchState} Event sent by {sender} ");


			string res = string.Join("\n", $" listen to {Ev.SwitchState} => " +
				$"{this.GetType().Name} has active team {ActiveTeam}" +
				$" and my team is {MyTeam} " +
				$"and room ActiveTeam in Romm is  \"{ActiveTeam}\"",
				$"");
			//Debug.LogError(res);
			if (MyTeam == TEAM.Black)
				RoomManager.Instance.blackText.text = res;
			if (MyTeam == TEAM.White)
				RoomManager.Instance.whiteText.text = res;

			if (ActiveTeam != MyTeam) SwitchState(pauseState);
			else if (ActiveTeam == MyTeam) SwitchState(playingState);
			else Debug.Log($"event contenent is {ActiveTeam}");
		}
	}



	public void OnEvent(EventData photonEvent)
	{
		this.ListenOnEvent(photonEvent.Code, photonEvent.CustomData, photonEvent.Sender);
	}


	#endregion




	public void SwitchActiveTeam()
	{

		if (IsMyTurn() == false) return;


		if (ActiveTeam == TEAM.White)
		{
			ActiveTeam = TEAM.Black;
		}
		else if (ActiveTeam == TEAM.Black)
		{
			ActiveTeam = TEAM.White;
		}
		else
		{
			ActiveTeam = TEAM.None;
			Debug.LogError($"the new active team is None ");
		}

		// RAISE EVENT
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
		PhotonNetwork.RaiseEvent((byte)Ev.SwitchState, null, raiseEventOptions, SendOptions.SendReliable);

	}

	private bool IsMyTurn()
	{
		return ActiveTeam == MyTeam;
	}

	[SerializeField] private UnitPhoton _selectedUnitMP;

	public new UnitPhoton SelectedUnit
	{
		get => _selectedUnitMP;
		set
		{
			if (SelectedUnit != null)
				SelectedUnit.enabled = false;


			_selectedUnitMP = value;

			SelectedUnit.enabled = true;
			Debug.Log($" selected id {SelectedUnit.photonView.ViewID}  {this}");
		}

	}




	public void SetDependenties(List<UnitPhoton> units, List<UnitPhoton> opponentList, TEAM myTeam)
	{
		MyTeam = myTeam;
		foreach (UnitPhoton unit in units)
		{
			Units.Add(unit);
		}
		foreach (UnitPhoton unit in opponentList)
		{
			EnemyUnits.Add(unit);
		}
	}


	public void generateUnits()
	{
		for (int i = 0; i < Units.Count; i++)
		{

			UnitPhoton newUnit = CreateController(Units[i].name, Units[i].transform.position);
			Units[i] = newUnit;
			Debug.Log($"selected unit is {SelectedUnit}");
			Units[i].transform.SetParent(this.transform);
			if (Units[i] == SelectedUnit)
			{
				Debug.Log($"   selected unit is equal one of the units True");
			}
		}
	}

	private UnitPhoton CreateController(string unitModelName, Vector3 ancherPoint, Quaternion? rotation = null)
	{
		Quaternion rot = rotation ?? Quaternion.identity;
		// we send this instance (gamestateManeger) to the UnitPhoton
		GameObject go = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", unitModelName), ancherPoint, rot, 0, new object[] { photonView.ViewID, });
		return go.GetComponent<UnitPhoton>();
	}








	private void Update()
	{
		State.Update(this);
	}



	public void initGameManager()
	{
		generateUnits();
		ActiveTeam = PhotonNetwork.CurrentRoom.GetTurn();
		Debug.LogError($" active team = {ActiveTeam} myteam is {MyTeam}");
		if (ActiveTeam == MyTeam)
		{
			SwitchState(playingState);

		}
		else
		{
			SwitchState(pauseState);
		}
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

	public void enableUnits(bool BOOL)
	{
		foreach (UnitPhoton unit in Units)
		{
			unit.enabled = BOOL;
			//if (BOOL)
			//	unit.GetComponent<Renderer>().material.color = Color.blue;
			//else
			//	unit.GetComponent<Renderer>().material.color = Color.red;



		}
	}

	public override string ToString()
	{
		return $" GM of player {transform.name} with viewID {photonView.ViewID}";
	}

}