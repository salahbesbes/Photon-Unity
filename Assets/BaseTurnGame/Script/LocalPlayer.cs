using System;
using UnityEngine;

[Serializable]
public class LocalPlayer
{

	public TEAM team { get; set; }
	public Board board { get; set; }

	public Transform ancherPoint;

	protected MultiplayerGameController controller;


	public LocalPlayer(TEAM team, Transform ancherPoint, Board board, MultiplayerGameController controller)
	{
		this.team = team;
		this.ancherPoint = ancherPoint;
		this.board = board;
		this.controller = controller;

	}

	public virtual void Update()
	{
		Debug.LogError($"defaut update");
	}



	public override string ToString()
	{
		return $"{GetType().Name}";
	}


}
[Serializable]
public class WhitePlayer : LocalPlayer
{
	public WhitePlayer(TEAM team, Transform ancherPoint, Board board, MultiplayerGameController controller) : base(team, ancherPoint, board, controller) { }

	public override void Update()
	{
		if (Input.GetKeyDown(KeyCode.M))
		{
			Debug.LogError($" {nameof(WhitePlayer)  } pressed M : ActivePlayer is {controller.ActivePlayer}   localPlayer is {controller.getLocalPlayer()}");
		}
	}



}

[Serializable]
public class BlackPlayer : LocalPlayer
{
	public BlackPlayer(TEAM team, Transform ancherPoint, Board board, MultiplayerGameController controller) : base(team, ancherPoint, board, controller) { }

	public override void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
		{
			Debug.LogError($" {nameof(BlackPlayer)  } pressed K : ActivePlayer is {controller.ActivePlayer}   localPlayer is {controller.getLocalPlayer()}");

		}
	}



}