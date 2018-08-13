using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum MoveType { normal, jumpTwoTiles };

public class Player : MonoBehaviour {

	// Settings
	private static readonly float moveTime = 0.35f;
	private float timer = 0f;

	// References
	private Animator animator;
	[SerializeField]
	private AnimationClip WalkLeft;

	// Properties
	[HideInInspector]
	public Vector2Int posRounded;

	[HideInInspector]
	public bool jumpTwoActivated = false;
    //private bool isAnimating = false;

    //Event fires when a successful step (one or two) is taken
    public UnityEvent OnSuccessfulStep = new UnityEvent();
    
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
		} else if (GM.Instance.getGameState() != GameState.Playing) {
			return;
		}

		//if (isAnimating) {
		//	return;
		//}

		MoveType moveType = jumpTwoActivated ? MoveType.jumpTwoTiles : MoveType.normal;
		int distance = jumpTwoActivated ? 2 : 1;

		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");
		if (horizontalInput > 0) {
			tryMove(moveType, new Vector2Int(distance, 0));
		} else if (horizontalInput < 0) {
			tryMove(moveType, new Vector2Int(-distance, 0));
		} else if (verticalInput > 0) {
			tryMove(moveType, new Vector2Int(0, distance));
		} else if (verticalInput < 0) {
			tryMove(moveType, new Vector2Int(0, -distance));
		}
	}


	// Player movement
	private bool tryMove(MoveType moveType, Vector2Int displacement) {
		Vector2Int targetPos = posRounded + displacement;
		switch (moveType) {
			case MoveType.normal:
				if (canMoveNormal(displacement, targetPos)) {
					move(moveType, displacement, targetPos);
                    OnSuccessfulStep.Invoke();
					return true;
				}
				break;
			case MoveType.jumpTwoTiles:
				Vector2Int jumpOverPos = posRounded + new Vector2Int(displacement.x / 2, displacement.y / 2);
				if (canMoveJumpTwoTiles(displacement, jumpOverPos, targetPos)) {
					move(moveType, displacement, targetPos);
                    OnSuccessfulStep.Invoke();
					return true;
				}
				break;
		}

		// TODO - play "invalid move" sound effect?
		
		return false;
	}

	public bool canMoveNormal(Vector2Int displacement, Vector2Int targetPos) {
		Tile tile = LevelManager.getTile(targetPos);
		ForegroundObject foregroundObj = LevelManager.getForegroundObject(targetPos);
		return ((tile != null && tile.isSteppable()) && (foregroundObj == null || foregroundObj.IsSteppable(MoveType.normal, displacement)));
	}

	public bool canMoveJumpTwoTiles(Vector2Int displacement, Vector2Int jumpOverPos, Vector2Int targetPos) {
		Tile tileToJump = LevelManager.getTile(jumpOverPos);
		ForegroundObject foregroundObjToJump = LevelManager.getForegroundObject(jumpOverPos);
		if ((tileToJump != null && !tileToJump.CanBeJumpedOver()) || (foregroundObjToJump != null && !foregroundObjToJump.CanBeJumpedOver())) {
			return false;
		}

		Tile tile = LevelManager.getTile(targetPos);
		ForegroundObject foregroundObj = LevelManager.getForegroundObject(targetPos);
		return ((tile != null && tile.isSteppable()) && (foregroundObj == null || foregroundObj.IsSteppable(MoveType.jumpTwoTiles, displacement)));
	}

	public void move(MoveType moveType, Vector2Int displacement, Vector2Int targetPosition) {
		timer = moveTime;

		Tile prevTile = LevelManager.getTile(posRounded);
		prevTile.OnLeave();
		Tile nextTile = LevelManager.getTile(targetPosition);
		nextTile.OnStep();

		ForegroundObject foregroundObject = LevelManager.getForegroundObject(targetPosition);
		if (foregroundObject != null) {
			foregroundObject.OnInteraction(moveType, displacement);
		}

		switch (moveType) {
			case (MoveType.normal):
				normalMoveAnim(displacement, targetPosition);
				break;
			case (MoveType.jumpTwoTiles):
				// TODO
				jumpTwoTilesAnim(displacement, targetPosition);
				break;
		}
	}

	public void placeAtPosition(Vector2Int position) {
		transform.position = new Vector3(position.x, position.y, transform.position.z);
		posRounded = position;
	}


	// Animation management
	private void normalMoveAnim(Vector2Int displacement, Vector2Int targetPosition) {
		// TODO - add animation
		// For now:
		this.transform.position += new Vector3(displacement.x, displacement.y, 0);
		posRounded = targetPosition;
	}

	private void jumpTwoTilesAnim(Vector2Int displacement, Vector2Int targetPosition) {
		// TODO - add animation
		// For now:
		this.transform.position += new Vector3(displacement.x, displacement.y, 0);
		posRounded = targetPosition;
	}

	//public void onAnimationEnd() {
	//	Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName("TODO"));
		
	//	//isAnimating = false;
	//}

}
