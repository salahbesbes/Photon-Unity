using TMPro;
using UnityEngine;

public class SelectingEnemy : BaseState<SP_PlayerStateManager>
{
	public override void EnterState(SP_PlayerStateManager playerContext)
	{
		//Debug.LogError($"{playerContext.transform.name} enter state {playerContext.State}");
		RoomManager.Instance.switchPlayerState.onClick.RemoveAllListeners();
		RoomManager.Instance.switchPlayerState.onClick.AddListener(() => { playerContext.SwitchState(playerContext.doingAction); });
		RoomManager.Instance.switchPlayerState.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{GetType().Name}";

	}

	public override void ExitState(SP_PlayerStateManager playerContext)
	{
		//Debug.Log($"{playerContext.transform.name} Exit state {GetType().Name}");

	}

	public override void Update(SP_PlayerStateManager playerContext)
	{
		NodeGrid.Instance.resetGrid();
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			//playerContext.SelectNextTarget();
		}

	}
}