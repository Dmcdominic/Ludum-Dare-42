using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : ForegroundObject {

	public List<KeycardColor> keycardsRequired;

	[SerializeField]
	private SpriteRenderer sr;

	private void Start() {
		updateVisualColors(keycardsRequired[0]);
	}

	private void updateVisualColors(KeycardColor color) {
		if (sr != null) {
			sr.color = PrefabManager.Instance.getColorHex(color);
		}
	}

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
			Vector2Int truePos = (Floor.pos3dToVect2Int(transform.position));
			LevelManager.getFloor().updateFgGridForAllPos(null, truePos, additionalCoords, false);

			// TODO - Add interact / fadeout animation if desired
			// For now:
			gameObject.SetActive(false);
		}
	}
}
