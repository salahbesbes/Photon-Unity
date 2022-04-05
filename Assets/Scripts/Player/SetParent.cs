using UnityEngine;

public class SetParent : MonoBehaviour
{

	UnitPhoton unit;
	GameStateManager gameStateManager;

	private void Start()
	{

		unit = GetComponent<UnitPhoton>();
		gameStateManager = GetComponent<GameStateManager>();
		if (unit != null)
		{
			if (unit.photonView.IsMine)
			{

				gameObject.transform.parent = unit.parent;
				return;
			}
			else
			{
				gameObject.transform.parent = GameObject.FindWithTag("opponnetTeam").transform.GetChild(0);
				return;
			}

		}
		else if (gameStateManager != null)
		{
			if (gameStateManager.photonView.IsMine)
			{

				gameObject.transform.parent = gameStateManager.parent;
				return;
			}

			else
			{
				gameObject.transform.parent = GameObject.FindWithTag("opponnetTeam").transform;
				return;

			}
		}
	}


}