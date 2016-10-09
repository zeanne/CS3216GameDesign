using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Generator : MonoBehaviour {

	public GameObject instructions;

	public float enemiesPerSecond;
	private float secondsPerEnemy;
	private float countdownToNextSpawn;

	public GameObject enemyPrefab;
 //	public GameObject groundPlane;

	private List<GameObject> inGameObjects;
	private int enemySaturation = 0;
	private int ENEMY_SATURATION_LIMIT = 4;

	private string TAG_ENEMY = "Enemy";
	private string TAG_FINISH = "Finish";
	private string TAG_FUEL = "Fuel";
	private string TAG_PLAYER = "Player";

	// Use this for initialization
	void Start () {
		secondsPerEnemy = 1f / enemiesPerSecond;
		countdownToNextSpawn = secondsPerEnemy;
		inGameObjects = new List<GameObject> ();

		inGameObjects.Add (GameObject.FindGameObjectWithTag (TAG_FINISH));
		inGameObjects.Add (GameObject.FindGameObjectWithTag (TAG_PLAYER));
		inGameObjects.AddRange (GameObject.FindGameObjectsWithTag (TAG_FUEL));

	}
	
	// Update is called once per frame
	void Update () {
		if (instructions.gameObject.activeInHierarchy) {
			return;
		}
		countdownToNextSpawn -= Time.deltaTime;

		if (countdownToNextSpawn <= 0) {
			countdownToNextSpawn = secondsPerEnemy;
			CreateNewEnemy ();
		}
	}

	void LateUpdate() {
		if (GameObject.FindGameObjectsWithTag(TAG_ENEMY).GetLength(0) < inGameObjects.Count ) {
			enemySaturation = 0;
			
		}
	}

	void CreateNewEnemy() {

		if (enemySaturation == ENEMY_SATURATION_LIMIT) {
			return;
		}

		bool intersecting = false;

		GameObject newEnemy = (GameObject) Instantiate(enemyPrefab, 
			new Vector3 (Random.Range (-20f, 20f), Random.Range (-2f, 3.5f), 0), Quaternion.identity);

		for (int i = 0; i < inGameObjects.Count; i++) {

			if (inGameObjects [i] == null) {
				continue;

			} else if (newEnemy.GetComponent<PolygonCollider2D>().bounds.Intersects(inGameObjects[i].GetComponent<PolygonCollider2D>().bounds)) {
				intersecting = true;
				break;
			}
		}
			

		if (intersecting) {
			enemySaturation++;
			Destroy (newEnemy);
			CreateNewEnemy ();

		} else {
			inGameObjects.Add (newEnemy);
			enemySaturation = 0;
		}
	}
}
