using TMPro;
using UnityEngine;

public class Idel : BaseState<PlayerStateManager>
{
	public override void EnterState(PlayerStateManager playerContext)
	{
		Debug.LogError($"{playerContext.transform.name} enter state {playerContext.CurrentState}");
		playerContext.roomManager.switchPlayerState.onClick.RemoveAllListeners();
		playerContext.roomManager.switchPlayerState.onClick.AddListener(() => { playerContext.SwitchState(playerContext.selectingEnemy); });
		playerContext.roomManager.switchPlayerState.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{GetType().Name}";

	}

	public override void ExitState(PlayerStateManager playerContext)
	{
		//Debug.Log($"{playerContext.transform.name} Exit state {GetType().Name}");

	}

	public override void Update(PlayerStateManager playerContext)
	{

		playerContext.checkTargetInRange();
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			playerContext.SelectNextTarget();
		}

	}
}