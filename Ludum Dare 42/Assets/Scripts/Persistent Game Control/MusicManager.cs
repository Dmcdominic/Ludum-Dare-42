using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

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

	
	public AudioSource a_s;

    public AudioClip world0track;
    public AudioClip world1track;
    public AudioClip world2track;
    public AudioClip world3track;

    private float pitch;
	//public GameObject source;
	public float max_music_volume;
	public float max_effects_volume;
	public static float global_pitch = 1;

    public UnityEvent to1;
    public UnityEvent to2;
    public UnityEvent to3;
    public UnityEvent toLast;

    //public int worldIndex;

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

        //worldIndex = GM.Instance.currentLevelManager.worldIndex;

        /*for (int i = 0; i < sxs.Length; i++) {
			GameObject g = Instantiate(source, transform);
			g.name = sxs[i].name;
			sxs[i].source = g.GetComponent<AudioSource>();
		}*/

        //a_s = this.GetComponent<AudioSource>();
        if (a_s)
        {
            Debug.Log("Found audiosource!");
        }
	}

    private void Start()
    {
        
    }

    void Update() {
		a_s.volume = max_music_volume;
		a_s.pitch = global_pitch;
		if (!a_s.isPlaying) a_s.Play();
	}

    public void ChangeToTutorialMusic()
    {
        a_s.clip = world0track;
        if (!a_s.isPlaying)
        {
            a_s.Play();
        }
    }
    public void ChangeToMusicOne()
    {
        //world0track.Stop();
        //world1track.Play();
        a_s.clip = world1track;
        if(!a_s.isPlaying)
        {
            a_s.Play();
        }
    }
    public void ChangeToMusicTwo()
    {
        // world1track.Stop();
        //world2track.Play();
        a_s.clip = world2track;
        if (!a_s.isPlaying)
        {
            a_s.Play();
        }
    }
    public void ChangeToMusicThree()
    {
        //world2track.Stop();
        //world3track.Play();
        a_s.clip = world3track;
        if (!a_s.isPlaying)
        {
            a_s.Play();
        }
    }
    public void ChangeToFinalMusic()
    {
        // world3track.Stop();
        a_s.Stop();
    }


    /*public void shot() {
		play_sound(0);
	}

	public void die() {
		play_sound(1);
	}
    */

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
