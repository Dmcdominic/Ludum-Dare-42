using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keycard : ForegroundObject {

	public KeycardColor color;

	public override bool IsSteppable(MoveType moveType, Vector2Int incomingDisplacement) {
		return true;
	}

	public override void OnInteraction(MoveType moveType, Vector2Int incomingDisplacement) {
		LevelManager.obtainKeycard(color);
		LevelManager.getFloor().updateForegroundObj(Floor.pos3dToVect2Int(transform.position), null);

		// TODO - Add interact / fadeout animation if desired
		// For now:
		gameObject.SetActive(false);
	}

	public override bool CanBePushedInto(Vector2Int incomingDisplacement) {
		return false;
	}

	public override bool CanBeJumpedOver() {
		return true;
	}
}
