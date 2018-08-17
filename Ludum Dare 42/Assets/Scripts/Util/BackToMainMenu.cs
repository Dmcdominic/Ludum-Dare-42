using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToMainMenu : MonoBehaviour {

	public GameObject BackButton;

	void Start () {
		IngameCanvas.Instance.gameObject.SetActive(false);
		StartCoroutine(showBackButtonAfterDelay());
	}

	IEnumerator showBackButtonAfterDelay() {
		yield return new WaitForSeconds(14);
		BackButton.SetActive(true);
	}

	public void returnToMainMenu() {
		IngameCanvas.Instance.gameObject.SetActive(true);
		GM.changeScene(SceneType.MainMenu);
	}
}
