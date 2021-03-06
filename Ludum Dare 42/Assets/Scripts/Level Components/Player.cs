﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum MoveType { normal, jumpTwoTiles };

public class Player : MonoBehaviour {

	// Settings
	private static readonly float invalidMoveSFXDelay = 0.1f;

	// References
	[SerializeField]
	private Animator animator;
	[SerializeField]
	private AnimationClip walkingRight;
	public static float moveAnimTime;

	// Properties
	[HideInInspector]
	public Vector2Int truePos;

	// Input control
	private float timer = 0f;
	private bool controlEnabled = true;
	private axisDir prevHorizontalAxisDir = axisDir.none;
	private axisDir prevVerticalAxisDir = axisDir.none;

	[HideInInspector]
	public bool jumpTwoActivated = false;
	

	//Event fires when a successful step (of any move type) is taken
	public UnityEvent StartSuccessfulStep = new UnityEvent();
    public UnityEvent CompleteSuccessfulStep = new UnityEvent();
    
	// Initialization
	private void Awake() {
		// Round the player's position to whole numbers
		Vector3 pos = transform.position;
		placeAtPosition(new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)));

		// Set the moveAnimTime, for global reference
		moveAnimTime = walkingRight.length;
	}

	// Player input management
	void Update () {
		float horizontalInput = Input.GetAxisRaw("Horizontal");
		float verticalInput = Input.GetAxisRaw("Vertical");
		//Debug.Log("Horizontal axis: " + horizontalInput);
		//Debug.Log("Vertical axis: " + verticalInput);

		// Distinct keypress/axis input check
		if (getAxisDir(horizontalInput) != prevHorizontalAxisDir) {
			prevHorizontalAxisDir = axisDir.none;
		}
		if (getAxisDir(verticalInput) != prevVerticalAxisDir) {
			prevVerticalAxisDir = axisDir.none;
		}

		// Movement delay management, for animation time
		if (timer > 0) {
			timer -= Time.deltaTime;
			return;
		}

		// Check if player movement should be allowed at all
		if (GM.Instance.getGameState() != GameState.Playing || !controlEnabled) {
			return;
		}

		// Determine the type of move and displacement magnitude
		MoveType moveType = jumpTwoActivated ? MoveType.jumpTwoTiles : MoveType.normal;
		int distance = jumpTwoActivated ? 2 : 1;

		// Attempt to move along the axis with the larger input
		if (Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput) && prevHorizontalAxisDir == axisDir.none) {
			if (horizontalInput > 0) {
				tryMove(moveType, new Vector2Int(distance, 0));
				prevHorizontalAxisDir = axisDir.pos;
			} else if (horizontalInput < 0) {
				tryMove(moveType, new Vector2Int(-distance, 0));
				prevHorizontalAxisDir = axisDir.neg;
			}
		} else if (Mathf.Abs(horizontalInput) < Mathf.Abs(verticalInput) && prevVerticalAxisDir == axisDir.none) {
			if (verticalInput > 0) {
				tryMove(moveType, new Vector2Int(0, distance));
				prevVerticalAxisDir = axisDir.pos;
			} else if (verticalInput < 0) {
				tryMove(moveType, new Vector2Int(0, -distance));
				prevVerticalAxisDir = axisDir.neg;
			}
		}

		// Previous movement system:

		//if (Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput)) {
		//	if (horizontalInput > 0) {
		//		tryMove(moveType, new Vector2Int(distance, 0));
		//	} else if (horizontalInput < 0) {
		//		tryMove(moveType, new Vector2Int(-distance, 0));
		//	}
		//} else {
		//	if (verticalInput > 0) {
		//		tryMove(moveType, new Vector2Int(0, distance));
		//	} else if (verticalInput < 0) {
		//		tryMove(moveType, new Vector2Int(0, -distance));
		//	}
		//}
	}

	// Axis direction input utility
	private enum axisDir { neg, none, pos };
	private axisDir getAxisDir(float input) {
		if (input < 0) {
			return axisDir.neg;
		} else if (input > 0) {
			return axisDir.pos;
		} else {
			return axisDir.none;
		}
	}

	// Player movement
	private bool tryMove(MoveType moveType, Vector2Int displacement) {
		timer = moveAnimTime;

		Vector2Int targetPos = truePos + displacement;
		switch (moveType) {
			case MoveType.normal:
				if (canMoveNormal(displacement, targetPos)) {
					move(moveType, displacement, targetPos);
					return true;
				}
				// Invalid normal move anim and sfx
				playerMoveAnim("Invalid Walking", displacement);
				MusicManager.play_with_delay("invalid_footstep", invalidMoveSFXDelay);
				return false;
			case MoveType.jumpTwoTiles:
				Vector2Int jumpOverPos = truePos + new Vector2Int(displacement.x / 2, displacement.y / 2);
				if (canMoveJumpTwoTiles(displacement, jumpOverPos, targetPos)) {
					move(moveType, displacement, targetPos);
					return true;
				}
				// Invalid coffee jumpp anim and sfx
				playerMoveAnim("Invalid Jumping", displacement);
				MusicManager.play_with_delay("invalid_footstep", invalidMoveSFXDelay);
				return false;
		}
		
		return false;
	}

	// Predicate to check if the player can move usign the normal movetype in a certain direction
	public bool canMoveNormal(Vector2Int displacement, Vector2Int targetPos) {
		if (Employee.willEmployeeMoveInto(targetPos) || Employee.willEmployeeSwapWith(truePos, targetPos)) {
			return false;
		}

		Tile tile = LevelManager.getTile(targetPos);
		ForegroundObject foregroundObj = LevelManager.getForegroundObject(targetPos);
		return ((tile != null && tile.isSteppable()) && (foregroundObj == null || foregroundObj.IsSteppable(MoveType.normal, displacement)));
	}

	// Predicate to check if the player can move using the jumpTwoTiles movetype in a certain direction
	public bool canMoveJumpTwoTiles(Vector2Int displacement, Vector2Int jumpOverPos, Vector2Int targetPos) {
		Tile tileToJump = LevelManager.getTile(jumpOverPos);
		ForegroundObject foregroundObjToJump = LevelManager.getForegroundObject(jumpOverPos);
		if ((tileToJump != null && !tileToJump.CanBeJumpedOver()) || (foregroundObjToJump != null && !foregroundObjToJump.CanBeJumpedOver())) {
			return false;
		}

		if (Employee.willEmployeeMoveInto(targetPos)) {
			return false;
		}

		Tile tile = LevelManager.getTile(targetPos);
		ForegroundObject foregroundObj = LevelManager.getForegroundObject(targetPos);
		return ((tile != null && tile.isSteppable()) && (foregroundObj == null || foregroundObj.IsSteppable(MoveType.jumpTwoTiles, displacement)));
	}

	// Moves the player using the given moveType, with the given displacement to the target position
	public void move(MoveType moveType, Vector2Int displacement, Vector2Int targetPosition) {
		StartSuccessfulStep.Invoke();

		Tile prevTile = LevelManager.getTile(truePos);
		prevTile.OnLeave();
		Tile nextTile = LevelManager.getTile(targetPosition);
		nextTile.OnStep();

		ForegroundObject foregroundObject = LevelManager.getForegroundObject(targetPosition);
		if (foregroundObject != null) {
			foregroundObject.OnInteraction(moveType, displacement);
		}

		switch (moveType) {
			case (MoveType.normal):
				if (foregroundObject is Pushable) {
					playerMoveAnim("Pushing", displacement);
				} else {
					playerMoveAnim("Walking", displacement);
				}
				placeByDisplacement(displacement);
				MusicManager.play_by_name("footstep");
				break;
			case (MoveType.jumpTwoTiles):
				playerMoveAnim("Jumping", displacement);
				MusicManager.play_by_name("coffee_jump");
				placeByDisplacement(displacement);
				break;
		}
		
		CompleteSuccessfulStep.Invoke();
	}

	// Move the player to a certain position and update their truePos
	private void placeAtPosition(Vector2Int position) {
		transform.position = new Vector3(position.x, position.y, transform.position.z);
		truePos = position;
	}
	private void placeByDisplacement(Vector2Int displacement) {
		placeAtPosition(truePos + displacement);
	}

	// Animation management
	private void playerMoveAnim(string animNamePrefix, Vector2Int displacement) {
		controlEnabled = false;
		StartCoroutine(enableControlDelayed());

		string animName = animNamePrefix + AnimUtil.getDirectionPostfix(displacement);
		animator.Play(animName);
	}

	// Re-enable player input after the movement animation delay
	IEnumerator enableControlDelayed() {
		yield return new WaitForSeconds(moveAnimTime);
		controlEnabled = true;
		yield return null;
	}

	public void disableControl() {
		controlEnabled = false;
	}

}
