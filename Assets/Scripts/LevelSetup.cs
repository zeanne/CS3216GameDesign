using UnityEngine;
using System.Collections;

public class LevelSetup : MonoBehaviour {

	public GameObject player;
	public GameObject machinePrefab;
	public GameObject generator;
	public GameObject goal;
	public GameObject floor;
	public GameObject wallsN;
	public GameObject wallsS;
	public GameObject wallsE;
	public GameObject wallsW;

	public int height;
	public int width;

	float WALL_THICKNESS = 10f;
	float FLOOR_EXTRA = 30f;
	float MACHINE_DISTANCE = 50f;
	float MACHINE_HEIGHT = 2f;
	float GOAL_HEIGHT = 2f;
//	float GOAL_WIDTH = 2f;



//	// Use this for initialization
	void Start () {

		floor.transform.position = new Vector3 (width/2, height/2, 1);
		floor.transform.localScale = new Vector3 (width + FLOOR_EXTRA, height + FLOOR_EXTRA, 1);

		wallsW.GetComponent<MeshRenderer> ().material.mainTextureScale = new Vector2 (width, WALL_THICKNESS);
		wallsN.transform.position = new Vector3 (width/2, height, 0);
		wallsN.transform.localScale = new Vector3 (width, WALL_THICKNESS, 1);

		wallsW.GetComponent<MeshRenderer> ().material.mainTextureScale = new Vector2 (width, WALL_THICKNESS);
		wallsS.transform.position = new Vector3 (width/2, 0, 1);
		wallsS.transform.localScale = new Vector3 (width, -WALL_THICKNESS, 1);

		wallsE.GetComponent<MeshRenderer> ().material.mainTextureScale = new Vector2 (WALL_THICKNESS, height);
		wallsE.transform.position = new Vector3 (width, height / 2, 0);
		wallsE.transform.localScale = new Vector3 (WALL_THICKNESS/2, height, 1);

		wallsW.GetComponent<MeshRenderer> ().material.mainTextureScale = new Vector2 (WALL_THICKNESS, height);
		wallsW.transform.position = new Vector3 (0, height / 2, 0);
		wallsW.transform.localScale = new Vector3 (WALL_THICKNESS/2, height, 1);

		float[] playerValues = GetPlayerDefaultValues (1);
		player.transform.position = new Vector3 (10, 10, 0);
		player.SendMessage("SetInitialValues", playerValues, SendMessageOptions.RequireReceiver);

		Instantiate(goal, new Vector3(width - WALL_THICKNESS, Random.Range(WALL_THICKNESS, height-WALL_THICKNESS), 0), Quaternion.identity);

		GameObject newMachine;
		float i = FLOOR_EXTRA;
		while (i < width - FLOOR_EXTRA) {
			newMachine = (GameObject) Instantiate (machinePrefab, new Vector3 (i, WALL_THICKNESS/2 + MACHINE_HEIGHT, 0), Quaternion.identity);
			newMachine.SendMessage ("InitialiseMenuObjects");
			i += MACHINE_DISTANCE;
		} 

		generator.gameObject.SendMessage ("SetInstantiateRange", new Vector4(FLOOR_EXTRA, width - WALL_THICKNESS, WALL_THICKNESS, height - WALL_THICKNESS/2), SendMessageOptions.DontRequireReceiver);
	}

	float[] GetPlayerDefaultValues(int level) {

		// VALUES: 
		// 0 - CHARACTER_ATTACK_RATE_INITIAL, 
		// 1 - CHARACTER_MOVE_SPEED_INITIAL, 
		// 2 - CHARACTER_MOVE_SPEED_BOOST, 
		// 3 - FUEL_AMOUNT_DEPLETION_MOVING,
		// 4 - FUEL_AMOUNT_DEPLETION_STATIONARY, 
		// 5 - FUEL_AMOUNT_INITIAL,
		// 6 - FUEL_AMOUNT_REPLENISH,
		// 7 - FUEL_AMOUNT_MAX

		switch(level) {
		case 1: 
			return new float[] { 2f, 0.4f, 0.05f, 1f, 0.2f, 20f, 5f, 20f };
		case 2:
			return new float[] { 2f, 0.4f, 0.05f, 1f, 0.2f, 10f, 5f, 20f };
		default:
			return new float[] { 2f, 0.4f, 0.05f, 1f, 0.2f, 10f, 5f, 20f };
		}
	}
}
