using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keycard : ForegroundObject {

	public KeycardColor color;

	public Sprite keycardRed;
	public Sprite keycardGreen;
	public Sprite keycardBlue;
	public Sprite keycardYellow;

	[SerializeField]
	private ParticleSystem ps;


	private new void Start() {
		updateVisualColors(color);
	}

	private void updateVisualColors(KeycardColor color) {
		if (sr != null) {
			switch(color) {
				case KeycardColor.Red:
					sr.sprite = keycardRed;
					break;
				case KeycardColor.Green:
					sr.sprite = keycardGreen;
					break;
				case KeycardColor.Blue:
					sr.sprite = keycardBlue;
					break;
				case KeycardColor.Yellow:
					sr.sprite = keycardYellow;
					break;
			}
		}
		
		if (ps != null) {
			ParticleSystem.MainModule ma = ps.main;
			Color colorHex = PrefabManager.Instance.getColorHex(color);
			colorHex.a = 17f/256f;
			ma.startColor = colorHex;
		}
	}

	public override bool IsSteppable(MoveType moveType, Vector2Int incomingDisplacement) {
		return true;
	}

	public override void OnInteraction(MoveType moveType, Vector2Int incomingDisplacement) {
		LevelManager.obtainKeycard(color);
		Vector2Int truePos = (Floor.pos3dToVect2Int(transform.position));
		LevelManager.getFloor().updateFgGridForAllPos(null, truePos, additionalCoords, false);

		// TODO - Add interact / fadeout animation if desired
		// For now:
		gameObject.SetActive(false);
	}

	public override bool CanBePushedInto(Vector2Int incomingDisplacement) {
		return false;
	}

	public override bool CanBeJumpedOver() {
		return true;
	}
}
