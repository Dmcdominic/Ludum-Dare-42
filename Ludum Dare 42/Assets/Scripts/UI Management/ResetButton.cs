using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : MonoBehaviour {

	public void resetLevel() {
		if (GM.Instance.getGameState() != GameState.Playing) {
			return;
		}
		GM.resetCurrentLevel();
	}

	private void Update() {
		if (GM.Instance.getGameState() != GameState.Playing) {
			return;
		}
		if (Input.GetAxisRaw("Reset") > 0) {
			resetLevel();
		}
	}
}
