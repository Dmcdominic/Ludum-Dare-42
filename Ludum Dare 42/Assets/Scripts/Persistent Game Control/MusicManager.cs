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
	private float pitch;
	private float trackVolume;
	[HideInInspector]
	public float global_music_volume;
	[HideInInspector]
	public float global_effects_volume;
	public static float global_pitch = 1;


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
		int nextTrackIndex = GM.getWorldFromScene(scene);
		if (nextTrackIndex >= 0) {
			changeToWorldTrack(nextTrackIndex);
		} else if (nextTrackIndex == -1) {
			changeMusicTrack(mainMenuTrack);
		} else if (nextTrackIndex == -2) {
			changeMusicTrack(endscreenTrack);
		}
	}

	// You should go through this method in order to change the track at any time
	public void changeMusicTrack(musicTrack track) {
		music_as.clip = track.clip;

		trackVolume = track.volume;
		music_as.volume = global_music_volume * trackVolume;

		if (!music_as.isPlaying) {
			music_as.Play();
		}
	}

	public void changeToWorldTrack(int worldIndex) {
		if (worldIndex >= 0 && worldIndex < worldTracks.Count) {
			changeMusicTrack(worldTracks[worldIndex]);
		}
	}

	// Play SFX
	public static void play_by_name(string name) {
		for (int i = 0; i < _instance.sxs.Length; i++) {
			if (_instance.sxs[i].name == name)
				play_sound(i);
		}
	}

	public static void play_with_delay(string name, float delay) {
		for (int i = 0; i < _instance.sxs.Length; i++) {
			if (_instance.sxs[i].name == name)
				play_sound_delayed(i, delay);
		}
	}

	private static void play_sound(int id) {
		AudioSource source = _instance.sxs[id].source;
		source.pitch = ((Random.value - .5f) * _instance.sxs[id].variation + _instance.sxs[id].mid) * global_pitch;
		source.PlayOneShot(_instance.sxs[id].clip, _instance.sxs[id].volume * _instance.global_effects_volume);
	}
	private static void play_sound_delayed(int id, float delay) {
		AudioSource source = _instance.sxs[id].source;
		source.clip = _instance.sxs[id].clip;
		source.pitch = ((Random.value - .5f) * _instance.sxs[id].variation + _instance.sxs[id].mid) * global_pitch;
		source.volume = _instance.sxs[id].volume * _instance.global_effects_volume;
		source.PlayDelayed(delay);
	}

	// Update music and sfx volumes, and immediately apply changes to all audio sources
	public static void updateVolumes(float musicVolume, float sfxVolume) {
		Instance.global_music_volume = musicVolume;
		Instance.global_effects_volume = sfxVolume;

		Instance.music_as.volume = Instance.global_music_volume * Instance.trackVolume;
		for (int i = 0; i < _instance.sxs.Length; i++) {
			Instance.sxs[i].source.volume = Instance.sxs[i].volume * Instance.global_effects_volume;
		}
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
