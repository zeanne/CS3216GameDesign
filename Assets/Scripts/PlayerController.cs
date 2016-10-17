
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	public GameObject instructions;
	public GameObject machineMenuCanvas;
	public Slider fuelBar;

	private Rigidbody2D rb2d;

//	public Text countdown;
	public Text resultText;
//	public Text fuelText;

	private string TAG_ENEMY = "Enemy";
	private string TAG_FINISH = "Finish";

	private static float CHARACTER_ATTACK_RATE_INITIAL;
	private static float CHARACTER_MOVE_SPEED_INITIAL;
	private static float CHARACTER_MOVE_SPEED_BOOST;
	private static float FUEL_AMOUNT_DEPLETION_MOVING;
	private static float FUEL_AMOUNT_DEPLETION_STATIONARY;
	private static float FUEL_AMOUNT_INITIAL;
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

		SetCountText ();
		resultText.text = "";
		fuelText.text = currentFuelAmount.ToString ();
		gameEnded = false;
		machineMenuCanvas.gameObject.SetActive (false);
	}

	void Update() {
		if (instructions.gameObject.activeInHierarchy && Input.anyKeyDown) {
			instructions.gameObject.SetActive (false);
		}

		if (gameEnded && Input.GetKeyDown ("r")) {
			SceneManager.LoadScene ("ReachTheGoal");
		}
	}

	void LateUpdate() {
		SetCountText ();
		SetFuelText ();
	}

	void FixedUpdate() {
		if (instructions.gameObject.activeInHierarchy) {
			return;
		}
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

		other.gameObject.SendMessage ("TakeFuel", gameObject, SendMessageOptions.DontRequireReceiver);
	}

	void OnCollisionStay2D(Collision2D other) {
		other.gameObject.SendMessage ("TakeDamage", currentAttackRate, SendMessageOptions.DontRequireReceiver);
	}

//	private void SetCountText() {
//		enemyCount = GameObject.FindGameObjectsWithTag (TAG_ENEMY).Length;
//		countdown.text = "Count: " + enemyCount.ToString() + " obstacles left";
//	}

	private void SetFuelText() {
		fuelBar.value = currentFuelAmount / FUEL_AMOUNT_INITIAL;
//		fuelText.text = "Remaining fuel amount: " + currentFuelAmount.ToString ();
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
	}

	// Machine Boost
	void ReplenishFuel() {
		currentFuelAmount += FUEL_AMOUNT_REPLENISH;
	}

	void AddFuel(float fuelAmount) {
		currentFuelAmount += fuelAmount;
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
		// 6 - FUEL_AMOUNT_REPLENISH

		if (playerValues.Length != 7) {
			Debug.Log ("EERRRORORORROROROR");
		}

		CHARACTER_ATTACK_RATE_INITIAL = playerValues [0];
		CHARACTER_MOVE_SPEED_INITIAL = playerValues [1];
		CHARACTER_MOVE_SPEED_BOOST = playerValues [2];
		FUEL_AMOUNT_DEPLETION_MOVING = playerValues [3];
		FUEL_AMOUNT_DEPLETION_STATIONARY = playerValues [4];
		FUEL_AMOUNT_INITIAL = playerValues [5];
		FUEL_AMOUNT_REPLENISH = playerValues [6];
	}
}
