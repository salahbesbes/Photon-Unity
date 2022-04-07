using Photon.Pun;

public class SP_PlayerStateManager : MonoBehaviourPunCallbacks
{
	public SelectingEnemy selectingEnemy = new SelectingEnemy();
	public Idel idelState = new Idel();
	public DoingAction doingAction = new DoingAction();
	public Dead dead = new Dead();

	//public AnimationType currentActionAnimation = AnimationType.idel;

	//private void Awake()
	//{
	//	//unit = GetComponent<AnyClass>();
	//	SwitchState(idelState);

	// //Debug.Log($"start of player state manager ");

	//}
	public override void OnEnable()
	{
		SwitchState(idelState);
	}

	private BaseState<SP_PlayerStateManager> _State;

	public BaseState<SP_PlayerStateManager> State
	{
		get => _State;
		private set
		{
			_State = value;
		}
	}

	public virtual void Awake()
	{
	}

	private void Update()
	{
		//currentPos = grid.getNodeFromTransformPosition(transform);
		State.Update(this);
		//customUpdate();
	}

	public virtual void customUpdate()
	{
	}

	public virtual void SwitchState(BaseState<SP_PlayerStateManager> newState)
	{
		State?.ExitState(this);
		State = newState;
		State.EnterState(this);
	}
}