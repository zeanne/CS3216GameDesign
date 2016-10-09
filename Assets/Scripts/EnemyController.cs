using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

	public float hp;
//	public GameObject hpTextObject;

	private Text hpText;


	void Start () {
//		hpText = (Text)hpTextObject.GetComponent<Text> ();
	}

	void Update () {
		if (hp <= 0) {
			Destroy (this.gameObject);
//			hpText.text = "0";
		}
	}

	void OnCollisionStay2D() {
//		hpText.text = hp.ToString();
	}
}
