using UnityEngine;

public class SetParent : MonoBehaviour
{

	PlayerControler unit;

	private void Start()
	{

		unit = GetComponent<PlayerControler>();
		if (unit.photonView.IsMine)
		{
			gameObject.transform.parent = unit.parent;

		}
		else
		{
			gameObject.transform.parent = GameObject.FindWithTag("opponnetTeam").transform;

		}
	}


}