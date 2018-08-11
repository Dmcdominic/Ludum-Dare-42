using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keycard : ForegroundObject {

	public KeycardColor color;

	public override bool IsSteppable(MoveType moveType, Vector2Int incomingPlayerDisplacement) {
		return true;
	}

	public override void OnInteraction(MoveType moveType, Vector2Int incomingPlayerDisplacement) {
		LevelManager.obtainKeycard(color);
		this.gameObject.SetActive(false);
		// TODO - Add interact / fadeout animation if desired
	}
}
