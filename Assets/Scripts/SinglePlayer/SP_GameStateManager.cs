using gameEventNameSpace;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class SP_GameStateManager : MonoBehaviourPunCallbacks
{
	// Current state of the player, this script is attached to the object of interest the player
	// object is accessible in this class and other children

	// this is the Subject and it have some Observers

	// initialise 4 stat of the Player 4 instatnce that lives in this class
	public PlayerTurn playerTurn = new PlayerTurn();

	public EnemyTurn enemyTurn = new EnemyTurn();

	[SerializeField]
	private BaseState<SP_GameStateManager> _State;

	public BaseState<SP_GameStateManager> State
	{
		get => _State;
		set
		{
			_State?.ExitState(this);
			_State = value;
			//StateEventSubject.Raise(_State);
		}
	}

	[SerializeField] protected List<UnitPhoton> _units;

	public List<UnitPhoton> Units
	{
		get => _units;
		set => _units = value;

	}


	[SerializeField] protected List<UnitPhoton> _enemyUnits;
	public List<UnitPhoton> EnemyUnits
	{
		get => _enemyUnits;
		set => _enemyUnits = value;
	}

	public BaseStateEvent StateEventSubject;
	public VoidEvent PlayerChangeEvent;

	private SP_PlayerStateManager _selectedUnit;

	public SP_PlayerStateManager SelectedUnit
	{
		get => _selectedUnit; set
		{
			//every time game manager want to switch player update the old selected one
			// to idel state
			_selectedUnit?.SwitchState(_selectedUnit?.idelState);
			//clearPreviousSelectedUnitFromAllVoidEvents(_selectedUnit);

			//clearPreviousSelectedUnitFromAllWeaponEvent(_selectedUnit);
			//clearPreviousSelectedUnitFromAlEquipementEvent(_selectedUnit);

			_selectedUnit = value;

			//Debug.Log($"Selected  {SelectedUnit} ");
			//PlayerChangeEvent.Raise();

			//MakeGAmeMAnagerListingToNewSelectedUnit(_selectedUnit);

			//MakeOnlySelectedUnitListingToEventArgument(_selectedUnit, PlayerChangeEvent);

			//MakeOnlySelectedUnitListingToEquipeEvent(_selectedUnit, _selectedUnit?.GetComponent<Stats>()?.unit?.EquipeEvent);
		}
	}

	//public Player SelectedPlayer
	//{
	//	get => _selectedPlayer; set
	//	{
	//		// every time game manager want to switch player update the old selected one
	//		// to idel state
	//		_selectedPlayer?.SwitchState(_selectedPlayer?.idelState);
	//		clearPreviousSelectedUnitFromAllVoidEvents(_selectedPlayer);
	//		clearPreviousSelectedUnitFromAllWeaponEvent(_selectedPlayer);
	//		clearPreviousSelectedUnitFromAlEquipementEvent(_selectedPlayer);

	//		_selectedPlayer = value;
	//		if (State is PlayerTurn)
	//		{
	//			MakeGAmeMAnagerListingToNewSelectedUnit(_selectedPlayer);
	//			MakeOnlySelectedUnitListingToEventArgument(_selectedPlayer, PlayerChangeEvent);
	//			MakeOnlySelectedUnitListingToEquipeEvent(_selectedPlayer, _selectedPlayer.GetComponent<Stats>()?.unit?.EquipeEvent);
	//		}
	//		else if (State is EnemyTurn)
	//		{
	//			MakeOnlySelectedUnitListingToWeaponEvent(_selectedPlayer, SelectedEnemy?.GetComponent<Stats>()?.unit?.ShootActionEvent);
	//			MakeOnlySelectedUnitListingToEventArgument(_selectedPlayer, SelectedEnemy?.onChangeTarget);
	//		}
	//	}
	//}

	[HideInInspector]
	//public NodeGrid grid;

	public static SP_GameStateManager Instance;

	public virtual void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			SwitchState(playerTurn);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public virtual void Start()
	{
		//grid = NodeGrid.Instance;
	}

	private void Update()
	{
		// for any state the player is in, we execute the update methode of that State
		// change of the state is instant since this update executs every frame
		State?.Update(this);
	}

	public virtual void SwitchState(BaseState<SP_GameStateManager> newState)
	{
		// change the current state and execute the start methode of that new State this is
		// the only way to change the state
		State?.ExitState(this);
		State = newState;
		//clearPreviousSelectedUnitFromAllWeaponEvent(SelectedUnit?.CurrentTarget);
		State.EnterState(this);
	}

	public virtual void ChangeState()
	{
		if (State == playerTurn) SwitchState(enemyTurn);
		else if (State == enemyTurn) SwitchState(playerTurn);
	}
}
