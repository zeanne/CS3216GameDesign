using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	public float attackRate;
	public float moveSpeed;

	public Text countdown;
	public Text winText;

	private Rigidbody2D rb2d;
	private bool won = false;
	private int enemyCount;

	private string TAG_ENEMY = "Enemy";
	private string TAG_FINISH = "Finish";


	void Start()
	{
		rb2d = GetComponent<Rigidbody2D> ();
		setCountText ();
		winText.text = "";
		won = false;
	}

	void Update() {
		if (won && Input.GetKeyDown ("r")) {
			SceneManager.LoadScene ("testfloor");
		}
	}

	void LateUpdate() {
		setCountText ();
	}
		
	void FixedUpdate()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector2 moveTo = new Vector2 (moveHorizontal * moveSpeed + transform.position.x, 
			moveVertical * moveSpeed + transform.position.y);

		rb2d.MovePosition(moveTo);
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.CompareTag (TAG_FINISH)) {
			winText.gameObject.SetActive (true);	

			winText.text = "You win!\nPress 'r' for replay.";
			won = true;
			moveSpeed = 0;
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
}
