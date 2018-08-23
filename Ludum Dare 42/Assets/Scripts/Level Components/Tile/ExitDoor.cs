using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExitDoor : Tile {

	public List<Sprite> elevatorSprites;

	protected override void Start() {
		int worldIndex = GM.Instance.currentLevelManager.worldIndex;
		int levelIndex = GM.Instance.currentLevelManager.levelIndex;
		if (GM.isFinalLvlInWorld(worldIndex, levelIndex)) {
			if (worldIndex >= elevatorSprites.Count) {
				Debug.LogError("Missing some elevatorSprites.");
			} else if (sr) {
				sr.sprite = elevatorSprites[worldIndex];
			}
		} else {
			base.Start();
		}
	}

	public override bool isSteppable() {
		return true;
	}

    public override bool isSteppableForNPC()
    {
        return false;
    }

    public override void OnStep() {
        GM.onBeatLevel();
	}

	public override void OnLeave() {
		// Does not apply
	}

	public override bool CanBePushedOnto() {
		return false;
	}

	public override bool CanBeJumpedOver() {
		return false;
	}
}