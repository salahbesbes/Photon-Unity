using Photon.Pun;
using UnityEngine;

public class Selected : MonoBehaviour
{

	PhotonView PV;
	private void Awake()
	{
		PV = GetComponent<PhotonView>();
	}
	private Color[] colors = new Color[5] { Color.black, Color.blue, Color.red, Color.cyan, Color.green };
	private int currentIndex;

	private void OnMouseDown()
	{
		if (PV.IsMine == false) return;
		PV.RPC(nameof(RPC_updateColor), RpcTarget.All);
	}
	[PunRPC]
	public void RPC_updateColor()
	{
		transform.GetComponent<Renderer>().material.color = colors[(++currentIndex) % 5];
	}




}