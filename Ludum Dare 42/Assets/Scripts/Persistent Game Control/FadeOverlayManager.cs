using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeOverlayManager : MonoBehaviour {

	private Animator animator;

	[SerializeField]
	private AnimationClip FadeToBlack;
	[SerializeField]
	private AnimationClip Loading;
	[SerializeField]
	private AnimationClip FadeFromBlack;

	[HideInInspector]
	public int nextSceneIndex = 1;

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
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	public void fadeToBlack(int sceneIndex) {
		nextSceneIndex = sceneIndex;
		animator.SetBool("DoneLoading", false);
		animator.Play("FadeToBlack");
	}

	public void onFadeToBlackFinished() {
		SceneManager.LoadScene(nextSceneIndex);
	}

	public void onFadeFromBlackFinished() {
		GM.Instance.refreshGamestate();
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		animator.SetBool("DoneLoading", true);
	}

}
