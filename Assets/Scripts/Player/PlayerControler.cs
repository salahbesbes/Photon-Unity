using Photon.Pun;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
	private CharacterController controller;
	private Vector3 playerVelocity;
	private bool groundedPlayer;
	public float playerSpeed = 10f;
	private float jumpHeight = 1.0f;
	private float gravityValue = -9.81f;

	public Transform cameraHolder;
	private float verticalLookRotation;
	public float mouseSensitivity = 2;

	private PhotonView PV;

	private void Awake()
	{
		PV = GetComponent<PhotonView>();
	}

	private void Start()
	{
		controller = gameObject.AddComponent<CharacterController>();

		if (PV.IsMine == false) enabled = false;
	}

	private void Update()
	{
		LookAround();
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