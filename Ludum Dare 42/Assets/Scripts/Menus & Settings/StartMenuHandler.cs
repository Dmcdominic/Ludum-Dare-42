using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuHandler : MonoBehaviour {

	// Editor fields
	public Animator animator;
	public GameObject MainMenuScreen;

	// Properties
	public static bool initialAnimDone = false;


	private void Awake() {
		if (!initialAnimDone) {
			playInitialAnim();
		} else {
			playMainAnim();
		}
	}

	// Update is called once per frame
	private void Update() {
		if (Input.anyKeyDown) {
			//this.gameObject.SetActive(false);
			MainMenuScreen.SetActive(true);
		}
	}

	private void playInitialAnim() {
		animator.Play("Initial Anim");
		initialAnimDone = true;
	}

	private void playMainAnim() {
		animator.Play("Pan Down");
		//animator.Play("Businessmen");
	}

}
