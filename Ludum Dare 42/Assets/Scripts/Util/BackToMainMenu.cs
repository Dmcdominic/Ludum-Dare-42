using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToMainMenu : MonoBehaviour {

	void Start () {
		IngameCanvas.Instance.gameObject.SetActive(false);
		StartCoroutine(returnAfterDelay());
	}
	
	IEnumerator returnAfterDelay() {
		yield return new WaitForSeconds(10);
		IngameCanvas.Instance.gameObject.SetActive(true);
		GM.changeScene(SceneType.MainMenu);
	}
}
