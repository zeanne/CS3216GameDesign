using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

	public float INITIAL_HP;
	private float currentHp;

	void Start () {
		currentHp = INITIAL_HP;
	}

	void Update () {
		if (currentHp <= 0) {
			Destroy (this.gameObject);
		}
	}

	void LateUpdate() {
		Color tempColor = this.gameObject.GetComponent<SpriteRenderer> ().color;
		tempColor.a = currentHp / INITIAL_HP;
		this.gameObject.GetComponent<SpriteRenderer> ().color = tempColor;
	}

	void OnCollisionStay2D(Collision2D other) {
		currentHp -= Time.deltaTime * other.gameObject.GetComponent<PlayerController> ().currentAttackRate;
	}
}
