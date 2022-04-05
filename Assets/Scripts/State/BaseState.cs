

using TMPro;
using UnityEngine;

public class PlayingState : BaseState<GameStateManager>
{
	public override void EnterState(GameStateManager gameContext)
	{
		Debug.Log($"{gameContext.transform.name} enter state {GetType().Name}");
		gameContext.roomManager.switchGameState.onClick.RemoveAllListeners();
		gameContext.roomManager.switchGameState.onClick.AddListener(() => { gameContext.SwitchState(gameContext.pauseState); });
		gameContext.roomManager.switchGameState.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{GetType().Name}";

	}

	public override void ExitState(GameStateManager gameContext)
	{
		Debug.Log($"{gameContext.transform.name} EXIT state {GetType().Name}");
	}

	public override void Update(GameStateManager gameContext)
	{
		//Debug.LogError($"{gameContext.transform.name} update  of {GetType().Name}");

	}
}


public class PauseState : BaseState<GameStateManager>
{
	public override void EnterState(GameStateManager gameContext)
	{
		Debug.Log($"{gameContext.transform.name} enter state {GetType().Name}");
		gameContext.roomManager.switchGameState.onClick.RemoveAllListeners();
		gameContext.roomManager.switchGameState.onClick.AddListener(() => { gameContext.SwitchState(gameContext.playingState); });
		gameContext.roomManager.switchGameState.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{GetType().Name}";


	}

	public override void ExitState(GameStateManager gameContext)
	{
		Debug.Log($"{gameContext.transform.name} EXIT state {GetType().Name}");
	}

	public override void Update(GameStateManager gameContext)
	{
		//Debug.LogError($"{gameContext.transform.name} update  of {GetType().Name}");
	}
}




public abstract class BaseState<T>
{
	public string name;

	public abstract void EnterState(T Context);

	public abstract void Update(T Context);

	public abstract void ExitState(T Context);

	public override string ToString()
	{
		return $"{this.GetType().Name}";
	}
}