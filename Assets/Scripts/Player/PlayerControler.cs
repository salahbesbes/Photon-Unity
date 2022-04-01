using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerControler : MonoBehaviourPunCallbacks
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

	private PhotonView PV;
	public Item[] items;
	private int itemIndex, prevItemIndex = -1;

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

		if (PV.IsMine == false) enabled = false;
		else
		{
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
		groundedPlayer = controller.isGrounded;
		if (groundedPlayer && playerVelocity.y < 0)
		{
			playerVelocity.y = 0f;
		}

		Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		controller.Move(move * Time.deltaTime * playerSpeed);

		if (move != Vector3.zero)
		{
			gameObject.transform.forward = move;
		}

		// Changes the height position of the player..
		if (Input.GetButtonDown("Jump") && groundedPlayer)
		{
			playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
		}

		playerVelocity.y += gravityValue * Time.deltaTime;
		controller.Move(playerVelocity * Time.deltaTime);
	}

	public void LookAround()
	{
		transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivity);
		verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivity;
		verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
		cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
	}
}

public class PunTurnManager : MonoBehaviourPunCallbacks

{
}