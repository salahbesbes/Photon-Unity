using UnityEngine;

public class SetParent : MonoBehaviour
{

	UnitPhoton unit;

	private void Start()
	{

		unit = GetComponent<UnitPhoton>();
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