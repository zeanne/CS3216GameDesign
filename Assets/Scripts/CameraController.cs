using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

//	public GameObject player;
//
//	// Use this for initialization
//	void Start () {
//	
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//	}
//
//	void LateUpdate () {
//		transform.position = player.transform.position;
//	}



	public GameObject player;       //Public variable to store a reference to the player game object


	private Vector3 offset;         //Private variable to store the offset distance between the player and camera

	// Use this for initialization
	void Start () 
	{
		//Calculate and store the offset value by getting the distance between the player's position and camera's position.
		offset = transform.position - player.transform.position;
		offset.x -= 5;
	}

	// LateUpdate is called after Update each frame
	void LateUpdate () 
	{
		// Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
		Vector3 newPosition = player.transform.position + offset;
		newPosition.y = 0;
		if (newPosition.x < -20) {
			newPosition.x = -20;
		} else if (newPosition.x > 20) {
			newPosition.x = 20;
		}

		transform.position = newPosition;
	}

}
