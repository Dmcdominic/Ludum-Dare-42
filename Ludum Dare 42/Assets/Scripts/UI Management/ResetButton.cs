using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : MonoBehaviour {

	private void Update() {
		//Debug.Log(GM.Instance.getGameState());

		if (GM.Instance.getGameState() != GameState.Playing) {
			return;
		}
		if (Input.GetAxisRaw("Reset") > 0) {
			resetLevel();
		}
	}

	public void resetLevel() {
		if (GM.Instance.getGameState() != GameState.Playing) {
			return;
		}
		GM.resetCurrentLevel();
	}

}
