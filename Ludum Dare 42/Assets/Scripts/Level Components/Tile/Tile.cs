﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour {

    public abstract bool isSteppable();
    public abstract bool isSteppableForNPC();
    public abstract void OnStep();
    public abstract void OnLeave();

	public List<Sprite> worldSprites;

	protected SpriteRenderer sr;


	protected virtual void Awake() {
		sr = GetComponentInChildren<SpriteRenderer>();
	}

	protected virtual void Start() {
		int worldIndex = GM.Instance.currentLevelManager.worldIndex;
		if (worldIndex >= worldSprites.Count) {
			Debug.LogError("Missing some worldsprites on: " + this.GetType());
		} else if (sr) {
			sr.sprite = worldSprites[worldIndex];
		}
	}

	public virtual bool CanBePushedOnto() {
		return false;
	}

	public virtual bool CanBeJumpedOver() {
		return true;
	}

	public virtual void onAboveTileUpdated(Tile aboveTile) {
	}

}
