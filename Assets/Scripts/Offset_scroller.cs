// Taken from https://unity3d.com/learn/tutorials/topics/2d-game-creation/2d-scrolling-backgrounds?

using UnityEngine;
using System.Collections;

public class Offset_scroller : MonoBehaviour {

	public float scrollSpeed;
	private Vector2 savedOffset;

	void Start () {
		savedOffset = GetComponent<Renderer>().sharedMaterial.GetTextureOffset ("_MainTex");
	}

	void Update () {
		float y = Mathf.Repeat (Time.time * scrollSpeed, 1);
		Vector2 offset = new Vector2 (savedOffset.x, y);
		GetComponent<Renderer>().sharedMaterial.SetTextureOffset ("_MainTex", offset);
	}

	void OnDisable () {
		GetComponent<Renderer>().sharedMaterial.SetTextureOffset ("_MainTex", savedOffset);
	}
}
