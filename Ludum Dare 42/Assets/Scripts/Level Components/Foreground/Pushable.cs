﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : ForegroundObject {

	// Editor settings
	public bool tall;

	// References
	[SerializeField]
	private Animator animator;
	private Coroutine sortingLayerCoroutine;


	public override bool IsSteppable(MoveType moveType, Vector2Int incomingDisplacement) {
		return moveType == MoveType.normal && CanBePushedInto(incomingDisplacement);
	}

	// Move the whole pushable
	public override void OnInteraction(MoveType moveType, Vector2Int incomingDisplacement) {
		Floor floor = LevelManager.getFloor();
		Vector2Int basePos = Floor.pos3dToVect2Int(transform.position);
		Vector2Int targetPos = basePos + incomingDisplacement;

		pushNextIfNotThis(floor, moveType, basePos, incomingDisplacement);
		// Push other objects that get affected
		foreach (Vector2Int relativePos in additionalCoords) {
			pushNextIfNotThis(floor, moveType, basePos + relativePos, incomingDisplacement);
		}

		transform.position = new Vector3(targetPos.x, targetPos.y, transform.position.z);
		if (allTilesAreHoles(floor, targetPos)) {
			floor.updateFgGridForAllPos(null, basePos, additionalCoords, false);
			if (tall) {
				createSafeTileAt(floor, targetPos);
				foreach (Vector2Int relativePos in additionalCoords) {
					createSafeTileAt(floor, targetPos + relativePos);
				}
				playFillInHoleAnim(incomingDisplacement);
			} else {
				playFallInHoleAnim(incomingDisplacement);
			}
		} else {
			playPushedAnim(incomingDisplacement);
			floor.updateFgGridForAllPos(null, basePos, additionalCoords, false);
			floor.updateFgGridForAllPos(this, targetPos, additionalCoords, false);
		}
	}

	private void pushNextIfNotThis(Floor floor, MoveType moveType, Vector2Int truePos, Vector2Int displacement) {
		Vector2Int targetPos = truePos + displacement;
		ForegroundObject foregroundObj = floor.getForegroundObj(targetPos);
		if (foregroundObj != null && foregroundObj != this) {
			foregroundObj.OnInteraction(moveType, displacement);
		}
	}

	// Predicate to check if this can be pushed a certain direction
	public override bool CanBePushedInto(Vector2Int incomingDisplacement) {
		Vector2Int basePos = Floor.pos3dToVect2Int(transform.position);
		if (!posCanBePushedInto(basePos, incomingDisplacement)) {
			return false;
		}

		foreach (Vector2Int relativePos in additionalCoords) {
			if (!posCanBePushedInto(basePos + relativePos, incomingDisplacement)) {
				return false;
			}
		}

		return true;
	}

	// Private predicate to check if a particular position in this pushable can be
	// pushed to the respective position given incoming displacement
	private bool posCanBePushedInto(Vector2Int truePos, Vector2Int incomingDisplacement) {
		Vector2Int targetPos = truePos + incomingDisplacement;
		Floor floor = LevelManager.getFloor();

		Tile tile = floor.getTile(targetPos);
		if (tile == null || !tile.CanBePushedOnto()) {
			return false;
		}

		ForegroundObject foregroundObj = floor.getForegroundObj(targetPos);
		if (foregroundObj != null && (foregroundObj != this && !foregroundObj.CanBePushedInto(incomingDisplacement))) {
			return false;
		}

		return true;
	}

	// Returns true if all tiles are holes underneath the object, given its truePos
	private bool allTilesAreHoles(Floor floor, Vector2Int truePos) {
		Tile tile = floor.getTile(truePos);
		if (tile == null || !(tile is Hole)) {
			return false;
		}

		foreach (Vector2Int relativePos in additionalCoords) {
			tile = floor.getTile(truePos + relativePos);
			if (tile == null || !(tile is Hole)) {
				return false;
			}
		}

		return true;
	}

	// Creates a safe tile at the given true position
	private void createSafeTileAt(Floor floor, Vector2Int truePos) {
		Tile currentTile = floor.getTile(truePos);
		Destroy(currentTile.gameObject, Player.moveAnimTime);

		SafeTile safeTile = Instantiate(PrefabManager.Instance.safeTile);
		safeTile.transform.position = Floor.Vect2IntToPos3d(truePos);
		floor.updateTile(truePos, safeTile);
		safeTile.hideForStepDuration();
	}

	// Pushables can be jumped over
	public override bool CanBeJumpedOver() {
		return true;
	}

	// Animation management
	private void playPushedAnim(Vector2Int displacement) {
		string animName = "Pushed" + AnimUtil.getDirectionPostfix(displacement);
		animator.Play(animName);

		if (displacement.y < 0) {
			sr.sortingLayerName = "Player";
			sr.sortingOrder = 1;
			if (sortingLayerCoroutine != null) {
				StopCoroutine(sortingLayerCoroutine);
			}
			sortingLayerCoroutine = StartCoroutine(returnToSortingLayerDelayed());
		}
	}

	// Set the sprite renderer's sorting layer back to foreground, after the standard move anim time
	IEnumerator returnToSortingLayerDelayed() {
		yield return new WaitForSeconds(Player.moveAnimTime);
		yield return new WaitForEndOfFrame();
		sr.sortingLayerName = "Foreground";
		sr.sortingOrder = 0;
		yield return null;
	}

	private void playFillInHoleAnim(Vector2Int displacement) {
		playPushedAnim(displacement);
		// Insert hole fill-in animation here
		StartCoroutine(disableGameObjectDelayed());
	}
	private void playFallInHoleAnim(Vector2Int displacement) {
		playPushedAnim(displacement);
		// Insert falling-into-hole anim here
		StartCoroutine(disableGameObjectDelayed());
	}

	// Disable the gameobject, after the standard move anim time
	IEnumerator disableGameObjectDelayed() {
		yield return new WaitForSeconds(Player.moveAnimTime);
		yield return new WaitForEndOfFrame();
		gameObject.SetActive(false);
	}
}
