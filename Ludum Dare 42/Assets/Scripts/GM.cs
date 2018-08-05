using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour {

	public enum Scene { Init, MainMenu };

	private static GM _instance;
    public static GM Instance {
        get {
            if (_instance == null) {
                _instance = new GM();
            }
            return _instance;
        }
    }
	
	private void Awake() {
		if (_instance != null && _instance != this) {
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}
		DontDestroyOnLoad(this);
	}
	
	public static void changeScene(Scene scene) {
		switch(scene) {
			case (Scene.Init):
				SceneManager.LoadScene(0);
				break;
			case (Scene.MainMenu):
				SceneManager.LoadScene(1);
				break;
		}
	}
}
