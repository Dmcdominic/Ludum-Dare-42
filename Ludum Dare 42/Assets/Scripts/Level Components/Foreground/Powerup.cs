using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerupType { Coffee };

public class Powerup : ForegroundObject {

	public override bool IsSteppable(MoveType moveType, Vector2Int incomingPlayerDisplacement) {
		return true;
	}

	public override void OnInteraction(MoveType moveType, Vector2Int incomingPlayerDisplacement)
    {
        this.gameObject.SetActive(false);
        //TODO: Add interact / fadeout animation if desired
    }
}
