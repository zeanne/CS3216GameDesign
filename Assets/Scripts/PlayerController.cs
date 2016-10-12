using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	public GameObject instructions;
	public GameObject machineMenuCanvas;

	private Rigidbody2D rb2d;

	public Text countdown;
	public Text resultText;
	public Text fuelText;

	private string TAG_ENEMY = "Enemy";
	private string TAG_FINISH = "Finish";

	private static float CHARACTER_ATTACK_RATE_INITIAL = 2f;
	private static float CHARACTER_MOVE_SPEED_INITIAL = 0.2f;
	private static float FUEL_AMOUNT_DEPLETION_MOVING = 0.1f;
	private static float FUEL_AMOUNT_DEPLETION_STATIONARY = 0.01f;
	private static float FUEL_AMOUNT_INITIAL = 10f;
	private static float FUEL_AMOUNT_REPLENISH = 5f;

	public float currentFuelAmount = FUEL_AMOUNT_INITIAL;
	public float currentMoveSpeed = CHARACTER_MOVE_SPEED_INITIAL;
	public float currentAttackRate = CHARACTER_ATTACK_RATE_INITIAL;

	private bool gameEnded = false;
	private int enemyCount;


	void Start() {
		rb2d = GetComponent<Rigidbody2D> ();

		SetUpVariables ();
	}

	void SetUpVariables() {
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

		currentFuelAmount -= FUEL_AMOUNT_DEPLETION_STATIONARY;

		if (moveDistance.magnitude != 0) {
			rb2d.MovePosition (oldPosition + moveDistance);
			currentFuelAmount -= moveDistance.magnitude * FUEL_AMOUNT_DEPLETION_MOVING;
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

	private void SetCountText() {
		enemyCount = GameObject.FindGameObjectsWithTag (TAG_ENEMY).Length;
		countdown.text = "Count: " + enemyCount.ToString() + " obstacles left";
	}

	private void SetFuelText() {
		fuelText.text = "Remaining fuel amount: " + currentFuelAmount.ToString ();
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

	void IncreaseMoveSpeed() {
		currentMoveSpeed += 0.2f;
	}

	void ReplenishFuel() {
		currentFuelAmount += FUEL_AMOUNT_REPLENISH;
	}

	void AddFuel(float fuelAmount) {
		currentFuelAmount += fuelAmount;
	}

	void PauseGame() {
		Time.timeScale = 0;
	}
}
