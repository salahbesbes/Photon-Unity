using TMPro;
using UnityEngine;

public class DoingAction : BaseState<PlayerStateManager>
{
	public override void EnterState(PlayerStateManager playerContext)
	{
		Debug.Log($"{playerContext.transform.name} enter state {GetType().Name}");
		RoomManager.Instance.switchPlayerState.onClick.RemoveAllListeners();
		RoomManager.Instance.switchPlayerState.onClick.AddListener(() => { playerContext.SwitchState(playerContext.dead); });
		RoomManager.Instance.switchPlayerState.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{GetType().Name}";

	}

	public override void ExitState(PlayerStateManager playerContext)
	{
		Debug.Log($"{playerContext.transform.name} Exit state {GetType().Name}");

	}

	public override void Update(PlayerStateManager playerContext)
	{
		Debug.Log($"{playerContext.transform.name} update state {GetType().Name}");

	}
}
