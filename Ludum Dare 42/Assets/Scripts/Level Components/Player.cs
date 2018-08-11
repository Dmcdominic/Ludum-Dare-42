using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveType { normal, jumpTwoTiles };

public class Player : MonoBehaviour {

	// Settings
	private static readonly float moveTime = 0.25f;
	private float timer = 0f;

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

		// Round the player's position to whole numbers
		Vector3 pos = transform.position;
		placeAtPosition(new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)));
	}

	// Player input management
	void Update () {
		if (timer > 0) {
			timer -= Time.deltaTime;
			return;
		}

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
				return (tile != null && tile.isSteppable() && (foregroundObject == null || foregroundObject.IsSteppable(moveType, displacement)));
			case (MoveType.jumpTwoTiles):
				// TODO
				return false;
			default:
				return false;
		}
	}

	public void move(MoveType moveType, Vector2Int displacement, Vector2Int targetPosition) {
		timer = moveTime;
		// TODO - Play the desired animation, and update the posOnFloor
		switch (moveType) {
			case (MoveType.normal):
				Tile prevTile = LevelManager.getTile(posOnFloor);
				prevTile.OnLeave();

				Tile nextTile = LevelManager.getTile(targetPosition);
				nextTile.OnStep();

				ForegroundObject foregroundObject = LevelManager.getForegroundObject(targetPosition);
				if (foregroundObject != null) {
					foregroundObject.OnInteraction(moveType, displacement);
				}

				// TODO - trigger player animation
				normalMoveAnim(displacement, targetPosition);
				break;
			case (MoveType.jumpTwoTiles):
				// TODO
				break;
		}
	}

	public void placeAtPosition(Vector2Int position) {
		transform.position = new Vector3(position.x, position.y, transform.position.z);
		posOnFloor = position;
	}


	// Animation management
	private void normalMoveAnim(Vector2Int displacement, Vector2Int targetPosition) {
		// For now:
		this.transform.position += new Vector3(displacement.x, displacement.y, 0);
		posOnFloor = targetPosition;
	}

	public void onAnimationEnd() {
		Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName("TODO"));
		
		//isAnimating = false;
	}

}
