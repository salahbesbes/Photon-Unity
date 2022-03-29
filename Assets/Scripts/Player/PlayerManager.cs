using Photon.Pun;
using System.IO;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	private PhotonView PM;

	private void Awake()
	{
		PM = GetComponent<PhotonView>();

		if (PM.IsMine)
		{
			CreateController();
		}
	}

	private void CreateController()
	{
		PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), Vector3.zero, Quaternion.identity).transform.parent = transform;
	}
}