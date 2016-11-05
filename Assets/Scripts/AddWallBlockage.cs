using UnityEngine;
using System.Collections;

public class AddWallBlockage : MonoBehaviour {

	public GameObject player;
	public GameObject wall1;
	public GameObject wall2;

	GameObject[] walls = new GameObject[2];

	float camSize = 20;
	float positionX = -10;
	float crossX = 20;

	int which = 0;

	// Use this for initialization
	void Start () {
		walls [0] = wall1;
		walls [1] = wall2;
	}
	
	// Update is called once per frame
	void Update () {
		if (player.transform.position.x > crossX) {
			crossX += camSize;
			positionX += camSize;
			Vector3 p = walls[which].transform.position;
			p.x = positionX;
			walls[which].transform.position = p;

			which = (which + 1) % 2;
		}
	}
}
