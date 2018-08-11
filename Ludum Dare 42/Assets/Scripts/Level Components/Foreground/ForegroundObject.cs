using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ForegroundObject : MonoBehaviour {

	public abstract bool IsSteppable(Vector2Int incomingPlayerDisplacement);

	public abstract void OnInteraction(Vector2Int incomingPlayerDisplacement);
}
