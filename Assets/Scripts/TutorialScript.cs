using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour {

	GameObject tutText;

	public GameObject enemy;
	public GameObject fuel;
	public GameObject goal;
	GameObject machine;
	GameObject last;

	float startFadeTime;
	float endFadeTime;

	bool started = false;
	bool fadingOut = false;
	bool fadingIn = false;


	void Start () {
		tutText = GameObject.Find ("tutText");

		machine = GameObject.FindGameObjectWithTag ("Machine");

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
			tempColor.a = Mathf.Max (0, (endFadeTime - Time.time) / (endFadeTime - startFadeTime));
			tutText.GetComponent<Text> ().color = tempColor;

		} else if (fadingOut && tutText.GetComponent<Text> ().color.a == 0) {
			fadingOut = false;

		} else if (fadingIn && tutText.GetComponent<Text> ().color.a < 1) {
			Color tempColor = tutText.GetComponent<Text> ().color;
			tempColor.a = Mathf.Min (1, (Time.time - startFadeTime) / (endFadeTime - startFadeTime));
			tutText.GetComponent<Text> ().color = tempColor;

		} else if (fadingIn && tutText.GetComponent<Text> ().color.a == 1) {
			fadingIn = false;
		} 

		if (machine.gameObject.GetComponent<Renderer> ().isVisible) {
			tutText.GetComponent<Text> ().text = "\n\n\n\n\n\n\n\nThis machine makes things easier";
			ShowText (machine);


		} else if (fuel.gameObject.GetComponent<Renderer> ().isVisible) {
			tutText.GetComponent<Text> ().text = "This is fuel. \nCollecting this causes damage to the environment";
			ShowText (fuel);
		
		} else if (goal.gameObject.GetComponent<Renderer> ().isVisible) {
			tutText.GetComponent<Text> ().text = "\n\n\n\n\nThis is the goal. Touch to proceed.";
			ShowText (goal);

		} else if (enemy != null && enemy.gameObject.GetComponent<Renderer> ().isVisible) {
			tutText.GetComponent<Text> ().text = "This is a rock. Push to destroy";
			ShowText (enemy);

		} else if (enemy == null && last != null) {

		} else {
			HideText();
		}
	}

	void FadeIn() {
		
		fadingIn = true;
		startFadeTime = Time.time;
		endFadeTime = Time.time + 0.5f;

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
		endFadeTime = Time.time + 0.5f;
	}

	void ShowText(GameObject obj) {
		if (last == obj) {
		} else {
			FadeIn ();
			last = obj;
		}
	}

	void HideText() {
		if (last != null) {
			FadeOut ();
			last = null;

		} else if (tutText.GetComponent<Text> ().color.a == 1) {
			FadeOut ();
		} 
	}
}
