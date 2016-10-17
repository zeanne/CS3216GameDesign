using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MachineController : MonoBehaviour {

	private bool hasBeenTriggered = false;

	public GameObject sparksPrefab;
	private GameObject machineMenuCanvas;
	private GameObject player;
	private Button noActionBtn;
	private Button addFuelBtn;
	private Button addSpeedBtn;

	void Start() {
		machineMenuCanvas = GameObject.Find ("MachineMenu");
		player = GameObject.Find ("Player");
//		noActionBtn = machineMenuCanvas.GetComponentsInChildren<Button> ()[0];
		noActionBtn = GameObject.Find ("DoNothingButton").GetComponent<Button> ();
		addFuelBtn = GameObject.Find ("ReplenishFuelButton").GetComponent<Button> ();
		addSpeedBtn = GameObject.Find ("BoostSpeedButton").GetComponent<Button> ();

		AddButtonListeners ();
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (hasBeenTriggered) {
			return;
		}

//		Vector3 currentPosition = this.transform.position;
//		currentPosition.z = -1;
		PauseGame ();
		hasBeenTriggered = true;
		machineMenuCanvas.gameObject.SetActive (true);
		Instantiate (sparksPrefab, this.transform.position, Quaternion.identity);
	}

	void SetupButtons() {
		noActionBtn = GameObject.Find ("DoNothingButton").GetComponent<Button> ();
		addFuelBtn = GameObject.Find ("ReplenishFuelButton").GetComponent<Button> ();
		addSpeedBtn = GameObject.Find ("BoostSpeedButton").GetComponent<Button> ();

		AddButtonListeners ();

	}

	void AddButtonListeners() {
		noActionBtn.onClick.AddListener (closeMachineMenuCanvas);
		noActionBtn.onClick.AddListener (ResumeGame);

		addFuelBtn.onClick.AddListener (closeMachineMenuCanvas);
		addFuelBtn.onClick.AddListener (SendMessageReplenishFuel);
		addFuelBtn.onClick.AddListener (ResumeGame);

		addSpeedBtn.onClick.AddListener (closeMachineMenuCanvas);
		addSpeedBtn.onClick.AddListener (SendMessageIncreaseSpeed);
		addSpeedBtn.onClick.AddListener (ResumeGame);
	}

	void closeMachineMenuCanvas() {
		machineMenuCanvas.gameObject.SetActive (false);
	}

	void PauseGame() {
		Time.timeScale = 0;
	}

	void ResumeGame() {
		Time.timeScale = 1;
	}

	void SendMessageReplenishFuel() {
		player.gameObject.SendMessage ("ReplenishFuel");
	}

	void SendMessageIncreaseSpeed() {
		player.gameObject.SendMessage ("IncreaseMoveSpeed");
	}
}
