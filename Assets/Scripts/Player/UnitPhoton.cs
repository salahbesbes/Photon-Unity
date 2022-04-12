using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class UnitPhoton : MP_PlayerStateManager
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

	public Item[] items;
	private int itemIndex, prevItemIndex = -1;

	public float speed = 10.0F;
	public float rotateSpeed = 5.0F;


	public override void Awake()
	{

		controller = gameObject.AddComponent<CharacterController>();
		// get data passed when PhotonNetwork.Instantiate is called in GameStateManager
		gameStateManager = PhotonView.Find((int)photonView.InstantiationData[0]).GetComponent<MP_GameStateManager>();
		SwitchState(idelState);

	}

	private void Start()
	{



		if (photonView.IsMine == false)
		{
			GameObject.FindWithTag("opponnetTeam").transform.position = gameStateManager.transform.position;
			gameObject.transform.parent = GameObject.FindWithTag("opponnetTeam").transform;
			enabled = false;
		}
		else
		{
			transform.SetParent(gameStateManager.transform);
			equipeItem(0);


			if (gameStateManager.SelectedUnit == this)
				enabled = true;
		}
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

		if (photonView.IsMine)
		{
			Hashtable hash = new Hashtable();
			hash.Add("itemIndex", itemIndex);
			PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
		}
	}

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
	{
		if (!photonView.IsMine && targetPlayer == photonView.Owner)
		{
			equipeItem((int)changedProps["itemIndex"]);
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


	public override string ToString()
	{
		return $"{transform.name} with id {photonView.ViewID}";
	}
}

