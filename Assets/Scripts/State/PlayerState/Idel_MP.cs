using TMPro;
using UnityEngine;

public class Idel_MP : BaseState<MP_PlayerStateManager>
{
	public override void EnterState(MP_PlayerStateManager playerContext)
	{
		//Debug.LogError($"{playerContext.transform.name} enter state {playerContext.CurrentState}");
		RoomManager.Instance.switchPlayerState.onClick.RemoveAllListeners();
		RoomManager.Instance.switchPlayerState.onClick.AddListener(() => { playerContext.SwitchState(playerContext.selectingEnemy); });
		RoomManager.Instance.switchPlayerState.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{GetType().Name}";

	}

	public override void ExitState(MP_PlayerStateManager playerContext)
	{
		//Debug.Log($"{playerContext.transform.name} Exit state {GetType().Name}");

	}

	public override void Update(MP_PlayerStateManager playerContext)
	{
		Debug.Log($"{playerContext.State}");
		playerContext.checkTargetInRange();
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			//playerContext.SelectNextTarget();
		}

	}
}


public class Idel : BaseState<SP_PlayerStateManager>
{
	public override void EnterState(SP_PlayerStateManager playerContext)
	{

	}

	public override void ExitState(SP_PlayerStateManager playerContext)
	{
		//Debug.Log($"{playerContext.transform.name} Exit state {GetType().Name}");

	}

	public override void Update(SP_PlayerStateManager playerContext)
	{

	}
}