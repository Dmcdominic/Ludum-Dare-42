using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerupType { Coffee };

public class Powerup : ForegroundObject {

	public PowerupType type;

	private new void Start() {
		// Do nothing
	}

	public override bool IsSteppable(MoveType moveType, Vector2Int incomingDisplacement) {
		return true;
	}

	public override void OnInteraction(MoveType moveType, Vector2Int incomingDisplacement) {
		LevelManager.applyPowerup(type);
        Destroy(this.gameObject);
		//TODO: Add interact / fadeout animation if desired
	}

	public override bool CanBePushedInto(Vector2Int incomingDisplacement) {
		return false;
	}

	public override bool CanBeJumpedOver() {
		return true;
	}
}