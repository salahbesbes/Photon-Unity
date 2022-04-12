using UnityEngine;

public class MP_PlayerStateManager : SP_PlayerStateManager
{
	public new SelectingEnemy_MP selectingEnemy = new SelectingEnemy_MP();
	public new Idel_MP idelState = new Idel_MP();
	public new DoingAction_MP doingAction = new DoingAction_MP();
	public new Dead_MP dead = new Dead_MP();


	[Tooltip("this properties are required for UnitPhoton")]
	[SerializeField] MP_GameStateManager _gameManager;
	protected MP_GameStateManager gameStateManager { get => _gameManager; set => _gameManager = value; }

	private BaseState<MP_PlayerStateManager> _State;

	public new BaseState<MP_PlayerStateManager> State
	{
		get => _State;
		private set
		{
			_State = value;
		}
	}

	public float radius = 10;

	public UnitPhoton CurrentTarget { get; set; }



	private void OnDrawGizmos()
	{
		Gizmos.DrawSphere(transform.position, radius);
	}
	public void checkTargetInRange()
	{
		string[] collidableLayers = { "Unit" };
		int layerToCheck = LayerMask.GetMask(collidableLayers);
		Collider[] potentialEnemies = Physics.OverlapSphere(transform.position, radius, layerToCheck);
		if (potentialEnemies.Length != 0)
		{
			gameStateManager.EnemyUnits.Clear();
			foreach (Collider unit in potentialEnemies)
			{

				UnitPhoton potentialUnit = unit.GetComponent<UnitPhoton>();
				if (potentialUnit == null) continue;
				//potentialUnit.photonView.RPC("AddMeToopponentList", RpcTarget.Others, gameStateManager.EnemyUnits);
				//if (potentialUnit.gameStateManager.MyTeam == gameStateManager.opponentTeam)
				//{
				//	gameStateManager.EnemyUnits.Add(potentialUnit);
				//}
			}

			if (gameStateManager.MyTeam == TEAM.White)
				RoomManager.Instance.whiteText.text = $"Enemy {gameStateManager.EnemyUnits.Count}";

			if (gameStateManager.MyTeam == TEAM.Black)
				RoomManager.Instance.blackText.text = $"Enemy {gameStateManager.EnemyUnits.Count}";

		}
	}

	public void SelectNextUnit()
	{
	}

	private void Update()
	{
		Debug.Log($"Enemies  Count{ gameStateManager.EnemyUnits.Count }");
		State.Update(this);
	}



	public void SwitchState(BaseState<MP_PlayerStateManager> newState)
	{
		if (newState == null)
		{

			Debug.Log($"new state is null");

			return;
		}
		State?.ExitState(this);
		State = newState;
		State.EnterState(this);
	}

}
