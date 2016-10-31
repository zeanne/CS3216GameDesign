
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

//	public GameObject instructions;
	public GameObject machineMenuCanvas;
	public Slider fuelBar;

	private Rigidbody2D rb2d;

	public Text resultText;

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

	public float currentFuelAmount;
	public float currentMoveSpeed;
	public float currentAttackRate;

	private bool gameEnded = false;
	private int enemyCount;


	void Start() {
		Time.timeScale = 1;

		rb2d = GetComponent<Rigidbody2D> ();
		SetUpVariables ();

	}

	void SetUpVariables() {
		currentFuelAmount = FUEL_AMOUNT_INITIAL;
		currentMoveSpeed = CHARACTER_MOVE_SPEED_INITIAL;
		currentAttackRate = CHARACTER_ATTACK_RATE_INITIAL;

		resultText.text = "";
		gameEnded = false;
		machineMenuCanvas.gameObject.SetActive (false);
	}

	void Update() {
//		if (instructions.gameObject.activeInHierarchy && Input.anyKeyDown) {
//			instructions.gameObject.SetActive (false);
//		}

		if (gameEnded && Input.GetKeyDown ("r")) {
			SceneManager.LoadScene (SceneManager.GetActiveScene().name);
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

		currentFuelAmount -= Time.deltaTime * FUEL_AMOUNT_DEPLETION_STATIONARY;

		if (moveDistance.magnitude != 0) {
			rb2d.MovePosition (oldPosition + moveDistance);
			currentFuelAmount -= Time.deltaTime * FUEL_AMOUNT_DEPLETION_MOVING;
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.CompareTag (TAG_FINISH)) {
			WinGame ();
			return;
		}

		if (other.gameObject.CompareTag (TAG_FUEL)) {
			other.gameObject.SendMessage ("TakeFuel", gameObject, SendMessageOptions.DontRequireReceiver);
			gameObject.SendMessageUpwards ("SpawnMore");
		}
	}

	void OnCollisionStay2D(Collision2D other) {
		other.gameObject.SendMessage ("TakeDamage", currentAttackRate, SendMessageOptions.DontRequireReceiver);
	}

	private void SetFuelBar() {
		fuelBar.value = currentFuelAmount / FUEL_AMOUNT_INITIAL;
		if (currentFuelAmount <= 0) {
			LoseGame ();
		}
	}

	private void SetGameEndStatus() {
		gameEnded = true;
		PauseGame ();
	}

	void LoseGame() {
		resultText.text = "You lose!\nPress 'r' for replay.";
		SetGameEndStatus ();
	}

	void WinGame() {
		resultText.text = "You win!\nPress 'r' for replay.";
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

	void AddFuel(float fuelAmount) {
		currentFuelAmount += fuelAmount;
		currentFuelAmount = Mathf.Min (FUEL_AMOUNT_MAX, currentFuelAmount);

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
}
