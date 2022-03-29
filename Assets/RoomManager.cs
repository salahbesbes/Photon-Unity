using Photon.Pun;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
	public static RoomManager Instance;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	public override void OnEnable()
	{
		base.OnEnable();
		// since this is a singleton class this callback is called when ever any scene
		// changes
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		Debug.Log($"{scene.name} is loaded");
		// check if we load the scene that have build index 1
		if (scene.buildIndex == 1)
		{
			if (PhotonNetwork.InRoom == false)
			{
				SceneManager.LoadScene(0);
			}
			else
				PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
		}
	}

	public override void OnDisable()
	{
		base.OnDisable();
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
}