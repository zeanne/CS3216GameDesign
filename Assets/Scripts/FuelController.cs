using UnityEngine;
using System.Collections;

public class FuelController : MonoBehaviour {

	public float amount;

	void TakeFuel(GameObject player) {
		player.SendMessage ("AddFuel", amount);
		Destroy (this.gameObject);
	}
}
