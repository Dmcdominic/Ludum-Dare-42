using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ForegroundObject : MonoBehaviour {

	public abstract bool IsSteppable(MoveType moveType, Vector2Int incomingPlayerDisplacement);

	public abstract void OnInteraction(MoveType moveType, Vector2Int incomingPlayerDisplacement);
}
