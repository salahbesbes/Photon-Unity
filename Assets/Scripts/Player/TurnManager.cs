using Photon.Realtime;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
/// <summary>
/// Pun turnBased Game manager.
/// Provides an Interface (IPunTurnManagerCallbacks) for the typical turn flow and logic, between players
/// Provides Extensions for Player, Room and RoomInfo to feature dedicated api for TurnBased Needs
/// </summary>



public interface IPunTurnManagerCallbacks
{
	/// <summary>
	/// Called the turn begins event.
	/// </summary>
	/// <param name="turn">Turn Index</param>
	void OnTurnBegins(int turn);

	/// <summary>
	/// Called when a turn is completed (finished by all players)
	/// </summary>
	/// <param name="turn">Turn Index</param>
	void OnTurnCompleted(int turn);

	/// <summary>
	/// Called when a player moved (but did not finish the turn)
	/// </summary>
	/// <param name="player">Player reference</param>
	/// <param name="turn">Turn Index</param>
	/// <param name="move">Move Object data</param>
	void OnPlayerMove(Player player, int turn, object move);

	/// <summary>
	/// When a player finishes a turn (includes the action/move of that player)
	/// </summary>
	/// <param name="player">Player reference</param>
	/// <param name="turn">Turn index</param>
	/// <param name="move">Move Object data</param>
	void OnPlayerFinished(Player player, int turn, object move);


	/// <summary>
	/// Called when a turn completes due to a time constraint (timeout for a turn)
	/// </summary>
	/// <param name="turn">Turn index</param>
	void OnTurnTimeEnds(int turn);
}


public static class TurnExtensions
{
	/// <summary>
	/// currently ongoing turn number
	/// </summary>
	public static readonly string TEAMPROPKEY = "Team";

	/// <summary>
	/// start (server) time for currently ongoing turn (used to calculate end)
	/// </summary>
	public static readonly string TURNSTARTPROPKEY = "TStart";

	/// <summary>
	/// Finished Turn of Actor (followed by number)
	/// </summary>
	public static readonly string FINISHEDTURNPROPKEY = "FToA";

	/// <summary>
	/// Sets the turn.
	/// </summary>
	/// <param name="room">Room reference</param>
	/// <param name="turn">Turn index</param>
	/// <param name="setStartTime">If set to <c>true</c> set start time.</param>
	public static void SetTurn(this Room room, TEAM turn, bool setStartTime = false)
	{
		if (room == null || room.CustomProperties == null)
		{
			Debug.Log($"set room error");
			return;
		}

		Hashtable turnProps = new Hashtable();
		turnProps[TEAMPROPKEY] = (int)turn;
		//if (setStartTime)
		//{
		//	turnProps[TURNSTARTPROPKEY] = PhotonNetwork.ServerTimestamp;
		//}

		room.SetCustomProperties(turnProps);
	}

	/// <summary>
	/// Gets the current turn from a RoomInfo
	/// </summary>
	/// <returns>The turn index </returns>
	/// <param name="room">RoomInfo reference</param>
	public static TEAM GetTurn(this RoomInfo room)
	{
		if (room == null || room.CustomProperties == null || !room.CustomProperties.ContainsKey(TEAMPROPKEY))
		{
			Debug.Log($"room props contain TEAMPROPKEY {room.CustomProperties.ContainsKey(TEAMPROPKEY)}");
			return 0;
		}

		return (TEAM)room.CustomProperties[TEAMPROPKEY];
	}


	/// <summary>
	/// Returns the start time when the turn began. This can be used to calculate how long it's going on.
	/// </summary>
	/// <returns>The turn start.</returns>
	/// <param name="room">Room.</param>
	public static int GetStartTimeTurn(this RoomInfo room)
	{
		//if (room == null || room.CustomProperties == null || !room.CustomProperties.ContainsKey(TURNSTARTPROPKEY))
		//{
		//	return 0;
		//}

		return (int)room.CustomProperties[TURNSTARTPROPKEY];
	}

	///// <summary>
	///// gets the player's finished turn (from the ROOM properties)
	///// </summary>
	///// <returns>The finished turn index</returns>
	///// <param name="player">Player reference</param>
	//public static int GetFinishedTurn(this Player player)
	//{
	//	Room room = PhotonNetwork.CurrentRoom;
	//	if (room == null || room.CustomProperties == null || !room.CustomProperties.ContainsKey(TURNPROPKEY))
	//	{
	//		return 0;
	//	}

	//	string propKey = FINISHEDTURNPROPKEY + player.ActorNumber;
	//	return (int)room.CustomProperties[propKey];
	//}

	///// <summary>
	///// Sets the player's finished turn (in the ROOM properties)
	///// </summary>
	///// <param name="player">Player Reference</param>
	///// <param name="turn">Turn Index</param>
	//public static void SetFinishedTurn(this Player player, int turn)
	//{
	//	Room room = PhotonNetwork.CurrentRoom;
	//	if (room == null || room.CustomProperties == null)
	//	{
	//		return;
	//	}

	//	string propKey = FINISHEDTURNPROPKEY + player.ActorNumber;
	//	Hashtable finishedTurnProp = new Hashtable();
	//	finishedTurnProp[propKey] = turn;

	//	room.SetCustomProperties(finishedTurnProp);
	//}
}