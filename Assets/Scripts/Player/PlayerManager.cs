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

	private void Awake()
	{
		generateUnits();
	}


	public void generateUnits()
	{

		unitHolder = Instantiate(unitHolder, unitHolder.transform.position, unitHolder.rotation, transform);
		foreach (Transform child in unitHolder)
		{
			PlayerControler unit = CreateController("PlayerController", Vector3.zero);
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






	public void setDependencices(TEAM team, TEAM activeTeam, RoomManager roomManager)
	{
		MyTeam = team;
		ActiveTeam = activeTeam;
		this.roomManager = roomManager;

	}
	public void SwitchActiveTeam()
	{
		PV.RPC(nameof(SwitchActiveTeam), RpcTarget.All);
	}
	[PunRPC]
	public void RPC_SwitchActiveTeam()
	{
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
			Debug.LogError($"activeteam is None");
		}

		Debug.LogError($"  {this.GetType().Name} has active team {ActiveTeam}");
	}

	private void OnEnable()
	{
		PhotonNetwork.AddCallbackTarget(this);
	}

	private void OnDisable()
	{
		PhotonNetwork.RemoveCallbackTarget(this);
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

			if (MyTeam == TEAM.Black)
				roomManager.blackText.text = res;
			if (MyTeam == TEAM.White)
				roomManager.whiteText.text = res;


		}
	}
}