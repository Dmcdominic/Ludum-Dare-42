using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : ForegroundObject {

	public List<KeycardColor> keycardsRequired;

	public override bool IsSteppable(MoveType moveType, Vector2Int incomingDisplacement) {
		return false;
	}

	public override void OnInteraction(MoveType moveType, Vector2Int incomingDisplacement) {
		// Does not apply
	}

	public override bool CanBePushedInto(Vector2Int incomingDisplacement) {
		return false;
	}

	public void tryToUnlock(KeycardColor color) {
		if (keycardsRequired.Remove(color) && keycardsRequired.Count == 0) {
			LevelManager.getFloor().updateForegroundObj(Floor.pos3dToVect2Int(transform.position), null);

			// TODO - Add interact / fadeout animation if desired
			// For now:
			gameObject.SetActive(false);
		}
	}
}
