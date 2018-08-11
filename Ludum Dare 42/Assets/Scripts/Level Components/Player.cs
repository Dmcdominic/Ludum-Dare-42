using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveType { normal, jumpTwoTiles };

public class Player : MonoBehaviour {

	// Settings
	//private static readonly float moveTime = 0.5f;

	// References
	private Animator animator;
	[SerializeField]
	private AnimationClip WalkLeft;

	// Properties
	[HideInInspector]
	public Vector2Int posOnFloor;

	//private bool isAnimating = false;

	
	// Initialization
	private void Awake() {
		animator = GetComponent<Animator>();
	}

	// Player input management
	void Update () {
		//if (isAnimating) {
		//	return;
		//}

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


	// Player movement
	private bool tryMove(MoveType moveType, Vector2Int displacement) {
		Vector2Int targetPosition = posOnFloor + displacement;
		if (canMove(moveType, displacement, targetPosition)) {
			move(moveType, displacement, targetPosition);
			return true;
		}
		return false;
	}

	public bool canMove(MoveType moveType, Vector2Int displacement, Vector2Int targetPosition) {
		switch (moveType) {
			case (MoveType.normal):
				Tile tile = LevelManager.getTile(targetPosition);
				ForegroundObject foregroundObject = LevelManager.getForegroundObject(targetPosition);
				return (tile.isSteppable() && foregroundObject.IsSteppable(moveType, displacement));
			case (MoveType.jumpTwoTiles):
				// TODO
				return false;
			default:
				return false;
		}
	}

	public void move(MoveType moveType, Vector2Int displacement, Vector2Int targetPosition) {
		// TODO - Play the desired animation, and update the posOnFloor
		switch (moveType) {
			case (MoveType.normal):
				Tile prevTile = LevelManager.getTile(posOnFloor);
				prevTile.OnLeave();

				Tile nextTile = LevelManager.getTile(targetPosition);
				nextTile.OnStep();

				ForegroundObject foregroundObject = LevelManager.getForegroundObject(targetPosition);
				foregroundObject.OnInteraction(moveType, displacement);

				// TODO - trigger player animation

				posOnFloor = targetPosition;
				break;
			case (MoveType.jumpTwoTiles):
				// TODO
				break;
		}
	}

	public void placeAtPosition(Vector2Int position) {
		transform.position = new Vector3(position.x, position.y, 0);
		posOnFloor = position;
	}


	// Animation management
	private void normalMoveAnim(MoveType moveType, Vector2Int displacement, Vector2Int targetPosition) {
		// For now:
		this.transform.position += new Vector3(displacement.x, displacement.y, 0);
	}

	public void onAnimationEnd() {
		Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName("TODO"));
		
		//isAnimating = false;
	}

}
