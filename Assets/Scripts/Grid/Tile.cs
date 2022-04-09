using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
	public Transform obj;
	public Node node;
	public Cover leftCover;
	public Cover rightCover;
	public Cover forwardCover;
	public Cover backCover;
	public List<Cover> listOfActiveCover;
	public bool mouseOnTile = false;
	private Transform parent;
	[HideInInspector]
	public float size = 1;
	[HideInInspector]
	public float offset = 2f;


	public GameObject getPrefabOnTopOfTheNode(Node node)
	{
		RaycastHit hit;


		if (Physics.Raycast(node.coord, Vector3.up, out hit))
		{

			//Debug.Log($"{hit.collider.name}");
			hit.collider.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.blue);
		}
		return hit.transform.gameObject;
	}

	//public Tile(Node node, Transform parent, List<Tile> listTiles, float quadsize)
	//{
	//	// create Quad
	//	size = quadsize;
	//	this.parent = parent;
	//	this.node = node;
	//	node.groundTile = GameObject
	//	obj = getPrefabOnTopOfTheTile();
	//	listTiles.Add(this);
	//	node.tile = this;
	//}
	private void Awake()
	{
		obj = getPrefabOnTopOfTheTile();
		transform.localScale = new Vector3(size, size, size);
		listOfActiveCover = new List<Cover>();

	}
	public Transform getPrefabOnTopOfTheTile()
	{

		string[] collidableLayers = { "Unwalkable", "Unit" };
		int layerToCheck = LayerMask.GetMask(collidableLayers);
		float GroundTileheight = node.groundTile.transform.position.y;
		Collider[] objs = Physics.OverlapSphere(node.coord + Vector3.up * GroundTileheight, NodeGrid.Instance.nodeSize / 2, layerToCheck);
		if (objs.Length != 0)
		{
			for (int i = 0; i < objs.Length; i++)
			{
				//Debug.Log($"{objs[i].name}");
			}
			// TODO: if 2 GameObject share same tile this line can cause bugs
			return objs[0].transform;
		}

		return null;
	}
	public void createRightCover()
	{
		Vector3 origin = obj.transform.position + Vector3.up * size;
		GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
		quad.transform.position = origin + Vector3.right * size;
		quad.transform.rotation = Quaternion.Euler(0, 90, 0);
		quad.transform.SetParent(parent);
		quad.transform.localScale = new Vector3(size, 2 * size, 1);

		quad.GetComponent<Renderer>().material = (Material)Resources.Load("tile", typeof(Material));
		quad.GetComponent<Renderer>().material.color = Color.red;
		rightCover = new Cover(quad);
		listOfActiveCover.Add(rightCover);
		//Debug.Log($"create right Cover");
	}

	public void createLeftCover()
	{
		Vector3 origin = obj.transform.position + Vector3.up * size;
		GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
		quad.transform.position = origin + Vector3.left * size;
		quad.transform.rotation = Quaternion.Euler(0, -90, 0);
		quad.transform.SetParent(parent);
		quad.transform.localScale = new Vector3(size, 2 * size, 1);

		quad.GetComponent<Renderer>().material = (Material)Resources.Load("tile", typeof(Material));
		quad.GetComponent<Renderer>().material.color = Color.green;
		leftCover = new Cover(quad);
		listOfActiveCover.Add(leftCover);
		//Debug.Log($"create left Cover");
	}

	public void createForwardCover()
	{
		Vector3 origin = obj.transform.position + Vector3.up * size;
		GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
		quad.transform.position = origin + Vector3.forward * size;
		quad.transform.rotation = Quaternion.Euler(0, 0, 0);
		quad.transform.SetParent(parent);
		quad.transform.localScale = new Vector3(size, 2 * size, 1);

		quad.GetComponent<Renderer>().material = (Material)Resources.Load("tile", typeof(Material));
		quad.GetComponent<Renderer>().material.color = Color.gray;
		forwardCover = new Cover(quad);
		listOfActiveCover.Add(forwardCover);
		//Debug.Log($"create forward Cover");
	}

	public void createBackCover()
	{
		Vector3 origin = obj.transform.position + Vector3.up * size;
		GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
		quad.transform.position = origin + Vector3.back * size / 2;
		quad.transform.rotation = Quaternion.Euler(180, 0, 0);
		quad.transform.SetParent(parent);
		quad.transform.localScale = new Vector3(size, 2 * size, 1);

		quad.GetComponent<Renderer>().material = (Material)Resources.Load("tile", typeof(Material));
		quad.GetComponent<Renderer>().material.color = Color.yellow;
		backCover = new Cover(quad);
		listOfActiveCover.Add(backCover);
		//Debug.Log($"create Back Cover");
	}

	public void destroyAllActiveCover()
	{
		if (listOfActiveCover.Count == 0) return;
		foreach (Cover cover in listOfActiveCover)
		{
			GameObject.Destroy(cover.coverObj);
		}
		//Debug.Log($"destroy");
		listOfActiveCover.Clear();
	}
}

public class Cover
{
	private float _value = 0;
	public GameObject coverObj;

	public float Value
	{ get { return _value; } set { _value = value; } }

	public Cover(GameObject cover, float coverValue = 45f)
	{
		coverObj = cover;
		Value = coverValue;
	}

	public override string ToString()
	{
		return $"cover exist";
	}
}