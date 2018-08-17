using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableTall : ForegroundObject {

	public override bool IsSteppable(MoveType moveType, Vector2Int incomingDisplacement) {
		return CanBePushedInto(incomingDisplacement);
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

		if (allTilesAreHoles(floor, targetPos)) {
			floor.updateFgGridForAllPos(null, basePos, additionalCoords, false);

			createSafeTileAt(floor, targetPos);
			foreach (Vector2Int relativePos in additionalCoords) {
				createSafeTileAt(floor, targetPos + relativePos);
			}
			// TODO - animation of object filling in hole?
			gameObject.SetActive(false);
			return;
		} else {
			// TODO - movement animation?
			transform.position = new Vector3(targetPos.x, targetPos.y, transform.position.z);
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

	// Returns true if all tiles are holes underneath the object, given its truePos
	private bool allTilesAreHoles(Floor floor, Vector2Int truePos) {
		Tile tile = floor.getTile(truePos);
		if (tile == null || !tile.isHole()) {
			return false;
		}

		foreach (Vector2Int relativePos in additionalCoords) {
			tile = floor.getTile(truePos + relativePos);
			if (tile == null || !tile.isHole()) {
				return false;
			}
		}

		return true;
	}

	private void createSafeTileAt(Floor floor, Vector2Int truePos) {
		Tile currentTile = floor.getTile(truePos);
		Destroy(currentTile.gameObject);

		SafeTile safeTile = Instantiate(PrefabManager.Instance.safeTile);
		safeTile.transform.position = Floor.Vect2IntToPos3d(truePos);
		floor.updateTile(truePos, safeTile);
	}

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
}
