using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ForegroundObject : MonoBehaviour {

	public List<Sprite> worldSprites;

	protected SpriteRenderer sr;


	private void Awake() {
		sr = GetComponentInChildren<SpriteRenderer>();
	}

	protected void Start() {
		int worldIndex = GM.Instance.currentLevelManager.worldIndex;
		if (worldIndex >= worldSprites.Count) {
			Debug.LogError("Missing some worldsprites on: " + this.GetType());
		} else {
			sr.sprite = worldSprites[GM.Instance.currentLevelManager.worldIndex];
		}
	}

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
