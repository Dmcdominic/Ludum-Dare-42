using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : Tile {

	public Sprite hole;
	public List<Sprite> holeShaded;


	private new void Start() {
		checkForSpriteUpdate();
		Vector2Int truePos = Floor.pos3dToVect2Int(this.transform.position);
		LevelManager.getFloor().updateTile(truePos, this);
	}

	public override bool isSteppable() {
		return false;
	}

	public override bool isSteppableForNPC() {
		return true;
	}

	public override void OnStep() {
		// Does not apply
	}

	public override void OnLeave() {
		// Does not apply
	}

	public override bool CanBePushedOnto() {
		return true;
	}

	// Methods to keep the hole sprites correct between shaded/unshaded
	public override void onAboveTileUpdated(Tile aboveTile) {
		checkForSpriteUpdate(aboveTile);
	}

	private void checkForSpriteUpdate() {
		Vector2Int truePos = Floor.pos3dToVect2Int(this.transform.position);
		Vector2Int aboveTruePos = truePos + new Vector2Int(0, 1);
		this.checkForSpriteUpdate(LevelManager.getFloor().getTile(aboveTruePos));
	}

	private void checkForSpriteUpdate(Tile aboveTile) {
		if (aboveTile != null && aboveTile is Hole) {
			sr.sprite = hole;
		} else {
			sr.sprite = holeShaded[GM.Instance.currentLevelManager.worldIndex];
		}
	}

}
