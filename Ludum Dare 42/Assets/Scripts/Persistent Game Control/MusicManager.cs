﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class MusicManager : MonoBehaviour {

	// Audio clips
	public musicTrack mainMenuTrack;
	public List<musicTrack> worldTracks;

	[SerializeField]
	public sx[] sxs;


	// References
	public AudioSource a_s;

	// Properties
    private float pitch;
	[HideInInspector]
	public float global_music_volume = 1;
	[HideInInspector]
	public float global_effects_volume = 1;
	public static float global_pitch = 1;


    // Singleton instance setup
    private static MusicManager _instance;
	public static MusicManager Instance { get { return _instance; } }

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
	
		//if (SettingsManager.Instance) {
		//	SettingsManager.Instance.applyToMusicManager();
		//}
		
		changeMusicTrack(mainMenuTrack);

        /*for (int i = 0; i < sxs.Length; i++) {
			GameObject g = Instantiate(source, transform);
			g.name = sxs[i].name;
			sxs[i].source = g.GetComponent<AudioSource>();
		}*/
	}

	// You should go through this method in order to change the track at any time
	public void changeMusicTrack(musicTrack track) {
		if (!a_s.clip == track.clip) {
			a_s.clip = track.clip;
		}

		a_s.volume = global_music_volume * track.volume;

		if (!a_s.isPlaying) {
			a_s.Play();
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

	public static void play_sound(int id) {
		_instance.sxs[id].source.pitch = ((Random.value - .5f) * _instance.sxs[id].variation + _instance.sxs[id].mid) * global_pitch;
		_instance.sxs[id].source.PlayOneShot(_instance.sxs[id].clip, _instance.sxs[id].volume * _instance.global_effects_volume);
	}

	// Update music volume
	public static void updateMusicVolume(float newGlobalVolume) {
		// TODO - update the volume from the SettingsManager
	}

}

[System.Serializable]
public struct musicTrack {
	[SerializeField]
	public AudioClip clip;

	[Range(0, 3)]
	public float volume;
}

[System.Serializable]
public struct sx {
	[SerializeField]
	public string name;

	[SerializeField]
	public float variation;

	[SerializeField]
	public float mid;

	[SerializeField]
	public AudioClip clip;

	[Range(0, 3)]
	public float volume;

	[HideInInspector]
	public AudioSource source;
}
