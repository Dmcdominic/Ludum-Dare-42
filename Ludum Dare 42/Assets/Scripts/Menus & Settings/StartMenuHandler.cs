using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuHandler : MonoBehaviour {

	// Editor fields
	public Animator animator;
	//public GameObject MainMenuScreen;

	// Properties
	private bool initialAnimDone = false;


	private void Awake() {
		if (!initialAnimDone) {
			playInitialAnim();
		} else {
			playMainAnim();
		}
	}

	// Update is called once per frame
	private void Update() {
		//if (Input.anyKeyDown) {
		//	MainMenuScreen.SetActive(true);
		//}

		float pauseInput = Input.GetAxisRaw("Pause");
		if (pauseInput > 0) {
			skipToMenu();
		}
	}

	private void playInitialAnim() {
		animator.Play("Initial Anim");
		initialAnimDone = true;
	}

	private void playMainAnim() {
		animator.Play("Pan Down");
	}

	private void skipToMenu() {
		animator.Play("Businessmen");
	}

}
