using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : ForegroundObject {

	public List<KeycardColor> keycardsRequired;
	bool locked = true;

	public override bool IsSteppable(MoveType moveType, Vector2Int incomingPlayerDisplacement) {
		return !locked;
	}

	public override void OnInteraction(MoveType moveType, Vector2Int incomingPlayerDisplacement) {
		// Does not apply
	}

	public void tryToUnlock(KeycardColor color) {
		if (keycardsRequired.Remove(color) && keycardsRequired.Count == 0) {
			//gameObject.SetActive(false);
			Destroy(this.gameObject);
			locked = false;
			// TODO - Add interact/ fadeout animation if desired
		}
	}
}
