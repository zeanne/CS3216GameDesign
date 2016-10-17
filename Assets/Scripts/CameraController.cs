using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;       //Public variable to store a reference to the player game object

	// LateUpdate is called after Update each frame
	void LateUpdate () 
	{
		Vector3 offset = new Vector3 ();
		offset.z = -5;
		Vector3 newPosition = player.transform.position + offset;

		transform.position = newPosition;
	}

}
