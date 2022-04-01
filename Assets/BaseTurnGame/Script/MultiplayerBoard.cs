using System.IO;

public class MultiplayerBoard : Board
{
	public string path = Path.Combine("Prefab", "nameof(MultiplayerBoard)");
	private MultiplayerGameController multiplayerController;
	internal void SetDependencies(MultiplayerGameController controller)
	{
		multiplayerController = controller;
	}
}
