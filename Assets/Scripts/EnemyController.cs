using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

	public float INITIAL_HP;
	private float currentHp;

	private float createTime;
	private bool creating;

	void Start () {
		currentHp = INITIAL_HP;
		createTime = Time.time;
		creating = true;
	}

	void Update () {
		if (Time.time - createTime < 0.5) {
			Vector3 p = transform.position;
			p.x -= 2 * Time.deltaTime;
			p.y -= 6 * Time.deltaTime;
			transform.position = p;

			Color c = GetComponent<SpriteRenderer> ().color;
			c.a = (Time.time - createTime) / 0.85f;
			GetComponent<SpriteRenderer> ().color = c;

		} else {
			creating = false;
		}

		if (currentHp <= 0) {
			Destroy (this.gameObject);
		}
	}

	void LateUpdate() {
		if (creating) {
			return;
		}

		Color tempColor = this.gameObject.GetComponent<SpriteRenderer> ().color;
		tempColor.a = currentHp / INITIAL_HP;
		this.gameObject.GetComponent<SpriteRenderer> ().color = tempColor;
	}

	void TakeDamage(float playerAttackRate) {
		currentHp -= Time.deltaTime * playerAttackRate;
	}
}
