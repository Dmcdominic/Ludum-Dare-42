using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToMainMenu : MonoBehaviour {

	void Start () {
		StartCoroutine(returnAfterDelay());
	}
	
	IEnumerator returnAfterDelay() {
		yield return new WaitForSeconds(10);
		GM.changeScene(SceneType.MainMenu);
	}
}
