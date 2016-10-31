using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour {

	GameObject tutText;
	GameObject player;

	float startFadeTime;
	float endFadeTime;

	bool started = false;
	bool fadingOut = false;
	bool fadingIn = false;

	bool sliderText = false;
	bool obsText = false;
	bool machineText = false;
	bool goalText = false;

	// Use this for initialization
	void Start () {
		tutText = GameObject.Find ("tutText");
		player = GameObject.Find ("Player");

		tutText.GetComponent<Text>().text = "<---  This is the fuel you have.\n      You use more fuel when moving\n\n\n\n\n\n\n      Use the arrow keys to move";
	
	}
	
	// Update is called once per frame
	void Update () {

		if ((!started) && (Input.anyKey)) {
			started = true;
			FadeOut ();
		}

		if (fadingOut && tutText.GetComponent<Text> ().color.a > 0) {
			Color tempColor = tutText.GetComponent<Text> ().color;
			tempColor.a = Mathf.Max (0, endFadeTime - Time.time);
			tutText.GetComponent<Text> ().color = tempColor;

		} else if (fadingOut && tutText.GetComponent<Text> ().color.a == 0) {
			Debug.Log ("asd");
			fadingOut = false;

		} else if (fadingIn && tutText.GetComponent<Text> ().color.a < 1) {
			Color tempColor = tutText.GetComponent<Text> ().color;
			tempColor.a = Mathf.Min (1, Time.time - startFadeTime);
			tutText.GetComponent<Text> ().color = tempColor;

		} else if (fadingIn && tutText.GetComponent<Text> ().color.a == 1) {
			fadingIn = false;
		} 

		if (player.transform.position.x > 15 && !machineText) {
			machineText = true;
			FadeIn ();

			tutText.GetComponent<Text> ().text = "\n\n\n\n\n\n\n\nThis machine makes things easier";

		} else if (player.transform.position.x > 30 && !obsText && tutText.GetComponent<Text> ().color.a == 1) {
			FadeOut ();

		} else if (player.transform.position.x > 40 && !obsText) {
			obsText = true;
			FadeIn ();
			tutText.GetComponent<Text> ().text = "This is a rock. Push to destroy";

		} else if (player.transform.position.x > 65 && !goalText && tutText.GetComponent<Text> ().color.a == 1) {
			FadeOut ();
		
		} else if (player.transform.position.x > 75 && !goalText) {
			goalText = true;
			FadeIn ();
			tutText.GetComponent<Text> ().text = "\n\n\n\n\nThis is the goal. Touch to go";
		}

	}

	void FadeIn() {
		fadingIn = true;
		startFadeTime = Time.time;
		endFadeTime = Time.time + 1;

		if (fadingOut) {
			fadingOut = false;

			Color tempColor = tutText.GetComponent<Text> ().color;
			tempColor.a = 0;
			tutText.GetComponent<Text> ().color = tempColor;
		}
	}

	void FadeOut() {
		fadingOut = true;
		startFadeTime = Time.time;
		endFadeTime = Time.time + 1;
	}
}
