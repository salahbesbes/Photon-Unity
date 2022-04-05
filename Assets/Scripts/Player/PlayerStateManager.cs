using System;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
	public SelectingEnemy selectingEnemy = new SelectingEnemy();
	public Idel idelState = new Idel();
	public DoingAction doingAction = new DoingAction();
	public Dead dead = new Dead();

	public RoomManager roomManager { get; private set; }
	public UnitPhoton unitPhoton { get; private set; }
	public GameStateManager gameStateManager { get; private set; }

	private void Start()
	{

	}

	public void setDependencies(RoomManager roomManager)
	{
		this.roomManager = roomManager;
	}
	private BaseState<PlayerStateManager> _State;

	public BaseState<PlayerStateManager> CurrentState
	{
		get => _State;
		private set
		{

			Debug.Log($" value = {value}");
			_State = value;
		}
	}

	public float radius = 10;

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
				if (potentialUnit.gameStateManager.MyTeam == gameStateManager.opponentTeam)
				{
					gameStateManager.EnemyUnits.Add(potentialUnit);
				}
			}

			if (gameStateManager.MyTeam == TEAM.White)
				roomManager.whiteText.text = $"Enemy {gameStateManager.EnemyUnits.Count}";

			if (gameStateManager.MyTeam == TEAM.Black)
				roomManager.blackText.text = $"Enemy {gameStateManager.EnemyUnits.Count}";

		}
	}

	//private void Start()
	//{
	//	unitPhoton = GetComponent<UnitPhoton>();
	//	roomManager = unitPhoton.roomManager;
	//	SwitchState(idelState);
	//}
	private void Update()
	{
		//currentPos = grid.getNodeFromTransformPosition(transform);
		CurrentState.Update(this);
		//customUpdate();
	}

	public void SelectNextTarget()
	{
		throw new NotImplementedException();
	}

	public virtual void customUpdate()
	{

	}

	public void SwitchState(BaseState<PlayerStateManager> newState)
	{
		if (newState == null)
		{

			Debug.Log($"new state is null");

			return;
		}
		CurrentState?.ExitState(this);
		CurrentState = newState;
		CurrentState.EnterState(this);
	}

	public void initPlayerManager(UnitPhoton unitPhoton, GameStateManager gameStateManager)
	{
		this.unitPhoton = unitPhoton;
		roomManager = unitPhoton.roomManager;
		this.gameStateManager = gameStateManager;
		SwitchState(idelState);

	}
}
