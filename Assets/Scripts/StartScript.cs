using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScript : MonoBehaviour {

	public GameObject fadeOutCanvas;
	public GameObject storyText;

	GameObject[] planets;
	GameObject startText;
	GameObject selectText;
	GameObject selectTextBox;

	string[] planetText;
	int selected = 0;
	bool started = false;
	bool submenu = false;

	// Use this for initialization
	void Start () {
		startText = GameObject.FindGameObjectWithTag ("StartText");
		selectText = GameObject.FindGameObjectWithTag ("SelectText");
		planets = GameObject.FindGameObjectsWithTag ("Planet");

		planetText = new string[] {"Start Game", "Options", "Story", "Credits"};
		selectTextBox = GameObject.Find ("SelectTextBox");

		selectTextBox.gameObject.SetActive (false);
		storyText.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {

		if (submenu) {
			if (Input.GetKeyDown(KeyCode.Escape)) {
				selectText.GetComponent<Text>().text = planetText [selected];
				storyText.SetActive (false);
				submenu = false;
			}

		} else if (started) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				started = false;
				startText.gameObject.SetActive (true);
				foreach (GameObject planet in planets) {
					planet.GetComponent<Animation> ().enabled = false;
				}
				selectTextBox.gameObject.SetActive (false);
				selectText.GetComponent<Text> ().text = "";

				startText.GetComponent<Text> ().text = "\n- Press 'Enter' key to start -";

			} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				TogglePrev ();
			} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
				ToggleNext ();
			} else if (Input.GetKeyDown (KeyCode.Return)) {
				Action ();
			}
			
		} else {
			if (Input.GetKeyDown(KeyCode.Return)) {
				started = true;
				startText.GetComponent<Text> ().text = "Navigate with <  > arrow keys\n    Press 'ESC' to return";
				selectTextBox.gameObject.SetActive (true);
				ToggleNext ();

			} else {
				
			}
		}

	}

	void TogglePrev() {
		planets [selected].GetComponent<Animation> ().enabled = false;

		selected = (selected - 1 + planets.Length) % planets.Length;
		planets [selected].GetComponent<Animation> ().enabled = true;
		selectText.GetComponent<Text>().text = planetText [selected];
	}

	void ToggleNext() {
		if (selectText.GetComponent<Text>().text.Equals("")) {
			planets [0].gameObject.GetComponent<Animation> ().enabled = true;
			selectText.GetComponent<Text>().text = planetText [0];
			selected = 0;

		} else {
			planets [selected].GetComponent<Animation> ().enabled = false;

			selected = (selected + 1) % planets.Length;
			planets [selected].GetComponent<Animation> ().enabled = true;
			selectText.GetComponent<Text>().text = planetText [selected];
		}
	}

	void Action() {
		switch (selected) {
		case 0:
			fadeOutCanvas.SetActive (true);
			Invoke ("loadNextScene", .5f);
			break;
		case 1:
			submenu = true;
			selectText.GetComponent<Text> ().text = "None available :/";
			break;
		case 2: // story
			selectText.GetComponent<Text> ().text = "";
			storyText.SetActive (true);
			submenu = true;
			break;

		case 3: // credits
			selectText.GetComponent<Text> ().text = "Thanks\nto us!";
			submenu = true;
			break;

		default :
			break;
		}
	}

	void loadNextScene() {
		SceneManager.LoadScene ("ReachTheGoal");
	}

}
