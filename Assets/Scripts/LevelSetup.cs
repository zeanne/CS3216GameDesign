using UnityEngine;
using System.Collections;

public class LevelSetup : MonoBehaviour {

	public GameObject player;
	public GameObject machinePrefab;
	public GameObject fuelPrefab;
	public GameObject generator;
	public GameObject goal;
	public GameObject floor;
	public GameObject wallsN;
	public GameObject wallsS;
	public GameObject wallsE;
	public GameObject wallsW;

	public int height;
	public int width;
	public int level;

	float WALL_THICKNESS = 10f;
	float FLOOR_EXTRA = 30f;
	float MACHINE_DISTANCE = 150f;
	float FUEL_DISTANCE = 50f;
	float MACHINE_HEIGHT = 2f;

//	// Use this for initialization
	void Start () {

		SetupPlayingSpace ();

		player.transform.position = new Vector3 (20, 10, 0);
		float[] playerValues = GetPlayerDefaultValues (level);
		player.SendMessage("SetInitialValues", playerValues, SendMessageOptions.RequireReceiver);

		if (level != 0) {
			player.transform.position = new Vector3 (10, 10, 0);
			Instantiate (goal, new Vector3 (width - WALL_THICKNESS, Random.Range (WALL_THICKNESS, height - WALL_THICKNESS), 0), Quaternion.identity);

			GameObject newMachine;
			float i = FLOOR_EXTRA;
			while (i < width - FLOOR_EXTRA) {
				newMachine = (GameObject)Instantiate (machinePrefab, new Vector3 (i, Random.Range (8, 30), 0), Quaternion.identity);
				newMachine.SendMessage ("InitialiseMenuObjects");
				i += MACHINE_DISTANCE;
			} 

			float j = FLOOR_EXTRA + 30;
			while (j < width - FLOOR_EXTRA) {
				Instantiate (fuelPrefab, new Vector3 (j, Random.Range (8, 30), 0), Quaternion.identity);
				j += FUEL_DISTANCE;
			} 
			generator.gameObject.SendMessage ("SetInstantiateRange", new Vector4 (FLOOR_EXTRA, width - WALL_THICKNESS, WALL_THICKNESS, height - WALL_THICKNESS / 2), SendMessageOptions.DontRequireReceiver);

		} else {
			GameObject newMachine = (GameObject)Instantiate (machinePrefab, new Vector3 (150f, WALL_THICKNESS / 2 + MACHINE_HEIGHT, 0), Quaternion.identity);
			newMachine.SendMessage ("InitialiseMenuObjects");
		}
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
		// 7 - FUEL_AMOUNT_MAX,
		// 8 - CHARACTER_MOVE_SPEED_MAX

		switch(level) {
		case 0:
			return new float[] { 6f, 0.4f, 0.025f, 0.5f, 0.2f, 20f, 5f, 20f, 1f };
		case 1: 
			return new float[] { 6f, 0.4f, 0.0025f, 1.0f, 0.2f, 20f, 6f, 20f, 1f };
		case 2:
			return new float[] { 2f, 0.4f, 0.0025f, 1f, 0.2f, 10f, 5f, 20f, 2f };
		default:
			return new float[] { 2f, 0.4f, 0.0025f, 1f, 0.2f, 10f, 5f, 20f, 2f };
		}
	}

	void SetupPlayingSpace() {
		floor.transform.position = new Vector3 (width/2, height/2, 1);
		floor.transform.localScale = new Vector3 (width + FLOOR_EXTRA, height + FLOOR_EXTRA, 1);

		wallsN.transform.position = new Vector3 (width/2, height, 0);
		wallsN.transform.localScale = new Vector3 (width, WALL_THICKNESS, 1);

		wallsS.transform.position = new Vector3 (width/2, 0, 0);
		wallsS.transform.localScale = new Vector3 (2*width, WALL_THICKNESS, 1);

		wallsE.GetComponent<MeshRenderer> ().material.mainTextureScale = new Vector2 (1, height / 20);
		wallsE.transform.position = new Vector3 (width, height / 2, 0);
		wallsE.transform.localScale = new Vector3 (WALL_THICKNESS, height*2, 1);

		wallsW.GetComponent<MeshRenderer> ().material.mainTextureScale = new Vector2 (1, height / 20);
		wallsW.transform.position = new Vector3 (0, height / 2, 0);
		wallsW.transform.localScale = new Vector3 (WALL_THICKNESS, height*2, 1);

	}
}
