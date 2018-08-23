using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorTransition : MonoBehaviour {

	public static int nextWorldIndex;
	public static int nextLevelIndex;

	public void toNextScene() {
		GM.changeToLevelSceneDirect(nextWorldIndex, nextLevelIndex);
	}

}
