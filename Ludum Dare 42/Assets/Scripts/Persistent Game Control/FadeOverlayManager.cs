using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOverlayManager : MonoBehaviour {

	private Animator animator;

	[SerializeField]
	private AnimationClip FadeToBlack;
	[SerializeField]
	private AnimationClip Loading;
	[SerializeField]
	private AnimationClip FadeFromBlack;

	// Singleton management
	private static FadeOverlayManager _instance;
	public static FadeOverlayManager Instance { get { return _instance; } }

	private void Awake() {
		if (_instance != null && _instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			_instance = this;
		}

		if (transform.parent == null) {
			DontDestroyOnLoad(this);
		}

		animator = GetComponent<Animator>();
	}

	public void fadeToBlack() {
		animator.Play("FadeToBlack");
	}

	public void loading() {
		animator.Play("Loading");
	}

	public void fadeFromBlack() {
		animator.Play("FadeFromBlack");
	}

}
