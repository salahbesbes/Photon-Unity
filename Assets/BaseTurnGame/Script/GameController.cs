using UnityEngine;

public class GameController : MonoBehaviour
{
	public LocalPlayer ActivePlayer;
	protected BlackPlayer blackPlayer;
	protected WhitePlayer whitePlayer;






	public void InitializeGame(Transform WhiteAncker, Board whiteBoard, Transform BlackAncker, Board blackBoard, MultiplayerGameController controller)
	{
		CreatePlayers(WhiteAncker, whiteBoard, BlackAncker, blackBoard, controller);
	}


	private void CreatePlayers(Transform WhiteAncker, Board whiteBoard, Transform BlackAncker, Board blackBoard, MultiplayerGameController controller)
	{
		whitePlayer = new WhitePlayer(TEAM.White, WhiteAncker, whiteBoard, controller);
		blackPlayer = new BlackPlayer(TEAM.Black, BlackAncker, blackBoard, controller);
	}
}