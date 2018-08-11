using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveType { normal, jumpTwoTiles };

public class Player : MonoBehaviour {

	// Settings
	//private static readonly float moveTime = 0.5f;

	// Properties
	[HideInInspector]
	public Vector2Int posOnFloor;

	private bool isAnimating = false;

	
	// Update is called once per frame
	void Update () {
		if (isAnimating) {
			return;
		}

		// TODO - player input
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");
		if (horizontalInput > 0) {
			tryMove(MoveType.normal, new Vector2Int(1, 0));
		} else if (horizontalInput < 0) {
			tryMove(MoveType.normal, new Vector2Int(-1, 0));
		} else if (verticalInput > 0) {
			tryMove(MoveType.normal, new Vector2Int(0, 1));
		} else if (verticalInput < 0) {
			tryMove(MoveType.normal, new Vector2Int(0, -1));
		}
	}

	private bool tryMove(MoveType moveType, Vector2Int displacement) {
		if (canMove(moveType, displacement, posOnFloor + displacement)) {
			move(moveType, displacement);
			return true;
		}
		return false;
	}

	public bool canMove(MoveType moveType, Vector2Int displacement, Vector2Int tilePosition) {
		if (moveType == MoveType.normal) {
			Tile tile = LevelManager.getTile(tilePosition);
			//ForegroundObject foregroundObject = LevelManager.
			return tile.isSteppable();
		} else if (moveType == MoveType.jumpTwoTiles) {
			// TODO
		}
		return false;
	}


	public void move(MoveType moveType, Vector2Int displacement) {
		// TODO - Play the desired animation, and update the posOnFloor
		
	}

	//public void updatePosOnFloor() {
	//	posOnFloor.x = Mathf.RoundToInt(transform.position.x);
	//	posOnFloor.y = Mathf.RoundToInt(transform.position.y);
	//}
}
