using UnityEngine;

public class Item : MonoBehaviour
{
	public GunInfo info;
	public GameObject prefab;

	public void hide()
	{
		transform.GetChild(0).gameObject.SetActive(false);
	}

	public void show()
	{
		transform.GetChild(0).gameObject.SetActive(true);
	}
}