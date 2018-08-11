using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : ForegroundObject {

	public List<KeycardColor> keycardsRequired;

	public override bool IsSteppable(MoveType moveType, Vector2Int incomingPlayerDisplacement) {
		return (LevelManager.areKeycardsCompleted(keycardsRequired));
	}

	public override void OnInteraction(MoveType moveType, Vector2Int incomingPlayerDisplacement) {
		gameObject.SetActive(false);
		// TODO - Add interact/ fadeout animation if desired
	}
}
