using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ForegroundObject : MonoBehaviour {

	public List<Vector2Int> additionalCoords = new List<Vector2Int>();

	public abstract bool IsSteppable(MoveType moveType, Vector2Int incomingDisplacement);
	public abstract void OnInteraction(MoveType moveType, Vector2Int incomingDisplacement);

	public virtual bool CanBePushedInto(Vector2Int incomingDisplacement) {
		return false;
	}

	public virtual bool CanBeJumpedOver() {
		return false;
	}
}
