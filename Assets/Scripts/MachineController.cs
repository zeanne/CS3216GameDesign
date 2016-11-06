using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MachineController : MonoBehaviour {

	private bool hasBeenTriggered = false;

	public GameObject sparksPrefab;
	private GameObject machineMenuCanvas;
	private GameObject player;

	private Button[] machineButtons = new Button[6];
	private int selected = 0;

	void Start() {
		InitialiseMenuObjects ();
	}

	void Update() {
		if (machineMenuCanvas.activeInHierarchy) {

			if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				ToggleLeft ();
			} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
				ToggleRight ();
			} else if (Input.GetKeyDown (KeyCode.UpArrow)) {
				ToggleVertical ();
			} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
				ToggleVertical ();
			} else if (Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown (KeyCode.Return)) {
				SelectButton ();
			}
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (hasBeenTriggered) {
			return;
		}

		PauseGame ();
		hasBeenTriggered = true;
		machineMenuCanvas.gameObject.SetActive (true);
		Instantiate (sparksPrefab, this.transform.position, Quaternion.identity);
	}

	void SetupButtons() {

		machineButtons[0] = GameObject.Find ("RefillFuelButton").GetComponent<Button> ();
		machineButtons[1] = GameObject.Find ("BoostSpeedButton").GetComponent<Button> ();
		machineButtons[2] = GameObject.Find ("DestroyRocksButton").GetComponent<Button> ();
		machineButtons[3] = GameObject.Find ("RepairWorldButton").GetComponent<Button> ();
		machineButtons[4] = GameObject.Find ("IncreaseStrengthButton").GetComponent<Button> ();
		machineButtons[5] = GameObject.Find ("BiggerTankButton").GetComponent<Button> ();

		AddButtonListeners ();

	}

	void AddButtonListeners() {

		machineButtons [0].onClick.RemoveAllListeners ();
		machineButtons [0].onClick.AddListener (closeMachineMenuCanvas);
		machineButtons [0].onClick.AddListener (SendMessageReplenishFuel);
		machineButtons [0].onClick.AddListener (ResumeGame);

		machineButtons [1].onClick.RemoveAllListeners ();
		machineButtons [1].onClick.AddListener (closeMachineMenuCanvas);
		machineButtons [1].onClick.AddListener (SendMessageIncreaseSpeed);
		machineButtons [1].onClick.AddListener (ResumeGame);

		machineButtons [2].onClick.RemoveAllListeners ();
		machineButtons [2].onClick.AddListener (closeMachineMenuCanvas);
		machineButtons [2].onClick.AddListener (DestroyRocksOnScreen);
		machineButtons [2].onClick.AddListener (ResumeGame);

		machineButtons [3].onClick.RemoveAllListeners ();
		machineButtons [3].onClick.AddListener (closeMachineMenuCanvas);
		machineButtons [3].onClick.AddListener (SendMessageRepairWorld);
		machineButtons [3].onClick.AddListener (ResumeGame);

		machineButtons [4].onClick.RemoveAllListeners ();
		machineButtons [4].onClick.AddListener (closeMachineMenuCanvas);
		machineButtons [4].onClick.AddListener (SendMessageIncreaseStrength);
		machineButtons [4].onClick.AddListener (ResumeGame);

		machineButtons [5].onClick.RemoveAllListeners ();
		machineButtons [5].onClick.AddListener (closeMachineMenuCanvas);
		machineButtons [5].onClick.AddListener (SendMessageIncreaseMaxFuel);
		machineButtons [5].onClick.AddListener (ResumeGame);

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
		player.SendMessage ("ReplenishFuel");
	}

	void SendMessageIncreaseSpeed() {
		player.SendMessage ("IncreaseMoveSpeed");
	}

	void SendMessageRepairWorld() {
		player.SendMessage ("RepairWorld");
	}

	void SendMessageIncreaseStrength() {
		player.SendMessage ("IncreaseStrength");
	}

	void SendMessageIncreaseMaxFuel() {
		player.SendMessage ("IncreaseMaxFuel");
	}

	void DestroyRocksOnScreen() {
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		GameObject e;

		for (int i = enemies.Length - 1; i >= 0; i--) {
			e = enemies [i];
			if (e.GetComponent<Renderer>().isVisible) {
				Destroy (e);
			}
		}
	}

	void InitialiseMenuObjects() {
		machineMenuCanvas = GameObject.Find ("MachineMenu");
		player = GameObject.Find ("Player");

		SetupButtons ();
//		noActionBtn = GameObject.Find ("DoNothingButton").GetComponent<Button> ();
//		addFuelBtn = GameObject.Find ("ReplenishFuelButton").GetComponent<Button> ();
//		addSpeedBtn = GameObject.Find ("BoostSpeedButton").GetComponent<Button> ();
//
//		AddButtonListeners ();

	}

	void ToggleVertical() {
		machineButtons [selected].GetComponent<Outline> ().enabled = false;

		switch (selected) {
		case 0:
			selected = 3;
			break;
		case 1:
			selected = 2;
			break;
		case 2:
			selected = 1;
			break;
		case 3:
			selected = 0;
			break;
		case 4:
			selected = 5;
			break;
		case 5:
			selected = 4;
			break;
		default:
			break;
		}

		machineButtons [selected].GetComponent<Outline> ().enabled = true;
	}

	void ToggleLeft() {
		machineButtons [selected].GetComponent<Outline> ().enabled = false;

		switch (selected) {
		case 0:
			selected = 4;
			break;
		case 1:
			selected = 3;
			break;
		case 2:
			selected = 0;
			break;
		case 3:
			selected = 5;
			break;
		case 4:
			selected = 2;
			break;
		case 5:
			selected = 1;
			break;
		default:
			break;
		}

		machineButtons [selected].GetComponent<Outline> ().enabled = true;
	}

	void ToggleRight() {
		machineButtons [selected].GetComponent<Outline> ().enabled = false;
		switch (selected) {
			case 0:
			selected = 2;
			break;
			case 1:
			selected = 5;
			break;
			case 2:
			selected = 4;
			break;
			case 3:
			selected = 1;
			break;
			case 4:
			selected = 0;
			break;
			case 5:
			selected = 3;
			break;
			default:
			break;
		}

		machineButtons [selected].GetComponent<Outline> ().enabled = true;
	}

	void SelectButton () {
		closeMachineMenuCanvas ();
		ResumeGame ();

		switch (selected) {
		case 0:
			SendMessageReplenishFuel ();
			break;
		case 1:
			SendMessageIncreaseSpeed ();
			break;
		case 2:
			DestroyRocksOnScreen ();
			break;
		case 3:
			SendMessageRepairWorld ();
			break;
		case 4:
			SendMessageIncreaseStrength ();
			break;
		case 5:
			SendMessageIncreaseMaxFuel ();
			break;
		default:
			break;
		}
	}
}
