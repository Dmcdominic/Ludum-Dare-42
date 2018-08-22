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

	// Properties
	private int nextSceneIndex = 1;
	private Coroutine trackFadeCoroutine;


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
		fadeTrackOutIfNeeded(sceneIndex);

		animator.SetBool("DoneLoading", false);
		animator.Play("FadeToBlack");
	}

	// Music fade-out management
	private void fadeTrackOutIfNeeded(int nextSceneBuildIndex) {
		resetTrackFade();
		musicTrack currentTrack = MusicManager.Instance.getCurrentTrack();
		musicTrack nextTrack = MusicManager.Instance.getTrackForSceneIndex(nextSceneBuildIndex);
		//if (currentTrack.clip.name != nextTrack.clip.name) {
		if (currentTrack.clip != nextTrack.clip) {
			trackFadeCoroutine = StartCoroutine(fadeTrackOut());
		}
	}

	IEnumerator fadeTrackOut() {
		float incr = 1f / FadeToBlack.length;
		float currentTVM = MusicManager.Instance.getTransitionVolumeMult();
		while (currentTVM > 0) {
			currentTVM = currentTVM - incr * Time.deltaTime;
			MusicManager.Instance.updateTransitionVolumeMult(currentTVM);
			yield return new WaitForSeconds(Time.deltaTime);
		}
		yield return null;
	}

	public void resetTrackFade() {
		if (trackFadeCoroutine != null) {
			StopCoroutine(trackFadeCoroutine);
			trackFadeCoroutine = null;
		}
		MusicManager.Instance.updateTransitionVolumeMult(1f);
	}

	public void onFadeToBlackFinished() {
		SceneManager.LoadScene(nextSceneIndex);
	}

	public void onFadeFromBlackFinished() {
		GM.Instance.refreshGamestate();
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		animator.SetBool("DoneLoading", true);
		resetTrackFade();
		GM.Instance.refreshGamestate();
	}

}
