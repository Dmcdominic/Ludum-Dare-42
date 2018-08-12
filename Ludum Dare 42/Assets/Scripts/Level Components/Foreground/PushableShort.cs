using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableShort : ForegroundObject {

	public override bool IsSteppable(MoveType moveType, Vector2Int incomingDisplacement) {
		return CanBePushedInto(incomingDisplacement);
	}

	public override void OnInteraction(MoveType moveType, Vector2Int incomingDisplacement) {
		Vector2Int pos = Floor.pos3dToVect2Int(transform.position);
		Vector2Int targetPos = pos + incomingDisplacement;

		// TODO - movement animation?
		transform.position = new Vector3(targetPos.x, targetPos.y, transform.position.z);
		
		Floor floor = LevelManager.getFloor();
		floor.updateForegroundObj(pos, null);

		ForegroundObject foregroundObj = floor.getForegroundObj(targetPos);
		if (foregroundObj != null) {
			foregroundObj.OnInteraction(moveType, incomingDisplacement);
		}

		Tile tile = floor.getTile(targetPos);
		if (tile is ChangeableTile) {
			ChangeableTile changeableTile = (ChangeableTile)tile;
			if (changeableTile.stepsRemaining == 0) {
				// TODO - animation of object falling into hole
				floor.updateForegroundObj(targetPos, null);
				gameObject.SetActive(false);
				return;
			}
		}
		floor.updateForegroundObj(targetPos, this);
	}

	public override bool CanBePushedInto(Vector2Int incomingDisplacement) {
		Vector2Int pos = Floor.pos3dToVect2Int(transform.position);
		Vector2Int targetPos = pos + incomingDisplacement;

		Floor floor = LevelManager.getFloor();
		Tile tile = floor.getTile(targetPos);
		ForegroundObject foregroundObj = floor.getForegroundObj(targetPos);

		return ((tile != null && tile.CanBePushedOnto()) &&
				(foregroundObj == null || foregroundObj.CanBePushedInto(incomingDisplacement)));
	}

	public override bool CanBeJumpedOver() {
		return true;
	}
}
