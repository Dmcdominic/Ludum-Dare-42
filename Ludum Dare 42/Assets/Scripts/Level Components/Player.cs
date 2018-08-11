using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	// Settings
	private static readonly float moveTime = 0.5f;

	// Properties
	[HideInInspector]
	public Vector2Int posOnFloor;

	
	// Update is called once per frame
	void Update () {
		// TODO - player input
	}

	public bool canMove(Vector2Int displacement) {
		// TODO
		return false;
	}

	public void move(Vector2Int displacement) {
		// TODO - Play the desired animation
		
	}

	public void updatePosOnFloor() {
		posOnFloor.x = Mathf.RoundToInt(transform.position.x);
		posOnFloor.y = Mathf.RoundToInt(transform.position.y);
	}
}
