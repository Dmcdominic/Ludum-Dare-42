using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

public class MusicManager : MonoBehaviour {

	public AudioClip mainMenuTrack;
	public List<AudioClip> worldTracks;

	[SerializeField]
	public sx[] sxs;

	[HideInInspector]
	public AudioSource a_s;

	private float pitch;
	public GameObject source;
	public float max_music_volume;
	public float max_effects_volume;
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
	
		if (SettingsManager.Instance) {
			SettingsManager.Instance.applyToMusicManager();
		}

		a_s = GetComponent<AudioSource>();
		a_s.clip = mainMenuTrack;
		
		a_s.volume = max_music_volume;
		a_s.Play();


		for (int i = 0; i < sxs.Length; i++) {
			GameObject g = Instantiate(source, transform);
			g.name = sxs[i].name;
			sxs[i].source = g.GetComponent<AudioSource>();
		}

	}

	void Update() {
		a_s.volume = max_music_volume;
		a_s.pitch = global_pitch;
		if (!a_s.isPlaying) a_s.Play();
	}


	public void shot() {
		play_sound(0);
	}

	public void die() {
		play_sound(1);
	}


	public static void play_by_name(string name) {
		for (int i = 0; i < _instance.sxs.Length; i++) {
			if (_instance.sxs[i].name == name)
				play_sound(i);
		}
	}

	public static void play_sound(int id) {
		_instance.sxs[id].source.pitch = ((Random.value - .5f) * _instance.sxs[id].variation + _instance.sxs[id].mid) * global_pitch;
		_instance.sxs[id].source.PlayOneShot(_instance.sxs[id].clip, _instance.sxs[id].volume * _instance.max_effects_volume);
	}

}
