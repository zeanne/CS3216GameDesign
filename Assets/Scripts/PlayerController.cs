using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	public GameObject instructions;

	public float attackRate;
	public float moveSpeed;
	public float fuelAmount;

	public Text countdown;
	public Text winText;
	public Text fuelText;

	private Rigidbody2D rb2d;
	private bool gameEnded = false;
	private int enemyCount;

	public float fuelDepletionRateMoving;
	public float fuelDepletionRateStationary;

	private string TAG_ENEMY = "Enemy";
	private string TAG_FINISH = "Finish";
	private string TAG_FUEL = "Fuel";


	void Start()
	{
		rb2d = GetComponent<Rigidbody2D> ();
		setCountText ();
		winText.text = "";
		fuelText.text = fuelAmount.ToString ();
		gameEnded = false;

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
		setCountText ();
		setFuelText ();
	}
		
	void FixedUpdate()
	{
		if (instructions.gameObject.activeInHierarchy) {
			return;
		}
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 oldPosition = transform.position;
		Vector3 moveDistance = new Vector3 (moveHorizontal * moveSpeed, moveVertical * moveSpeed);

		if (moveDistance.magnitude == 0) {			
			fuelAmount -= fuelDepletionRateStationary;

		} else {
			rb2d.MovePosition (oldPosition + moveDistance);
			fuelAmount -= moveDistance.magnitude * fuelDepletionRateMoving;
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.CompareTag (TAG_FINISH)) {
			winText.gameObject.SetActive (true);	

			winText.text = "You win!\nPress 'r' for replay.";
			gameEnd ();

		} else if (other.gameObject.CompareTag (TAG_FUEL)) {
			fuelAmount += other.gameObject.GetComponent<FuelController> ().amount;
			Destroy (other.gameObject);
		}
	}

	void OnCollisionStay2D(Collision2D other) {
		if (other.gameObject.CompareTag (TAG_ENEMY)) {
			other.gameObject.GetComponent<EnemyController> ().hp -= Time.deltaTime * attackRate ;
		}
	}

	void setCountText() {
		enemyCount = GameObject.FindGameObjectsWithTag (TAG_ENEMY).Length;
		countdown.text = "Count: " + enemyCount.ToString() + " obstacles left";
	}

	void setFuelText() {
		fuelText.text = fuelAmount.ToString ();
		if (fuelAmount <= 0) {
			winText.text = "You lose!\nPress 'r' for replay.";
			gameEnd ();
		}
	}

	void moveUsingFuel() {
		
	}

	void gameEnd() {
		gameEnded = true;
		moveSpeed = 0;
		fuelDepletionRateStationary = 0;
	}
}
