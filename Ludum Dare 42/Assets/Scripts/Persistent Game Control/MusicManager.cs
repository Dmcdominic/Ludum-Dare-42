using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class MusicManager : MonoBehaviour {

	// Audio clips
	public musicTrack mainMenuTrack;
	public List<musicTrack> worldTracks;
	public musicTrack endscreenTrack;

	[SerializeField]
	public sx[] sxs;


	// References
	public AudioSource music_as;
	public AudioSource sfx_proto_as;

	// Properties
	private musicTrack currentTrack;

	private float global_music_volume = 1;
	private float global_effects_volume = 1;
	private float global_pitch = 1;

	private float transition_volume_mult = 1;


	// Singleton instance setup
	private static MusicManager _instance;
	public static MusicManager Instance {
		get {
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType<MusicManager>();
				if (_instance == null) {
					Debug.LogError("No MusicManager found in the scene");
				}
			}
			return _instance;
		}
	}

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

		SceneManager.sceneLoaded += OnSceneLoaded;

		for (int i = 0; i < sxs.Length; i++) {
			GameObject g = Instantiate(sfx_proto_as.gameObject, transform);
			g.name = sxs[i].name;
			sxs[i].source = g.GetComponent<AudioSource>();
		}
	}

	// When each scene is loaded, the correct track should be played
	void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		musicTrack nextTrack = getTrackForSceneIndex(scene.buildIndex);
		changeMusicTrack(nextTrack);
	}

	// Returns the track that should be played for a certain scene
	public musicTrack getTrackForSceneIndex(int buildIndex) {
		int nextTrackIndex = GM.getWorldFromSceneIndex(buildIndex);
		if (nextTrackIndex >= 0) {
			return getWorldTrack(nextTrackIndex);
		} else if (nextTrackIndex == -1) {
			return mainMenuTrack;
		} else if (nextTrackIndex == -2) {
			return endscreenTrack;
		}
		Debug.LogError("No track found for scene buildIndex: " + buildIndex);
		return mainMenuTrack;
	}

	// Helper function for getTrackForScene.
	// Returns the track for a certain world.
	private musicTrack getWorldTrack(int worldIndex) {
		if (worldIndex >= 0 && worldIndex < worldTracks.Count) {
			return worldTracks[worldIndex];
		}
		Debug.LogError("No track found for world: " + worldIndex);
		return worldTracks[0];
	}


	// You should go through this method in order to change the track at any time
	public void changeMusicTrack(musicTrack track) {
		currentTrack = track;
		music_as.clip = track.clip;

		refreshVolumes();

		if (!music_as.isPlaying) {
			music_as.Play();
		}
	}


	// Play SFX
	public static void play_by_name(string name) {
		for (int i = 0; i < Instance.sxs.Length; i++) {
			if (Instance.sxs[i].name == name)
				play_sound(i);
		}
	}

	public static void play_with_delay(string name, float delay) {
		for (int i = 0; i < Instance.sxs.Length; i++) {
			if (Instance.sxs[i].name == name)
				play_sound_delayed(i, delay);
		}
	}

	private static void play_sound(int id) {
		AudioSource source = Instance.sxs[id].source;
		source.pitch = ((Random.value - .5f) * Instance.sxs[id].variation + Instance.sxs[id].mid) * Instance.global_pitch;
		source.PlayOneShot(Instance.sxs[id].clip, Instance.sxs[id].volume * Instance.global_effects_volume);
	}
	private static void play_sound_delayed(int id, float delay) {
		AudioSource source = Instance.sxs[id].source;
		source.clip = Instance.sxs[id].clip;
		source.pitch = ((Random.value - .5f) * Instance.sxs[id].variation + Instance.sxs[id].mid) * Instance.global_pitch;
		source.volume = Instance.sxs[id].volume * Instance.global_effects_volume;
		source.PlayDelayed(delay);
	}

	// Update music and sfx volumes, and immediately apply changes to all audio sources
	public void updateGlobalVolumes(float musicVolume, float sfxVolume) {
		global_music_volume = musicVolume;
		global_effects_volume = sfxVolume;
		refreshVolumes();
	}

	// update the transition volume multiplier, and immediately apply changes to all audio sources
	public void updateTransitionVolumeMult(float transitionVolumeMult) {
		if (transitionVolumeMult < 0) {
			transitionVolumeMult = 0;
		}
		transition_volume_mult = transitionVolumeMult;
		refreshVolumes();
	}

	// Re-calculates and applies the true volume for the music audiosource and all sfx audiosources
	private void refreshVolumes() {
		music_as.volume = global_music_volume * currentTrack.volume * transition_volume_mult;
		for (int i = 0; i < Instance.sxs.Length; i++) {
			sxs[i].source.volume = sxs[i].volume * global_effects_volume * transition_volume_mult;
		}
	}

	// Util
	public musicTrack getCurrentTrack() {
		return currentTrack;
	}

	public float getTransitionVolumeMult() {
		return transition_volume_mult;
	}

}

// Struct for music AudioClips
[System.Serializable]
public struct musicTrack {
	[SerializeField]
	public AudioClip clip;

	[Range(0, 3)]
	public float volume;
}

// Struct for SoundFX AudioClips
[System.Serializable]
public struct sx {
	public string name;
	public float variation;
	public float mid;
	public AudioClip clip;

	[Range(0, 3)]
	public float volume;

	[HideInInspector]
	public AudioSource source;
}
