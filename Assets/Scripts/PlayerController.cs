
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

//	public GameObject instructions;
	public GameObject machineMenuCanvas;
	public GameObject pollutionColour;
	public Slider fuelBar;

	private Rigidbody2D rb2d;
	public GameObject gameOverCanvas;

	private string TAG_ENEMY = "Enemy";
	private string TAG_FINISH = "Finish";
	private string TAG_FUEL = "Fuel";
	private string TAG_MACHINE = "Machine";

	private static float CHARACTER_ATTACK_RATE_INITIAL;
	private static float CHARACTER_MOVE_SPEED_INITIAL;
	private static float CHARACTER_MOVE_SPEED_BOOST;
	private static float CHARACTER_MOVE_SPEED_MAX;
	private static float FUEL_AMOUNT_DEPLETION_MOVING;
	private static float FUEL_AMOUNT_DEPLETION_STATIONARY;
	private static float FUEL_AMOUNT_INITIAL;
	private static float FUEL_AMOUNT_MAX;
	private static float FUEL_AMOUNT_REPLENISH;

	public static float highScore;
	public int level;

	public float currentFuelAmount;
	public float currentMoveSpeed;
	public float currentAttackRate;

	private bool playerStartsGame;
	private bool gameEnded;
	private int enemyCount;

	private float opacitySupposed = 0f;
	private float opacityCurrent = 0f;
	private float rotationZ = 0f;

	void Start() {
		Time.timeScale = 0;

		rb2d = GetComponent<Rigidbody2D> ();
		SetUpVariables ();

	}

	void SetUpVariables() {
		currentFuelAmount = FUEL_AMOUNT_MAX;
		currentMoveSpeed = CHARACTER_MOVE_SPEED_INITIAL;
		currentAttackRate = CHARACTER_ATTACK_RATE_INITIAL;

		gameEnded = false;
		playerStartsGame = false;
		machineMenuCanvas.gameObject.SetActive (false);
		gameOverCanvas.SetActive (false);
	}

	void Update() {


//		if (instructions.gameObject.activeInHierarchy && Input.anyKeyDown) {
//			instructions.gameObject.SetActive (false);
//		}
		if (!playerStartsGame) {
			PlayerStartedMoving ();
			return;
		}

		if (opacityCurrent < opacitySupposed) {
			opacityCurrent += 0.01f;

		} else if (opacityCurrent > opacitySupposed) {
			opacityCurrent -= 0.01f;
		}

		Color tempColor = pollutionColour.GetComponent<SpriteRenderer> ().color;
		tempColor.a = opacityCurrent;
		pollutionColour.GetComponent<SpriteRenderer> ().color = tempColor;


		if (gameEnded && level == 0) {
			if (Input.GetKeyDown(KeyCode.Space)) {
				SceneManager.LoadScene ("ReachTheGoal");

			} else if (Input.GetKeyDown(KeyCode.Return)) {
				SceneManager.LoadScene (SceneManager.GetActiveScene().name);
			}
		} else if (gameEnded && level == 1) {
			if (Input.GetKeyDown(KeyCode.Space)) {
				SceneManager.LoadScene (SceneManager.GetActiveScene().name);

			} else if (Input.GetKeyDown(KeyCode.Return)) {
				SceneManager.LoadScene ("Start");
			}
		}
	}

	void LateUpdate() {
		SetFuelBar ();
	}

	void FixedUpdate() {
//		if (instructions.gameObject.activeInHierarchy) {
//			return;
//		}
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 oldPosition = transform.position;
		Vector3 moveDistance = new Vector3 (moveHorizontal * currentMoveSpeed, moveVertical * currentMoveSpeed);
		Vector3 newPosition = (oldPosition + moveDistance);

		rb2d.transform.position = Vector3.Lerp (oldPosition, newPosition, Time.time);

		if (moveDistance.magnitude != 0) {
			currentFuelAmount -= Time.deltaTime * FUEL_AMOUNT_DEPLETION_MOVING;
		} else {
			currentFuelAmount -= Time.deltaTime * FUEL_AMOUNT_DEPLETION_STATIONARY;
		}

		Vector3 oldRotation = rb2d.transform.eulerAngles;
		Vector3 newRotation = rb2d.transform.eulerAngles;

		if (Input.GetKey(KeyCode.RightArrow)) {
			if (rotationZ == 0) {
				rotationZ = 355;
				oldRotation.z = 360;
				newRotation.z = rotationZ;

			} else if (rotationZ < 180) {
				newRotation.z = Mathf.Max(rotationZ - 5f, 0);
				rotationZ = newRotation.z;

			} else {
				newRotation.z = Mathf.Max (rotationZ - 5f, 330f);
				rotationZ = newRotation.z;
			}

		} else if (Input.GetKey(KeyCode.LeftArrow)) {
			if (rotationZ < 180) {
				newRotation.z = Mathf.Min (30f, rotationZ + 5f);
				rotationZ = newRotation.z;

			} else {
				newRotation.z = Mathf.Min (360, rotationZ + 5);
				rotationZ = Mathf.Repeat (newRotation.z, 360);
			}

		} else {
			if (rotationZ == 0) {
				return;
			}

			if (rotationZ < 180) {
				newRotation.z = rotationZ - 5f;
			} else {
				newRotation.z = rotationZ + 5f;
			}
			rotationZ = Mathf.Repeat(newRotation.z, 360);
		}

		rb2d.transform.eulerAngles = Vector3.Lerp (oldRotation, newRotation, Time.time);
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.CompareTag (TAG_FINISH)) {
			WinGame ();
			return;
		}

		if (other.gameObject.CompareTag (TAG_FUEL)) {
			other.gameObject.SendMessage ("TakeFuel", gameObject, SendMessageOptions.DontRequireReceiver);
		}
	}

	void OnCollisionStay2D(Collision2D other) {
		if (other.gameObject.CompareTag (TAG_ENEMY)) {
			other.gameObject.SendMessage ("TakeDamage", currentAttackRate, SendMessageOptions.DontRequireReceiver);
		}
	}

	private void SetFuelBar() {
		fuelBar.value = currentFuelAmount / FUEL_AMOUNT_MAX;
		if (currentFuelAmount <= 0) {
			LoseGame ();
		}
	}

	private void SetGameEndStatus() {
		gameOverCanvas.SetActive (true);
		gameEnded = true;
		PauseGame ();
	}

	void LoseGame() {
//		resultText.text = "You lose!\nPress 'r' for replay.";
		SetGameEndStatus ();
	}

	void WinGame() {
//		resultText.text = "You win!\nPress 'r' for replay.";
		SetGameEndStatus ();

	}

	// Machine Boost
	void IncreaseMoveSpeed() {
		currentMoveSpeed += CHARACTER_MOVE_SPEED_BOOST;
		currentMoveSpeed = Mathf.Min (CHARACTER_MOVE_SPEED_MAX, currentMoveSpeed);
	}

	// Machine Boost
	void ReplenishFuel() {
		currentFuelAmount += FUEL_AMOUNT_REPLENISH;
		currentFuelAmount = Mathf.Min (FUEL_AMOUNT_MAX, currentFuelAmount);
	}

	// Machine Boost
	void RepairWorld() {
		opacitySupposed *= 0.75f;
		opacitySupposed = Mathf.Max (0, opacitySupposed);

		Color tempColor = pollutionColour.GetComponent<SpriteRenderer> ().color;
		tempColor.a = opacityCurrent;
		pollutionColour.GetComponent<SpriteRenderer> ().color = tempColor;

		gameObject.SendMessageUpwards ("SpawnLess", SendMessageOptions.DontRequireReceiver);
	}

	// Machine Boost
	void IncreaseStrength() {
		currentAttackRate += 0.3f;
	}

	// Machine Boost
	void IncreaseMaxFuel() {
		FUEL_AMOUNT_MAX += 10f;
	}

	void AddFuelAndPollution(float fuelAmount) {
		currentFuelAmount += fuelAmount;
		currentFuelAmount = Mathf.Min (FUEL_AMOUNT_MAX, currentFuelAmount);


		opacitySupposed = (opacitySupposed + 0.6f) / 2;

	}

	void PauseGame() {
		Time.timeScale = 0;
	}

	void SetInitialValues(float[] playerValues) {
		// 0 - CHARACTER_ATTACK_RATE_INITIAL, 
		// 1 - CHARACTER_MOVE_SPEED_INITIAL, 
		// 2 - CHARACTER_MOVE_SPEED_BOOST, 
		// 3 - FUEL_AMOUNT_DEPLETION_MOVING,
		// 4 - FUEL_AMOUNT_DEPLETION_STATIONARY,
		// 5 - FUEL_AMOUNT_INITIAL,
		// 6 - FUEL_AMOUNT_REPLENISH,
		// 7 - FUEL_AMOUNT_MAX,
		// 8 - CHARACTER_MOVE_SPEED_MAX

		if (playerValues.Length != 9) {
			Debug.Log ("EERRRORORORROROROR");
		}

		CHARACTER_ATTACK_RATE_INITIAL = playerValues [0];
		CHARACTER_MOVE_SPEED_INITIAL = playerValues [1];
		CHARACTER_MOVE_SPEED_BOOST = playerValues [2];
		CHARACTER_MOVE_SPEED_MAX = playerValues [8];
		FUEL_AMOUNT_DEPLETION_MOVING = playerValues [3];
		FUEL_AMOUNT_DEPLETION_STATIONARY = playerValues [4];
		FUEL_AMOUNT_INITIAL = playerValues [5];
		FUEL_AMOUNT_REPLENISH = playerValues [6];
		FUEL_AMOUNT_MAX = playerValues [7];
	}

	bool PlayerStartedMoving() {
		if  (Input.GetKeyDown(KeyCode.UpArrow)
			|| Input.GetKeyDown(KeyCode.DownArrow)
			|| Input.GetKeyDown(KeyCode.LeftArrow)
			|| Input.GetKeyDown(KeyCode.RightArrow)) {

			Time.timeScale = 1;
			playerStartsGame = true;
			return true;
		}

		return false;
	}
}
