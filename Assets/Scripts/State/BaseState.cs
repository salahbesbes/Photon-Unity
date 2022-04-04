

using UnityEngine;

public class PlayingState : BaseState<PlayerManager>
{
	public override void EnterState(PlayerManager playerContext)
	{
		Debug.Log($"{playerContext.transform.name} enter state {GetType().Name}");
		playerContext.roomManager.switchMyState.onClick.RemoveAllListeners();
		playerContext.roomManager.switchMyState.onClick.AddListener(() => { playerContext.SwitchState(playerContext.pauseState); });
	}

	public override void ExitState(PlayerManager playerContext)
	{
		Debug.Log($"{playerContext.transform.name} EXIT state {GetType().Name}");
	}

	public override void Update(PlayerManager playerContext)
	{
		Debug.LogError($"{playerContext.transform.name} update  of {GetType().Name}");

	}
}


public class PauseState : BaseState<PlayerManager>
{
	public override void EnterState(PlayerManager playerContext)
	{
		Debug.Log($"{playerContext.transform.name} enter state {GetType().Name}");
		playerContext.roomManager.switchMyState.onClick.RemoveAllListeners();
		playerContext.roomManager.switchMyState.onClick.AddListener(() => { playerContext.SwitchState(playerContext.playingState); });
	}

	public override void ExitState(PlayerManager playerContext)
	{
		Debug.Log($"{playerContext.transform.name} EXIT state {GetType().Name}");
	}

	public override void Update(PlayerManager playerContext)
	{
		Debug.LogError($"{playerContext.transform.name} update  of {GetType().Name}");
	}
}




public abstract class BaseState<T>
{
	public string name;

	public abstract void EnterState(T playerContext);

	public abstract void Update(T playerContext);

	public abstract void ExitState(T playerContext);

	public override string ToString()
	{
		return $"{this.GetType().Name}";
	}
}