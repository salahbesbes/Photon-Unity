using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class UnitPhoton : MonoBehaviourPunCallbacks
{
	private CharacterController controller;
	private Vector3 playerVelocity;
	private bool groundedPlayer;
	public float playerSpeed = 10f;
	public float jumpHeight = 5f;
	private float gravityValue = -9.81f;

	public Transform cameraHolder;
	private float verticalLookRotation;
	public float mouseSensitivity = 2;

	public PhotonView PV { get; private set; }
	public Item[] items;
	private int itemIndex, prevItemIndex = -1;

	public float speed = 10.0F;
	public float rotateSpeed = 5.0F;


	[SerializeField] public GameStateManager gameStateManager { get; private set; }
	[SerializeField] public RoomManager roomManager { get; private set; }
	[SerializeField] public PlayerStateManager playerStateManager { get; private set; }
	public Transform parent;



	public void setDependencies(GameStateManager manager, Transform parent)
	{
		this.gameStateManager = manager;
		this.parent = parent;
	}
	public void equipeItem(int _index)
	{
		if (_index == prevItemIndex)
			return;
		itemIndex = _index;
		items[itemIndex].show();

		if (prevItemIndex != -1)
			items[prevItemIndex].hide();

		prevItemIndex = itemIndex;

		if (PV.IsMine)
		{
			Hashtable hash = new Hashtable();
			hash.Add("itemIndex", itemIndex);
			PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
		}
	}

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
	{
		if (!PV.IsMine && targetPlayer == PV.Owner)
		{
			equipeItem((int)changedProps["itemIndex"]);
		}
	}

	private void Awake()
	{
		PV = GetComponent<PhotonView>();
	}

	private void Start()
	{
		controller = gameObject.AddComponent<CharacterController>();


		if (PV.IsMine == false)
		{
			playerStateManager = GetComponent<PlayerStateManager>();
			//gameStateManager = GetComponentInParent<GameStateManager>();
			//roomManager = gameStateManager.roomManager;
			//playerStateManager.initPlayerManager(this, gameStateManager);

			playerStateManager.enabled = false;
			enabled = false;
		}
		else
		{
			playerStateManager = GetComponent<PlayerStateManager>();
			gameStateManager = GetComponentInParent<GameStateManager>();
			roomManager = gameStateManager.roomManager;
			playerStateManager.initPlayerManager(this, gameStateManager);
			equipeItem(0);
		}
	}

	private void Update()
	{
		LookAround();

		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			equipeItem(0);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			equipeItem(1);
		}
	}

	private void FixedUpdate()
	{

		CharacterController controller = GetComponent<CharacterController>();

		// Rotate around y - axis
		transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);

		// Move forward / backward
		Vector3 forward = transform.TransformDirection(Vector3.forward);
		float curSpeed = speed * Input.GetAxis("Vertical");
		controller.SimpleMove(forward * curSpeed);
	}

	public void LookAround()
	{
		transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivity);
		verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivity;
		verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
		cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
	}

	//public void initUnit(GameStateManager gameStateManager, RoomManager roomManager)
	//{
	//	this.gameStateManager = gameStateManager;
	//	this.roomManager = roomManager;
	//	playerStateManager = GetComponent<PlayerStateManager>();
	//	playerStateManager.initPlayerManager(this);
	//}
}

