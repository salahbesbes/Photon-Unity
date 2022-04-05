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



	public void setDependencies(RoomManager roomManager)
	{
		this.roomManager = roomManager;
	}
	private BaseState<PlayerStateManager> _State;

	public BaseState<PlayerStateManager> State
	{
		get => _State;
		private set
		{
			_State = value;
		}
	}

	private void Awake()
	{
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
		State.Update(this);
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
		State?.ExitState(this);
		State = newState;
		State.EnterState(this);
	}

	public void initPlayerManager(UnitPhoton unitPhoton)
	{
		this.unitPhoton = unitPhoton;
		roomManager = unitPhoton.roomManager;
		SwitchState(idelState);

	}
}
