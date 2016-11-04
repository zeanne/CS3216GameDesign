using UnityEngine;
using System.Collections;

public class FuelController : MonoBehaviour {

	public float amount;
	public Sprite disabled;

	bool active = true;

	void TakeFuel(GameObject player) {
		if (active) {
			active = false;
			player.SendMessage ("AddFuelAndPollution", amount);
			this.GetComponent<SpriteRenderer> ().sprite = disabled;

		}
	}
}
